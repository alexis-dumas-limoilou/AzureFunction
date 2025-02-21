@allowed([
  'canadaeast'
  'canadacentral'
])
param location string

param sqlServerName string
param databaseName string
param sqlAdminLogin string
@minLength(10)
@maxLength(20)
@secure()
param sqlAdminPassword string

param azureFunctionName string

param containerGroupName string
param containerName string
param imageName string
param cpuCores int
param memoryGB int

param redisName string
param redisSkuCapacity int

//application de function
module azureFunction 'modules/azureFunction.bicep' = {
  name: 'azureFunction${azureFunctionName}'
  params: {
    azureFunctionName: azureFunctionName
    location: location
    spName: 'sp-${azureFunctionName}'
    spSku: 'Dynamic'
    storageName: 'st-${azureFunctionName}'
  }
}

//Azure SQL Database
module sqldatabase 'modules/sqldatabase.bicep' = {
  name: 'sqldatabase${sqlServerName}'
  params: {
      sqlServerName: sqlServerName
      databaseNames: databaseName
      location: location
      sqlAdminLogin: sqlAdminLogin
      sqlAdminPassword: sqlAdminPassword
      startIpAddress: '0.0.0.0'
      endIpAddress: '255.255.255.255'
  }
}

//Azure container Instance
resource containerInstance 'Microsoft.ContainerInstance/containerGroups@2024-11-01-preview' = {
  name: containerGroupName
  location: location
  properties: {
    containers: [
      {
        name: containerName
        properties: {
          image: imageName
          resources: {
            requests: {
              cpu: cpuCores
              memoryInGB: memoryGB
            }
          }
        }
      }
    ]
    osType: 'Linux'
  }
}

//Azure Redis Cache
resource redisCache 'Microsoft.Cache/redis@2024-11-01' = {
  name: redisName
  location: location
  properties: {
    sku: {
      capacity: redisSkuCapacity
      family: 'C'
      name: 'Basic'
    }
  }
}
