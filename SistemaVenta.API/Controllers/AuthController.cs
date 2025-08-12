// En: SistemaVenta.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using SVServices.Interfaces;
using Shared.DTOs;
using SVRepository.Entities;
using SistemaVenta.API.Utilidades;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

/// <summary>
/// Controlador para manejar la autenticación y autorización de usuarios en la API.
/// </summary>
/// <remarks>
/// Este controlador proporciona endpoints para:
/// - Autenticación de usuarios mediante login
/// - Generación de tokens JWT
/// - Validación de credenciales
/// - Registro de auditoría de intentos de autenticación
/// 
/// Implementa medidas de seguridad como:
/// - Encriptación de contraseñas con SHA256
/// - Logging detallado de intentos de autenticación
/// - Auditoría de actividades de seguridad
/// - Validación de entrada de datos
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuditoriaService _auditoriaService;

    /// <summary>
    /// Inicializa una nueva instancia del controlador de autenticación.
    /// </summary>
    /// <param name="usuarioService">Servicio para operaciones de usuario.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <param name="logger">Logger para registrar información de autenticación.</param>
    /// <param name="auditoriaService">Servicio para registrar auditoría de autenticación.</param>
    /// <remarks>
    /// El constructor recibe las dependencias necesarias para el funcionamiento
    /// del controlador, incluyendo servicios de usuario, configuración JWT,
    /// logging y auditoría.
    /// </remarks>
    public AuthController(
        IUsuarioService usuarioService,
        IConfiguration configuration,
        ILogger<AuthController> logger,
        IAuditoriaService auditoriaService)
    {
        _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _auditoriaService = auditoriaService ?? throw new ArgumentNullException(nameof(auditoriaService));
    }

    /// <summary>
    /// Autentica un usuario y genera un token JWT si las credenciales son válidas.
    /// </summary>
    /// <param name="loginDto">Datos de login del usuario (nombre de usuario y contraseña).</param>
    /// <returns>
    /// - 200 OK con datos de sesión si la autenticación es exitosa
    /// - 400 Bad Request si las credenciales son incorrectas o faltan datos
    /// - 500 Internal Server Error si ocurre un error interno
    /// </returns>
    /// <remarks>
    /// Este endpoint:
    /// 1. Valida los datos de entrada
    /// 2. Encripta la contraseña con SHA256
    /// 3. Valida las credenciales contra la base de datos
    /// 4. Genera un token JWT si la autenticación es exitosa
    /// 5. Registra la actividad en el sistema de auditoría
    /// 6. Retorna los datos de sesión incluyendo el token JWT
    /// 
    /// Tanto los intentos exitosos como fallidos son registrados para
    /// fines de seguridad y auditoría.
    /// </remarks>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        // Validación inicial
        _logger.LogInformation($"Login attempt: {JsonSerializer.Serialize(loginDto)}");

        if (loginDto == null || string.IsNullOrEmpty(loginDto.NombreUsuario))
        {
            _logger.LogWarning("Username missing");
            return BadRequest("Usuario requerido");
        }

        try
        {
            _logger.LogInformation($"Intento de login: {loginDto.NombreUsuario}");

            // Obtener IP del cliente
            var ipAddress = GetClientIpAddress();

            // Encriptar la contraseña antes de pasarla al servicio
            var claveCifrada = Util.ConvertirASha256(loginDto.Clave);

            // Validar credenciales con la contraseña encriptada
            var usuarioValidado = await _usuarioService.Login(loginDto.NombreUsuario, claveCifrada);

            if (usuarioValidado == null || usuarioValidado.IdUsuario == 0)
            {
                // Registrar intento fallido
                await _auditoriaService.RegistrarAutenticacion(loginDto.NombreUsuario, "Fallido", ipAddress, "Credenciales incorrectas");
                return BadRequest("Credenciales incorrectas.");
            }

            // Registrar intento exitoso
            await _auditoriaService.RegistrarAutenticacion(loginDto.NombreUsuario, "Exitoso", ipAddress, $"Login exitoso - Rol: {usuarioValidado.RefRol.Nombre}");

            var token = GenerateJwtToken(usuarioValidado);

            var sessionDto = new SessionDTO
            {
                IdUsuario = usuarioValidado.IdUsuario,
                NombreCompleto = usuarioValidado.NombreCompleto,
                Correo = usuarioValidado.Correo,
                NombreUsuario = usuarioValidado.NombreUsuario,
                Rol = usuarioValidado.RefRol.Nombre,
                Token = token
            };

            return Ok(sessionDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login");
            
            // Registrar error en autenticación
            var ipAddress = GetClientIpAddress();
            await _auditoriaService.RegistrarAutenticacion(loginDto?.NombreUsuario ?? "Desconocido", "Error", ipAddress, $"Error interno: {ex.Message}");
            
            return StatusCode(500, "Error interno");
        }
    }

    /// <summary>
    /// Genera un token JWT para un usuario autenticado.
    /// </summary>
    /// <param name="usuario">El usuario para el cual se generará el token.</param>
    /// <returns>Un token JWT válido que contiene los claims del usuario.</returns>
    /// <remarks>
    /// Este método genera un token JWT que incluye:
    /// - ID del usuario
    /// - Nombre de usuario
    /// - Nombre completo
    /// - Correo electrónico
    /// - Rol del usuario
    /// - Fecha de expiración (configurada en appsettings.json)
    /// 
    /// El token se firma usando la clave secreta configurada en la aplicación
    /// y utiliza el algoritmo HMAC-SHA256 para la firma.
    /// </remarks>
    private string GenerateJwtToken(Usuario usuario)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Los "Claims" son declaraciones sobre el usuario que se guardan dentro del token.
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.RefRol.Nombre),
            new Claim("IdRol", usuario.RefRol.IdRol.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8), // El token expirará en 8 horas
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GetClientIpAddress()
    {
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
        
        // Verificar headers de proxy
        if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        }
        else if (HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
        {
            ip = HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
        }

        return ip ?? "Desconocida";
    }
}