resource "azurerm_resource_group" "res-0" {
  location = "polandcentral"
  name     = "rc2k"
}
resource "azurerm_container_app" "res-1" {
  container_app_environment_id = azurerm_container_app_environment.res-2.id
  max_inactive_revisions       = 100
  name                         = "rc2k-hub"
  resource_group_name          = azurerm_resource_group.res-0.name
  revision_mode                = "Single"
  workload_profile_name        = "Consumption"
  ingress {
    client_certificate_mode = "ignore"
    external_enabled        = true
    target_port             = 0
    traffic_weight {
      latest_revision = true
      percentage      = 100
    }
  }
  secret {
    name  = "client-id"
    value = var.keda_client_id
  }
  secret {
    name  = "client-password"
    value = var.keda_client_password
  }
  template {
    cooldown_period_in_seconds = 150
    min_replicas               = 0
    max_replicas               = 1
    container {
      cpu    = 0.25
      image  = "docker.io/theketrab/rc2k-hub-prod-image:latest"
      memory = "0.5Gi"
      name   = "rc2k-hub"
      env {
        name  = "ConnectionStrings__DefaultConnection"
        value = "Data Source=./RC2K.db"
      }
      env {
        name  = "Security__Salt"
        value = var.security_salt
      }
      env {
        name  = "Security__Iterations"
        value = "434482"
      }
      env {
        name  = "Cosmos__ApiKey"
        value = var.cosmos_api_key
      }
      env {
        name  = "ASPNETCORE_ENVIRONMENT"
        value = "Production"
      }
      env {
        name  = "ApplicationInsights__ConnectionString"
        value = var.application_insights_connection_string
      }
      env {
        name  = "Mailing__SftpAppPassword"
        value = var.mailing_sftp_app_password
      }
      env {
        name  = "Mailing__SenderEmail"
        value = "kettydun@gmail.com"
      }
      env {
        name  = "CAPTCHA__SecretKey"
        value = var.captcha_secret_key
      }
      env {
        name        = "MY_CLIENT_ID"
        secret_name = "client-id"
      }
      env {
        name        = "MY_CLIENT_PASSWORD"
        secret_name = "client-password"
      }
      liveness_probe {
        initial_delay = 0
        port          = 23040
        timeout       = 5
        transport     = "TCP"
      }
      readiness_probe {
        failure_count_threshold = 48
        interval_seconds        = 5
        port                    = 23040
        success_count_threshold = 1
        timeout                 = 5
        transport               = "TCP"
      }
      startup_probe {
        failure_count_threshold = 240
        initial_delay           = 1
        interval_seconds        = 1
        port                    = 23040
        timeout                 = 3
        transport               = "TCP"
      }
    }
    custom_scale_rule {
      custom_rule_type = "azure-app-insights"
      metadata = {
        activationTargetValue                = "0"
        activeDirectoryClientIdFromEnv       = "MY_CLIENT_ID"
        activeDirectoryClientPasswordFromEnv = "MY_CLIENT_PASSWORD"
        applicationInsightsId                = "12825ae2-7d67-440e-9f08-54b6d20cf868"
        ignoreNullValues                     = "True"
        metricAggregationTimespan            = "0:1"
        metricAggregationType                = "max"
        metricId                             = "customMetrics/ActiveCircuits"
        targetValue                          = "1"
        tenantId                             = "ecc03a36-fd60-44d9-a470-c14c4304bfcd"
      }
      name = "app-insights-scale-rule"
    }
  }
}
resource "azurerm_container_app_environment" "res-2" {
  location                   = "polandcentral"
  log_analytics_workspace_id = azurerm_log_analytics_workspace.res-31.id
  name                       = "managedEnvironment-rc2k-9123"
  resource_group_name        = azurerm_resource_group.res-0.name
  workload_profile {
    name                  = "Consumption"
    workload_profile_type = "Consumption"
  }
}
resource "azurerm_cosmosdb_account" "res-4" {
  automatic_failover_enabled = true
  free_tier_enabled          = true
  location                   = "polandcentral"
  name                       = "rc2k-db"
  offer_type                 = "Standard"
  resource_group_name        = azurerm_resource_group.res-0.name
  tags = {
    defaultExperience       = "Core (SQL)"
    hidden-cosmos-mmspecial = ""
    hidden-workload-type    = "Learning"
  }
  consistency_policy {
    consistency_level = "Session"
  }
  geo_location {
    failover_priority = 0
    location          = "polandcentral"
  }
}
resource "azurerm_cosmosdb_sql_database" "res-5" {
  account_name        = "rc2k-db"
  name                = "rc2k-db"
  resource_group_name = azurerm_resource_group.res-0.name
  autoscale_settings {
  }
  depends_on = [
    azurerm_cosmosdb_account.res-4,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-6" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "BonusPoints"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-7" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "BonusPoints-prod"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-8" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Drivers"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  unique_key {
    paths = ["/name"]
  }
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-9" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Drivers-prod"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  unique_key {
    paths = ["/name"]
  }
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-10" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Notifications"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-11" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Notifications-prod"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-12" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Statistics"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-13" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Statistics-prod"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-14" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "TimeEntries"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-15" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "TimeEntries-prod"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-16" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Users"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  unique_key {
    paths = ["/email"]
  }
  unique_key {
    paths = ["/name"]
  }
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-17" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "Users-prod"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  unique_key {
    paths = ["/email", "/name"]
  }
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-18" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "VerifyInfos"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_container" "res-19" {
  account_name          = "rc2k-db"
  database_name         = "rc2k-db"
  name                  = "VerifyInfos-prod"
  partition_key_paths   = ["/partitionKey"]
  partition_key_version = 2
  resource_group_name   = azurerm_resource_group.res-0.name
  depends_on = [
    azurerm_cosmosdb_sql_database.res-5,
  ]
}
resource "azurerm_cosmosdb_sql_role_definition" "res-27" {
  account_name        = "rc2k-db"
  assignable_scopes   = [azurerm_cosmosdb_account.res-4.id]
  name                = "Cosmos DB Built-in Data Reader"
  resource_group_name = azurerm_resource_group.res-0.name
  type                = "BuiltInRole"
  permissions {
    data_actions = ["Microsoft.DocumentDB/databaseAccounts/readMetadata", "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/executeQuery", "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/read", "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/readChangeFeed"]
  }
}
resource "azurerm_cosmosdb_sql_role_definition" "res-28" {
  account_name        = "rc2k-db"
  assignable_scopes   = [azurerm_cosmosdb_account.res-4.id]
  name                = "Cosmos DB Built-in Data Contributor"
  resource_group_name = azurerm_resource_group.res-0.name
  type                = "BuiltInRole"
  permissions {
    data_actions = ["Microsoft.DocumentDB/databaseAccounts/readMetadata", "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/*", "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*"]
  }
}
resource "azurerm_log_analytics_workspace" "res-31" {
  location            = "polandcentral"
  name                = "workspacerc2ka4b6"
  resource_group_name = azurerm_resource_group.res-0.name
}
resource "azurerm_static_web_app" "res-713" {
  location            = "westeurope"
  name                = "rc2k-hub-static"
  repository_branch   = "main"
  repository_url      = "https://github.com/TheKetrab/RC2K"
  resource_group_name = azurerm_resource_group.res-0.name
}
resource "azurerm_static_web_app_custom_domain" "res-715" {
  domain_name       = "rc2khub.com"
  static_web_app_id = azurerm_static_web_app.res-713.id
  validation_type   = ""
}
resource "azurerm_static_web_app_custom_domain" "res-716" {
  domain_name       = "www.rc2khub.com"
  static_web_app_id = azurerm_static_web_app.res-713.id
  validation_type   = ""
}
resource "azurerm_monitor_action_group" "res-717" {
  name                = "Application Insights Smart Detection"
  resource_group_name = azurerm_resource_group.res-0.name
  short_name          = "SmartDetect"
  arm_role_receiver {
    name                    = "Monitoring Contributor"
    role_id                 = "749f88d5-cbae-40b8-bcfc-e573ddc772fa"
    use_common_alert_schema = true
  }
  arm_role_receiver {
    name                    = "Monitoring Reader"
    role_id                 = "43d0d8ad-25c7-4714-9337-8ba259a9fe05"
    use_common_alert_schema = true
  }
}
resource "azurerm_application_insights" "res-718" {
  application_type    = "web"
  location            = "polandcentral"
  name                = "rc2k-hub-insights"
  resource_group_name = azurerm_resource_group.res-0.name
  sampling_percentage = 0
}
