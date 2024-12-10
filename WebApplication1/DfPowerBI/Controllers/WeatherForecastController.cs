using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DfPowerBI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        private static readonly string TenantId = "d022fc40-e3d0-4b60-af21-740a92219103";
        private static readonly string Audience = "bd8ff232-34b4-4999-8796-4700e5b30a88";
        private static readonly string Issuer = $"https://login.microsoftonline.com/{TenantId}/v2.0";


        [HttpGet("token")]
        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateAudience = true,
                ValidAudience = Audience,
                ValidateLifetime = false,
                IssuerSigningKeys = GetSigningKeys(),
                ValidateIssuerSigningKey = true,
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }

        private static IEnumerable<SecurityKey> GetSigningKeys()
        {
            // Retrieve the signing keys from the Azure AD metadata endpoint
            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                $"{Issuer}/.well-known/openid-configuration",
                new OpenIdConnectConfigurationRetriever());

            var openIdConfig = configurationManager.GetConfigurationAsync().Result;
            return openIdConfig.SigningKeys;
        }
    }
}
