# Azure Key Vault ASP.NET API in C#

A minimal ASP.NET API that returns a test secret held in Azure KeyVault

1. A Key Vault must be created in Azure.  
   In this example it was called: morsley-uk-key-vault  
   This value resides in the appsettings file under: KeyVault:Name  

2. A Secret must be added to the above key vault.  
   In this example it was called: test-secret  
   This value resides in the appsettings file under: test-secret  

3. A new App Registration is created which yeilds:  
   TenantId - This value resides in the appsettings file under: Azure:TenantId  
   ClientId - This value resides in the appsettings file under: Azure:ClientId  
   ClientSecret - This value resides in the User Secrets under: Azure:ClientSecret  
   
4. In Key Vault under Access Control for our key vault, our App Registration neeeds to be granted Key Vault Secrets User.  
