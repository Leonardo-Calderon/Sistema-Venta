// En: SistemaVenta.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using SVServices.Interfaces;
using Shared.DTOs;
using SVRepository.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IConfiguration _configuration;

    // 1. Inyectamos IUsuarioService y también IConfiguration para leer los settings del token
    public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
    {
        _usuarioService = usuarioService;
        _configuration = configuration;
    }

    // 2. Endpoint para iniciar sesión
    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        try
        {
            var usuarioValidado = await _usuarioService.Login(loginDto.NombreUsuario, loginDto.Clave);

            // 3. Si las credenciales no son válidas, el servicio devuelve null o un usuario sin Id
            if (usuarioValidado == null || usuarioValidado.IdUsuario == 0)
            {
                return BadRequest("Credenciales incorrectas.");
            }

            // 4. Si el usuario es válido, generamos el Token JWT
            var token = GenerateJwtToken(usuarioValidado);

            // 5. Creamos el DTO de Sesión para devolverlo al cliente
            var sessionDto = new SessionDTO
            {
                IdUsuario = usuarioValidado.IdUsuario,
                NombreCompleto = usuarioValidado.NombreCompleto,
                Correo = usuarioValidado.Correo,
                NombreUsuario = usuarioValidado.NombreUsuario,
                Rol = usuarioValidado.RefRol.Nombre, // Obtenemos el nombre del rol
                Token = token // Adjuntamos el token
            };

            return Ok(sessionDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
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