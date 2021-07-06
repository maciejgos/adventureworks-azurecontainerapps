terraform {
  backend "azurerm" {
    resource_group_name  = "rg-shared-resources"
    storage_account_name = "advtfstatestorage001"
    container_name       = "tfstate"
    key                  = "dev-tfstate"
  }
}