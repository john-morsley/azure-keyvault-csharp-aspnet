# Azure Key Vault RBAC Setup Guide

## Problem
Your application is receiving a 403 Forbidden error when trying to access Azure Key Vault:
```
Caller is not authorized to perform action on resource.
Action: 'Microsoft.KeyVault/vaults/secrets/readMetadata/action'
```

This means your Azure AD application doesn't have the necessary permissions to access the Key Vault.

## Solution: Configure Azure RBAC Permissions

### Method 1: Using Azure Portal

1. **Navigate to your Key Vault**
   - Go to [Azure Portal](https://portal.azure.com)
   - Navigate to your Key Vault: `morsley-uk-key-vault`

2. **Configure Access Control (IAM)**
   - In the Key Vault, go to **Access control (IAM)** in the left menu
   - Click **+ Add** → **Add role assignment**

3. **Assign the Role**
   - **Role**: Select **Key Vault Secrets User** (recommended) or **Key Vault Secrets Officer** (if you need write access)
   - **Assign access to**: User, group, or service principal
   - **Select**: Search for your application by Client ID: `cfd17100-c23b-4ecb-8ee1-c4bd5c54e7ab`
   - Click **Save**

### Method 2: Using Azure CLI

```bash
# Set variables
KEYVAULT_NAME="morsley-uk-key-vault"
CLIENT_ID="cfd17100-c23b-4ecb-8ee1-c4bd5c54e7ab"
SUBSCRIPTION_ID="adfd0655-bb93-4de0-90eb-6d72c8dc4a3e"
RESOURCE_GROUP="morsley-uk-rg"

# Get the Key Vault resource ID
KEYVAULT_ID="/subscriptions/$SUBSCRIPTION_ID/resourceGroups/$RESOURCE_GROUP/providers/Microsoft.KeyVault/vaults/$KEYVAULT_NAME"

# Assign Key Vault Secrets User role
az role assignment create \
  --role "Key Vault Secrets User" \
  --assignee $CLIENT_ID \
  --scope $KEYVAULT_ID
```

### Method 3: Using Azure PowerShell

```powershell
# Set variables
$KeyVaultName = "morsley-uk-key-vault"
$ClientId = "cfd17100-c23b-4ecb-8ee1-c4bd5c54e7ab"
$SubscriptionId = "adfd0655-bb93-4de0-90eb-6d72c8dc4a3e"
$ResourceGroupName = "morsley-uk-rg"

# Get the Key Vault resource
$KeyVault = Get-AzKeyVault -VaultName $KeyVaultName -ResourceGroupName $ResourceGroupName

# Assign Key Vault Secrets User role
New-AzRoleAssignment -ApplicationId $ClientId -RoleDefinitionName "Key Vault Secrets User" -Scope $KeyVault.ResourceId
```

## Required Azure RBAC Roles

Choose the appropriate role based on your needs:

| Role | Permissions | Use Case |
|------|-------------|----------|
| **Key Vault Secrets User** | Read secrets | Production applications (recommended) |
| **Key Vault Secrets Officer** | Read, write, delete secrets | Development/admin applications |
| **Key Vault Administrator** | Full access to Key Vault | Key Vault management |

## Verify the Setup

1. **Wait for propagation** (can take up to 10 minutes)
2. **Test your application** by running it and accessing the `/api/keyvault/secret` endpoint
3. **Check the logs** for any remaining errors

## Create the Test Secret

If the secret doesn't exist in your Key Vault, create it:

### Using Azure Portal:
1. Go to your Key Vault → **Secrets**
2. Click **+ Generate/Import**
3. Name: `test-secret`
4. Value: `Hello from Azure Key Vault!`
5. Click **Create**

### Using Azure CLI:
```bash
az keyvault secret set --vault-name "morsley-uk-key-vault" --name "test-secret" --value "Hello from Azure Key Vault!"
```

## Troubleshooting

### Common Issues:

1. **Role assignment not propagated**: Wait up to 10 minutes after creating the role assignment
2. **Wrong Client ID**: Ensure you're using the correct Application (client) ID from your Azure AD app registration
3. **Key Vault not found**: Verify the Key Vault name and that it exists in the specified resource group
4. **Secret doesn't exist**: Create the `test-secret` in your Key Vault

### Verify Role Assignment:
```bash
# Check role assignments for your Key Vault
az role assignment list --scope "/subscriptions/adfd0655-bb93-4de0-90eb-6d72c8dc4a3e/resourceGroups/morsley-uk-rg/providers/Microsoft.KeyVault/vaults/morsley-uk-key-vault" --assignee "cfd17100-c23b-4ecb-8ee1-c4bd5c54e7ab"
```

## Next Steps

After configuring the permissions:
1. Restart your application
2. Test the `/api/keyvault/secret` endpoint
3. Check the application logs for any remaining issues
4. If successful, you should see the secret value returned from Azure Key Vault
