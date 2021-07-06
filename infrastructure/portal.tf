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
  name                         = format("adv-dbserver-%v", random_integer.random.result)
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
  name                = format("adv-appinsights-%v", random_integer.random.result)
  resource_group_name = var.resourceGroupName
  location            = var.location
  application_type    = "web"
}

resource "azurerm_app_service_plan" "portal" {
  name                = format("adv-appserviceplan-linux-%v", random_integer.random.result)
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
  name                = format("adv-appservice-%v", random_integer.random.result)
  resource_group_name = var.resourceGroupName
  location            = var.location
  app_service_plan_id = azurerm_app_service_plan.portal.id

  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.portal.instrumentation_key
  }

  connection_string {
    name  = "DefaultConnectionString"
    type  = "SQLServer"
    value = "Server=tcp:${azurerm_mssql_server.mssql.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.advdb.name};Persist Security Info=False;User ID=${local.db_admin};Password=${local.db_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}