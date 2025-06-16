// En: SistemaVenta.API/Controllers/UsuariosController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize] // El usuario debe estar logueado para acceder a cualquier método
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    // GET: api/usuarios?buscar=texto
    [HttpGet]
    [Authorize(Roles = "Administrador")] // Solo el admin puede ver la lista de usuarios
    public async Task<IActionResult> Lista(string buscar = "")
    {
        try
        {
            var listaEntidades = await _usuarioService.Lista(buscar);

            if (listaEntidades == null || !listaEntidades.Any())
                return Ok(new List<UsuarioDTO>());

            // Mapeo cuidadoso para NO exponer la contraseña
            var listaDto = listaEntidades.Select(u => new UsuarioDTO
            {
                IdUsuario = u.IdUsuario,
                NombreCompleto = u.NombreCompleto,
                Correo = u.Correo,
                NombreUsuario = u.NombreUsuario,
                IdRol = u.RefRol.IdRol,
                DescripcionRol = u.RefRol.Nombre,
                Activo = u.Activo == 1,
                Clave = "" // ¡IMPORTANTE! Nunca devolver la clave
            }).ToList();

            return Ok(listaDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // POST: api/usuarios
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] UsuarioDTO dto)
    {
        if (dto == null) return BadRequest("Datos de usuario inválidos.");

        try
        {
            // ALERTA DE SEGURIDAD: Tu código actual guarda la clave en texto plano.
            // El servicio debería hashear la clave antes de guardarla.
            // Ejemplo de cómo se haría en el servicio:
            // string claveHasheada = BCrypt.Net.BCrypt.HashPassword(dto.Clave);
            // entidad.Clave = claveHasheada;

            var entidad = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Correo = dto.Correo,
                NombreUsuario = dto.NombreUsuario,
                Clave = dto.Clave, // Temporalmente en texto plano, ¡corregir en el servicio!
                RefRol = new Rol { IdRol = dto.IdRol }
            };

            var resultadoSp = await _usuarioService.Crear(entidad);
            if (!string.IsNullOrEmpty(resultadoSp))
            {
                return BadRequest(resultadoSp);
            }

            return Ok("Usuario creado con éxito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // PUT: api/usuarios
    [HttpPut]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar([FromBody] UsuarioDTO dto)
    {
        if (dto == null || dto.IdUsuario == 0) return BadRequest("Datos de usuario inválidos.");

        try
        {
            // El sp_editarUsuario no modifica la clave, lo cual es correcto.
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
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // DELETE: api/usuarios/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var resultadoSp = await _usuarioService.Eliminar(id);
            if (!string.IsNullOrEmpty(resultadoSp))
            {
                return BadRequest(resultadoSp);
            }

            return Ok("Usuario eliminado con éxito.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}