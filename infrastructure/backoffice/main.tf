terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "2.99.0"
    }
  }
}

provider "azurerm" {
  features {}
}

data "azurerm_client_config" "current" {

}

// Random password generator
resource "random_password" "pwd" {
  length           = 16
  special          = true
  override_special = "_%@"
}

locals {
  prefix      = lower("advcore${substr(terraform.workspace, 0, 3)}")
  db_admin    = "sqldbadmin"
  db_password = random_password.pwd.result
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-advcore-containerapps"
  location = "West Europe"

  tags = {
    "project"     = "Adventure Works Core"
    "environment" = terraform.workspace
  }
}

resource "azurerm_mssql_server" "mssql" {
  name                         = "${local.prefix}dbserver"
  resource_group_name          = azurerm_resource_group.rg.name
  location                     = azurerm_resource_group.rg.location
  version                      = "12.0"
  administrator_login          = local.db_admin
  administrator_login_password = local.db_password
  minimum_tls_version          = "1.2"

  tags = azurerm_resource_group.rg.tags
}

resource "azurerm_mssql_database" "mssql" {
  name        = "advdb"
  server_id   = azurerm_mssql_server.mssql.id
  sku_name    = "Basic"
  sample_name = "AdventureWorksLT"

  tags = azurerm_resource_group.rg.tags
}

resource "azurerm_sql_firewall_rule" "allow_all_azure_ips" {
  name                = "AllowAllAzureIps"
  resource_group_name = azurerm_resource_group.rg.name
  server_name         = azurerm_mssql_server.mssql.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_application_insights" "portal" {
  name                = "${local.prefix}insights"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  application_type    = "web"
}

resource "azurerm_app_service_plan" "portal" {
  name                = "${local.prefix}plan"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_app_service" "portal" {
  name                = "${local.prefix}app"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  app_service_plan_id = azurerm_app_service_plan.portal.id

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.portal.instrumentation_key
  }

  connection_string {
    name  = "ApplicationDbConnection"
    type  = "SQLServer"
    value = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault.portal.vault_uri}secrets/ApplicationDbConnection)"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_key_vault" "portal" {
  name                        = "${local.prefix}vault"
  location                    = azurerm_resource_group.rg.location
  resource_group_name         = azurerm_resource_group.rg.name
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    secret_permissions = [
      "Set",
      "Get",
      "List",
      "Delete",
      "Purge",
      "Recover"
    ]
  }
}

resource "azurerm_key_vault_secret" "portal" {
  name         = "ApplicationDbConnection"
  value        = "Server=tcp:${azurerm_mssql_server.mssql.fully_qualified_domain_name},1433;Initial Catalog=advdb;Persist Security Info=False;User ID=${local.db_admin};Password=${local.db_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  key_vault_id = azurerm_key_vault.portal.id
}

resource "azurerm_key_vault_access_policy" "portalPolicy" {
  key_vault_id = azurerm_key_vault.portal.id
  tenant_id    = azurerm_app_service.portal.identity[0].tenant_id
  object_id    = azurerm_app_service.portal.identity[0].principal_id

  depends_on = [
    azurerm_app_service.portal,
    azurerm_key_vault.portal
  ]

  secret_permissions = [
    "Get",
  ]
}
