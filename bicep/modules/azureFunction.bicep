param spName string
@allowed([
    'canadaeast'
    'canadacentral'
])
param location string
param azureFunctionName string
param spSku string

param storageName string
param storageSku string = 'Standard_ZRS'

//plan de service
resource servicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: 'sp-${spName}'
  location: location
  sku:{
    name: spSku
  }
  tags:{
    name: 'Application'
    value: spName
  }
}

//compte de stockage
resource storage 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: storageName
  kind: 'StorageV2'
  location: location
  sku: {
    name: storageSku
  }
  properties: {
    
  }
}

//application insights
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${azureFunctionName}-ai'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}

//application de function
resource azureFunction 'Microsoft.Web/sites@2024-04-01' = {
  name: azureFunctionName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: servicePlan.id
    siteConfig: {
      appSettings:[
        {
          name: 'AzureWebJobsStorage'
          value: storage.properties.primaryEndpoints.blob
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
      ]
    }
  }
}
