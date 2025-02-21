@allowed([
  'canadaeast'
  'canadacentral'
])
param location string
param sqlServerName string
param sqlAdminLogin string

@minLength(10)
@maxLength(20)
@secure()
param sqlAdminPassword string
param databaseNames string

param startIpAddress string
param endIpAddress string

resource sqlServer 'Microsoft.Sql/servers@2024-05-01-preview' = {
  name: 'srv-${sqlServerName}'
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2024-05-01-preview' = {
  name: 'db-${databaseNames}'
  location: location
  parent: sqlServer
}

resource sqlFirewallRule 'Microsoft.Sql/servers/firewallRules@2024-05-01-preview' = {
  name: 'PlageIps-${sqlServerName}'
  parent: sqlServer
  properties: {
    startIpAddress: startIpAddress
    endIpAddress: endIpAddress
  }
}
