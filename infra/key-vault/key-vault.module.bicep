@description('The location for the resource(s) to be deployed.')
param location string = resourceGroup().location

param KeyVaultName string

@secure()
param apikeysecret_value string

resource key_vault 'Microsoft.KeyVault/vaults@2024-11-01' existing = {
  name: KeyVaultName
}

resource secret_ApiKey 'Microsoft.KeyVault/vaults/secrets@2024-11-01' = {
  name: 'ApiKey'
  properties: {
    value: apikeysecret_value
  }
  parent: key_vault
}

output vaultUri string = key_vault.properties.vaultUri

output name string = KeyVaultName