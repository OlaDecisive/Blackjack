@description('The location into which your Azure resources should be deployed.')
param location string = resourceGroup().location

@description('Select the type of environment you want to provision. Allowed values are Production and Test.')
@allowed([
  'Production'
  'Test'
])
param environmentType string

@description('A unique suffix to add to resource names that need to be globally unique.')
@maxLength(10)
//param resourceNameSuffix string = uniqueString(resourceGroup().id)
param resourceNameSuffix string = toLower(environmentType)

param databaseAdministratorLogin string
@secure()
param databaseAdministratorPassword string

// Define the names for resources.
var databaseName = 'psql-ola-blackjack-${resourceNameSuffix}'
var appServiceAppName = 'app-ola-blackjack-${resourceNameSuffix}'
var appServicePlanName = 'asp-ola-blackjack'
var logAnalyticsWorkspaceName = 'log-ola-blackjack-${resourceNameSuffix}'
var applicationInsightsName = 'appi-ola-blackjack-${resourceNameSuffix}'
var storageAccountName = 'stolablackjack${resourceNameSuffix}'

// Define the SKUs for each component based on the environment type.
var environmentConfigurationMap = {
  Production: {
    appServicePlan: {
      sku: {
        name: 'B1'
        capacity: 1
      }
    }
    storageAccount: {
      sku: {
        name: 'Standard_LRS'
      }
    }
  }
  Test: {
    appServicePlan: {
      sku: {
        name: 'B1'
        capacity: 1
      }
    }
    storageAccount: {
      sku: {
        name: 'Standard_LRS'
      }
    }
  }
}

resource database 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' = {
  name: databaseName
  location: location
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    createMode: 'Create'
    version: '16'
    administratorLogin: databaseAdministratorLogin
    administratorLoginPassword: databaseAdministratorPassword
    storage: {
      storageSizeGB: 32
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
  }
}

resource firewallRule_AzureIps 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2022-12-01' = {
  name: 'AllowAzureIps'
  parent: database
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: environmentConfigurationMap[environmentType].appServicePlan.sku
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource appServiceApp 'Microsoft.Web/sites@2022-03-01' = {
  name: appServiceAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: applicationInsights.properties.ConnectionString
        }
        {
          name: 'SCM_DO_BUILD_DURING_DEPLOYMENT'
          value: 'true'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
      ]
      connectionStrings: [
        {
          name: 'PSQL_CONNECTIONSTRING'
          connectionString: 'Server=${databaseName}.postgres.database.azure.com;Database=blackjack;Port=5432;User Id=${databaseAdministratorLogin};Password=${databaseAdministratorPassword};Ssl Mode=Require;'
          type: 'PostgreSQL'
        }
      ]
      linuxFxVersion: 'DOTNETCORE|8.0'
      httpLoggingEnabled: true
      detailedErrorLoggingEnabled: true
      logsDirectorySizeLimit: 35
    }
  }
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: logAnalyticsWorkspaceName
  location: location
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    Request_Source: 'rest'
    Flow_Type: 'Bluefield'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: environmentConfigurationMap[environmentType].storageAccount.sku
}

output appServiceAppHostName string = appServiceApp.properties.defaultHostName
