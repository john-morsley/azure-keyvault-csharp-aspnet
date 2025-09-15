using Microsoft.AspNetCore.Mvc;

namespace Morsley.UK.AzureKeyVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeyVaultController : ControllerBase
{
    private readonly ILogger<KeyVaultController> _logger;

    public KeyVaultController(ILogger<KeyVaultController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        _logger.LogInformation("GET request received for KeyVault endpoint");
        return Ok("Hello from Morsley.UK.AzureKeyVault.API - KeyVault endpoint is working!");
    }

    [HttpGet("secret")]
    public ActionResult<object> GetSecret()
    {
        _logger.LogInformation("GET request received for secret endpoint");
        
        // This is a placeholder - in a real implementation, you would retrieve from Azure Key Vault
        var secretResponse = new
        {
            SecretName = "test-secret",
            Value = "This would be retrieved from Azure Key Vault",
            Timestamp = DateTime.UtcNow,
            Status = "Success"
        };

        return Ok(secretResponse);
    }
}
