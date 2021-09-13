terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=2.63.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "rg-shared-resources"
    storage_account_name = "advtfstatestorage001"
    container_name       = "tfstate"
    key                  = "dev-tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "random_integer" "random" {
  min = 1
  max = 50000
}

// Random password generator
resource "random_password" "random" {
  length           = 16
  special          = true
  override_special = "_%@"
}

data "azurerm_client_config" "current" {

}

locals {
  db_admin    = "sqldbadmin"
  db_password = random_password.random.result
}

resource "azurerm_mssql_server" "mssql" {
  name                         = format("%vadvdbserver", var.prefix)
  resource_group_name          = var.resourceGroupName
  location                     = var.location
  version                      = "12.0"
  administrator_login          = local.db_admin
  administrator_login_password = local.db_password
}

resource "azurerm_mssql_database" "advdb" {
  name        = "advdb"
  server_id   = azurerm_mssql_server.mssql.id
  collation   = "SQL_Latin1_General_CP1_CI_AS"
  sku_name    = "Basic"
  sample_name = "AdventureWorksLT"
}

resource "azurerm_application_insights" "portal" {
  name                = format("%vadvappinsights", var.prefix)
  resource_group_name = var.resourceGroupName
  location            = var.location
  application_type    = "web"
}

resource "azurerm_app_service_plan" "portal" {
  name                = format("%vadvlinuxappserviceplan", var.prefix)
  resource_group_name = var.resourceGroupName
  location            = var.location
  kind                = "Linux"
  reserved            = true

  sku {
    tier = "Standard"
    size = "B1"
  }
}

resource "azurerm_app_service" "portal" {
  name                = format("%vadvappservice", var.prefix)
  resource_group_name = var.resourceGroupName
  location            = var.location
  app_service_plan_id = azurerm_app_service_plan.portal.id

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.portal.instrumentation_key
  }

  connection_string {
    name  = "DefaultConnectionString"
    type  = "SQLServer"
    value = "@Microsoft.KeyVault(SecretUri=${azurerm_key_vault.portal.vault_uri}secrets/DefaultConnectionString)"
  }

  identity {
    type = "SystemAssigned"
  }
}

resource "azurerm_key_vault" "portal" {
  name                        = format("%vadvkeyvault", var.prefix)
  location                    = var.location
  resource_group_name         = var.resourceGroupName
  enabled_for_disk_encryption = true
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  soft_delete_retention_days  = 7
  purge_protection_enabled    = false

  sku_name = "standard"

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    key_permissions = [
      "Get",
    ]

    secret_permissions = [
      "Set",
      "Get",
      "Delete",
      "Purge",
      "Recover"
    ]

    storage_permissions = [
      "Get",
    ]
  }
}

resource "azurerm_key_vault_secret" "portal" {
  name         = "DefaultConnectionString"
  value        = "Server=tcp:${azurerm_mssql_server.mssql.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.advdb.name};Persist Security Info=False;User ID=${local.db_admin};Password=${local.db_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
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

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]
}

resource "azurerm_key_vault_access_policy" "github" {
  key_vault_id = azurerm_key_vault.portal.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = "89dd788c-3865-4bb1-931b-7f6204870d6b"

  key_permissions = [
    "Get",
  ]

  secret_permissions = [
    "Get",
  ]
}