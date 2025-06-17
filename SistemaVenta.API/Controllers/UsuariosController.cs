using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;
using SistemaVenta.API.Utilidades; // Importante para usar Util.cs

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ICorreoService _correoService; // Inyecta el servicio de correo
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(
        IUsuarioService usuarioService,
        ICorreoService correoService, // Añádelo al constructor
        ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _correoService = correoService; // Asígnalo
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Lista([FromQuery] string buscar = "")
    {
        var listaEntidades = await _usuarioService.Lista(buscar);
        var listaDto = listaEntidades.Select(u => new UsuarioDTO
        {
            IdUsuario = u.IdUsuario,
            NombreCompleto = u.NombreCompleto,
            Correo = u.Correo,
            NombreUsuario = u.NombreUsuario,
            IdRol = u.RefRol.IdRol,
            DescripcionRol = u.RefRol.Nombre,
            Activo = u.Activo == 1,
        }).ToList();
        return Ok(listaDto);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] UsuarioCrearDTO dto)
    {
        try
        {
            // 1. Generar y encriptar la clave (lógica de FrmUsuario)
            var claveGenerada = Util.GenerarCode();
            var claveEncriptada = Util.ConvertirASha256(claveGenerada);

            var entidad = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Correo = dto.Correo,
                NombreUsuario = dto.NombreUsuario,
                Clave = claveEncriptada, // Guardamos la clave encriptada
                RefRol = new Rol { IdRol = dto.IdRol },
                ResetearClave = 1, // Forzar cambio de clave en el primer login
                Activo = 1
            };

            var resultadoSp = await _usuarioService.Crear(entidad);
            if (!string.IsNullOrEmpty(resultadoSp))
            {
                return BadRequest(resultadoSp);
            }

            // 2. Enviar correo con la clave generada (lógica de FrmUsuario)
            var mensaje = $"<h3>Usuario creado correctamente.</h3>" +
                          $"<p>Sus credenciales de acceso son:</p>" +
                          $"<p><b>Nombre de usuario:</b> {dto.NombreUsuario}</p>" +
                          $"<p><b>Clave temporal:</b> {claveGenerada}</p>" +
                          $"<p>Por su seguridad, se le pedirá que cambie la clave la primera vez que inicie sesión.</p>";

            await _correoService.Enviar(dto.Correo, "¡Bienvenido a SistemaVenta!", mensaje);

            return Ok("Usuario creado con éxito.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando usuario");
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id, [FromBody] UsuarioDTO dto)
    {
        if (id != dto.IdUsuario) return BadRequest("El ID del usuario no coincide.");

        var entidad = new Usuario
        {
            IdUsuario = dto.IdUsuario,
            NombreCompleto = dto.NombreCompleto,
            Correo = dto.Correo,
            NombreUsuario = dto.NombreUsuario,
            Activo = dto.Activo ? 1 : 0,
            RefRol = new Rol { IdRol = dto.IdRol }
        };

        var resultadoSp = await _usuarioService.Editar(entidad);
        if (!string.IsNullOrEmpty(resultadoSp))
        {
            return BadRequest(resultadoSp);
        }
        return Ok("Usuario actualizado con éxito.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultadoSp = await _usuarioService.Eliminar(id);
        if (!string.IsNullOrEmpty(resultadoSp))
        {
            return BadRequest(resultadoSp);
        }
        return Ok("Usuario eliminado con éxito.");
    }
}