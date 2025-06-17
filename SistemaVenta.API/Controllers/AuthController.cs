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

        try
        {
            _logger.LogInformation($"Intento de login: {loginDto.NombreUsuario}");

            // 2. Cifra la contraseña aquí, antes de pasarla al servicio.
            var claveCifrada = Util.ConvertirASha256(loginDto.Clave);

            // 3. Usa la clave cifrada para el login.
            var usuarioValidado = await _usuarioService.Login(loginDto.NombreUsuario, claveCifrada);

            if (usuarioValidado == null || usuarioValidado.IdUsuario == 0)
            {
                return BadRequest("Credenciales incorrectas.");
            }
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
            return StatusCode(500, "Error interno");
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