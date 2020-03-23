provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "darts_rg" {
  name     = "dm-darts"
  location = "eastus"
}

resource "azurerm_storage_account" "darts_sa" {
  name                     = "dmdartssa"
  resource_group_name      = azurerm_resource_group.darts_rg.name
  location                 = azurerm_resource_group.darts_rg.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "darts_asp" {
  name                = "dm-darts-asp"
  location            = azurerm_resource_group.darts_rg.location
  resource_group_name = azurerm_resource_group.darts_rg.name
  kind                = "FunctionApp"

  sku {
    tier = "Dynamic"
    size = "Y1"
  }
}

resource "azurerm_function_app" "darts_functions" {
  name                      = "dm-darts-app"
  location                  = azurerm_resource_group.darts_rg.location
  resource_group_name       = azurerm_resource_group.darts_rg.name
  app_service_plan_id       = azurerm_app_service_plan.darts_asp.id
  storage_connection_string = azurerm_storage_account.darts_sa.primary_connection_string
}

resource "azurerm_sql_server" "darts_sql_server" {
  name                         = "darts-db-server"
  location                     = azurerm_resource_group.darts_rg.location
  resource_group_name          = azurerm_resource_group.darts_rg.name
  administrator_login          = "dmusil"
  administrator_login_password = "TFtest187!"
  version                      = "12.0"
}

resource "azurerm_sql_database" "darts_db" {
  name                             = "darts-db"
  location                         = azurerm_resource_group.darts_rg.location
  server_name                      = azurerm_sql_server.darts_sql_server.name
  resource_group_name              = azurerm_resource_group.darts_rg.name
  edition                          = "GeneralPurpose"
  requested_service_objective_name = "GP_S_Gen5_1"
}

resource "azurerm_sql_firewall_rule" "allow_azure" {
  name                = "allow-azure-services"
  resource_group_name = azurerm_resource_group.darts_rg.name
  server_name         = azurerm_sql_server.darts_sql_server.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

