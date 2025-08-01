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
using System.Data;
using Microsoft.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUsuarioService usuarioService,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _usuarioService = usuarioService;
        _configuration = configuration;
        _logger = logger;
    }

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

        if (string.IsNullOrEmpty(loginDto.Clave))
        {
            _logger.LogWarning("Password missing");
            return BadRequest("Contraseña requerida");
        }

        try
        {
            _logger.LogInformation($"Intento de login: {loginDto.NombreUsuario}");

            // Verificar configuración JWT
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
            {
                _logger.LogError("Configuración JWT incompleta");
                return StatusCode(500, "Error de configuración del servidor");
            }

            // Cifra la contraseña aquí, antes de pasarla al servicio.
            var claveCifrada = Util.ConvertirASha256(loginDto.Clave);
            _logger.LogInformation($"Contraseña cifrada generada para usuario: {loginDto.NombreUsuario}");

            // Usa la clave cifrada para el login.
            var usuarioValidado = await _usuarioService.Login(loginDto.NombreUsuario, claveCifrada);

            if (usuarioValidado == null || usuarioValidado.IdUsuario == 0)
            {
                _logger.LogWarning($"Credenciales incorrectas para usuario: {loginDto.NombreUsuario}");
                return BadRequest("Credenciales incorrectas.");
            }

            // Verificar que el usuario esté activo
            if (usuarioValidado.Activo == 0)
            {
                _logger.LogWarning($"Usuario inactivo intentando login: {loginDto.NombreUsuario}");
                return BadRequest("Usuario inactivo. Contacte al administrador.");
            }

            // Verificar que el rol esté presente
            if (usuarioValidado.RefRol == null)
            {
                _logger.LogError($"Usuario sin rol asignado: {loginDto.NombreUsuario}");
                return StatusCode(500, "Error: Usuario sin rol asignado");
            }

            _logger.LogInformation($"Usuario autenticado exitosamente: {loginDto.NombreUsuario}, Rol: {usuarioValidado.RefRol.Nombre}");

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

            _logger.LogInformation($"Sesión creada exitosamente para usuario: {loginDto.NombreUsuario}");
            return Ok(sessionDto);
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, $"Error de base de datos en login para usuario {loginDto.NombreUsuario}: {sqlEx.Message}");
            return StatusCode(500, "Error de conexión a la base de datos. Contacte al administrador.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error inesperado en login para usuario {loginDto.NombreUsuario}: {ex.Message}");
            return StatusCode(500, "Error interno del servidor. Intente nuevamente.");
        }
    }

// 6. Método privado para generar el Token
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
}