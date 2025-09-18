using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

var builder = WebApplication.CreateBuilder(args);

// Add User Secrets configuration first
builder.Configuration.AddUserSecrets<Program>();

var configuration = builder.Configuration;

var clientId = configuration["Azure:ClientId"];
var tenantId = configuration["Azure:TenantId"];
var clientSecret = configuration["Azure:ClientSecret"];

var keyVaultName = configuration["KeyVault:Name"];

// Validate configuration values
if (string.IsNullOrEmpty(clientId))
    throw new InvalidOperationException("Azure:ClientId is not configured");
if (string.IsNullOrEmpty(tenantId))
    throw new InvalidOperationException("Azure:TenantId is not configured");
if (string.IsNullOrEmpty(clientSecret))
    throw new InvalidOperationException("Azure:ClientSecret is not configured");
if (string.IsNullOrEmpty(keyVaultName))
    throw new InvalidOperationException("KeyVault:Name is not configured");

var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

var credential = new ClientSecretCredential(
    tenantId,
    clientId,
    clientSecret
);

// Add Azure Key Vault configuration before building the app
try
{
    builder.Configuration.AddAzureKeyVault(keyVaultUri, credential);
    Console.WriteLine($"Successfully configured Azure Key Vault: {keyVaultUri}");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to configure Azure Key Vault: {ex.Message}");
    throw;
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Azure KeyVault API v1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();