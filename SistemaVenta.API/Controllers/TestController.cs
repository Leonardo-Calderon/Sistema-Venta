using Microsoft.AspNetCore.Mvc;

namespace SistemaVenta.API.Controllers
{
    /// <summary>
    /// Controlador temporal para probar la carga de secretos desde User Secrets
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Endpoint para verificar que los secretos se cargan correctamente desde User Secrets
        /// </summary>
        /// <returns>Estado de carga de cada secreto</returns>
        [HttpGet("secrets")]
        public IActionResult TestSecrets()
        {
            var result = new
            {
                ConnectionString = !string.IsNullOrEmpty(_configuration.GetConnectionString("cadenaSQL")),
                JwtKey = !string.IsNullOrEmpty(_configuration["Jwt:Key"]),
                JwtIssuer = !string.IsNullOrEmpty(_configuration["Jwt:Issuer"]),
                JwtAudience = !string.IsNullOrEmpty(_configuration["Jwt:Audience"]),
                CloudinaryCloudName = !string.IsNullOrEmpty(_configuration["Cloudinary:CloudName"]),
                CloudinaryApiKey = !string.IsNullOrEmpty(_configuration["Cloudinary:ApiKey"]),
                CloudinaryApiSecret = !string.IsNullOrEmpty(_configuration["Cloudinary:ApiSecret"]),
                SmtpHost = !string.IsNullOrEmpty(_configuration["Smtp:Host"]),
                SmtpPort = !string.IsNullOrEmpty(_configuration["Smtp:Port"]),
                SmtpUser = !string.IsNullOrEmpty(_configuration["Smtp:User"]),
                SmtpPass = !string.IsNullOrEmpty(_configuration["Smtp:Pass"]),
                Message = "Prueba de carga de secretos desde User Secrets",
                Timestamp = DateTime.UtcNow,
                Environment = _configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development"
            };

            return Ok(result);
        }

        /// <summary>
        /// Endpoint para verificar la configuración general de la aplicación
        /// </summary>
        /// <returns>Información de configuración</returns>
        [HttpGet("config")]
        public IActionResult TestConfig()
        {
            var result = new
            {
                LoggingLevel = _configuration["Logging:LogLevel:Default"],
                AllowedHosts = _configuration["AllowedHosts"],
                UserSecretsId = "fb096d92-1e7e-4b58-9ff8-73cbac9f561a",
                Message = "Configuración general de la aplicación",
                Timestamp = DateTime.UtcNow
            };

            return Ok(result);
        }

        /// <summary>
        /// Endpoint para verificar que la aplicación está funcionando
        /// </summary>
        /// <returns>Estado de la aplicación</returns>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            var result = new
            {
                Status = "Healthy",
                Message = "API funcionando correctamente",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0"
            };

            return Ok(result);
        }
    }
}
