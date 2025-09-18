using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace Morsley.UK.AzureKeyVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeyVaultController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<KeyVaultController> _logger;

    public KeyVaultController(
        IConfiguration configuration,
        ILogger<KeyVaultController> logger)
    {
        _configuration = configuration;
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

        try
        {
            var secretValue = _configuration["test-secret"];
            
            if (string.IsNullOrEmpty(secretValue))
            {
                _logger.LogWarning("Secret 'test-secret' not found or is empty");
                return NotFound(new { message = "Secret 'test-secret' not found", error = "The secret may not exist in Key Vault or the application may not have permission to access it." });
            }

            _logger.LogInformation("Successfully retrieved secret value");
            return Ok(new { secretName = "test-secret", value = secretValue });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving secret from Key Vault");
            return StatusCode(500, new { message = "Error retrieving secret", error = ex.Message });
        }
    }
}
