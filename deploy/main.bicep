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
param resourceNameSuffix string = environmentType

// Define the names for resources.
var appServiceAppName = 'app-ola-blackjack-${resourceNameSuffix}'
var appServicePlanName = 'asp-ola-blackjack-${resourceNameSuffix}'
var logAnalyticsWorkspaceName = 'log-ola-blackjack-${resourceNameSuffix}'
var applicationInsightsName = 'appi-ola-blackjack-${resourceNameSuffix}'
var storageAccountName = 'stolablackjack${toLower(resourceNameSuffix)}'

// Define the SKUs for each component based on the environment type.
var environmentConfigurationMap = {
  Production: {
    appServicePlan: {
      sku: {
        name: 'F1'
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
        name: 'F1'
      }
    }
    storageAccount: {
      sku: {
        name: 'Standard_LRS'
      }
    }
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
      ]
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
