using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using SVRepository.Entities;
using SVServices.Interfaces;
using SistemaVenta.API.Utilidades;
using System.Security.Claims;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
        private readonly IUsuarioService _usuarioService;
    private readonly ICorreoService _correoService;
    private readonly ILogger<UsuariosController> _logger;
    private readonly IValidacionService _validacionService;

    public UsuariosController(
        IUsuarioService usuarioService,
        ICorreoService correoService, 
        ILogger<UsuariosController> logger,
        IValidacionService validacionService)
    {
        _usuarioService = usuarioService;
        _correoService = correoService;
        _logger = logger;
        _validacionService = validacionService;
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

    /// <summary>
    /// PASO 4: Endpoint de búsqueda segura con validación y sanitización
    /// </summary>
    [HttpGet("search")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> BusquedaSegura([FromQuery] string searchTerm = "")
    {
        try
        {
            // PASO 4: Validación y sanitización del término de búsqueda
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                _logger.LogInformation("Búsqueda de usuarios sin término de búsqueda");
                return await Lista(""); // Retornar lista completa
            }

            // Sanitizar el término de búsqueda
            var searchTermSanitizado = _validacionService.SanitizarString(searchTerm, 50, true);
            
            // Verificar si el término de búsqueda fue rechazado por la sanitización
            if (searchTermSanitizado == null)
            {
                _logger.LogWarning("Término de búsqueda rechazado por sanitización: {SearchTerm}", searchTerm);
                return BadRequest("El término de búsqueda contiene caracteres no permitidos.");
            }

            // Verificar si contiene caracteres peligrosos
            if (_validacionService.ContieneCaracteresPeligrosos(searchTerm))
            {
                _logger.LogWarning("Se detectaron caracteres peligrosos en la búsqueda: {SearchTerm}", searchTerm);
                return BadRequest("El término de búsqueda contiene caracteres no permitidos.");
            }

            // Validar longitud mínima y máxima
            if (searchTermSanitizado.Length < 2)
            {
                _logger.LogWarning("Término de búsqueda demasiado corto: {SearchTerm}", searchTermSanitizado);
                return BadRequest("El término de búsqueda debe tener al menos 2 caracteres.");
            }

            if (searchTermSanitizado.Length > 50)
            {
                _logger.LogWarning("Término de búsqueda demasiado largo: {SearchTerm}", searchTermSanitizado);
                return BadRequest("El término de búsqueda no puede exceder 50 caracteres.");
            }

            // PASO 4: Realizar búsqueda segura usando el servicio
            _logger.LogInformation("Iniciando búsqueda segura de usuarios con término: {SearchTerm}", searchTermSanitizado);
            
            var listaEntidades = await _usuarioService.Lista(searchTermSanitizado);
            
            // Mapear a DTOs
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

            _logger.LogInformation("Búsqueda segura completada. Resultados encontrados: {Count}", listaDto.Count);
            
            return Ok(new
            {
                TerminoBusqueda = searchTermSanitizado,
                TotalResultados = listaDto.Count,
                Resultados = listaDto,
                FechaBusqueda = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en búsqueda segura de usuarios con término: {SearchTerm}", searchTerm);
            return StatusCode(500, "Error interno del servidor durante la búsqueda.");
        }
    }

    [HttpGet("Perfil")]
    [Authorize(Roles = "Administrador,Ventas")] // Cualquier usuario autenticado puede obtener su perfil
    public async Task<IActionResult> ObtenerPerfil()
    {
        try
        {
            // Extraer el ID del usuario del token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Token inválido o no contiene el ID del usuario.");
            }

            // Obtener el usuario por ID
            var usuario = await _usuarioService.ObtenerPorId(userId);
            if (usuario == null || usuario.IdUsuario == 0)
            {
                return NotFound("Usuario no encontrado.");
            }

            var dto = new UsuarioDTO
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = usuario.NombreCompleto,
                Correo = usuario.Correo,
                NombreUsuario = usuario.NombreUsuario,
                IdRol = usuario.RefRol.IdRol,
                DescripcionRol = usuario.RefRol.Nombre,
                Activo = usuario.Activo == 1,
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo perfil de usuario");
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Crear([FromBody] UsuarioCrearDTO dto)
    {
        try
        {
            // PASO 2: Validación y sanitización de datos de entrada
            var erroresValidacion = _validacionService.ValidarObjeto(dto);
            if (erroresValidacion.Any())
            {
                var mensajesError = erroresValidacion.Select(e => e.ErrorMessage).ToList();
                _logger.LogWarning("Errores de validación en creación de usuario: {Errores}", string.Join(", ", mensajesError));
                return BadRequest(new { Errores = mensajesError });
            }

            // Sanitizar datos de entrada
            var nombreCompletoSanitizado = _validacionService.SanitizarString(dto.NombreCompleto, 100, false);
            var correoSanitizado = _validacionService.SanitizarEmail(dto.Correo);
            var nombreUsuarioSanitizado = _validacionService.SanitizarString(dto.NombreUsuario, 50, false);
            var idRolSanitizado = _validacionService.SanitizarInt(dto.IdRol, 1, 10); // Asumiendo máximo 10 roles

            // Verificar si algún dato fue rechazado por la sanitización
            if (nombreCompletoSanitizado == null || correoSanitizado == null || 
                nombreUsuarioSanitizado == null || idRolSanitizado == null)
            {
                _logger.LogWarning("Datos de usuario rechazados por sanitización: NombreCompleto={NombreCompleto}, Correo={Correo}, NombreUsuario={NombreUsuario}, IdRol={IdRol}", 
                    dto.NombreCompleto, dto.Correo, dto.NombreUsuario, dto.IdRol);
                return BadRequest("Los datos proporcionados contienen información no válida o peligrosa.");
            }

            // Verificar si contiene caracteres peligrosos
            if (_validacionService.ContieneCaracteresPeligrosos(dto.NombreCompleto) ||
                _validacionService.ContieneCaracteresPeligrosos(dto.NombreUsuario))
            {
                _logger.LogWarning("Se detectaron caracteres peligrosos en la creación de usuario");
                return BadRequest("Los datos proporcionados contienen caracteres no permitidos.");
            }

            // Determinar la contraseña a usar
            string contrasenaFinal;
            bool esContrasenaTemporal;
            string contrasenaTemporal = "";

            if (!string.IsNullOrWhiteSpace(dto.Contrasena))
            {
                // Usar la contraseña proporcionada por el usuario
                contrasenaFinal = dto.Contrasena;
                esContrasenaTemporal = false;
                _logger.LogInformation("Usuario creado con contraseña personalizada");
            }
            else
            {
                // Generar contraseña temporal automáticamente
                contrasenaTemporal = Util.GenerarCode();
                contrasenaFinal = contrasenaTemporal;
                esContrasenaTemporal = true;
                _logger.LogInformation("Usuario creado con contraseña temporal generada automáticamente");
            }

            // Encriptar la contraseña
            var claveEncriptada = Util.ConvertirASha256(contrasenaFinal);

            // PASO 2: Creación segura de la entidad con datos sanitizados
            var entidad = new Usuario
            {
                NombreCompleto = nombreCompletoSanitizado,
                Correo = correoSanitizado,
                NombreUsuario = nombreUsuarioSanitizado,
                Clave = claveEncriptada,
                RefRol = new Rol { IdRol = idRolSanitizado.Value },
                ResetearClave = esContrasenaTemporal ? 1 : 0, // Solo resetear si es contraseña temporal
                Activo = 1
            };

            // Guardar en la base de datos usando el servicio
            var resultadoSp = await _usuarioService.Crear(entidad);
            if (!string.IsNullOrEmpty(resultadoSp))
            {
                _logger.LogError("Error al crear usuario en base de datos: {Error}", resultadoSp);
                return BadRequest(resultadoSp);
            }

            // Preparar mensaje de email según el tipo de contraseña
            string mensajeEmail;
            if (esContrasenaTemporal)
            {
                mensajeEmail = $"<h3>Usuario creado correctamente.</h3>" +
                              $"<p>Sus credenciales de acceso son:</p>" +
                              $"<p><b>Nombre de usuario:</b> {nombreUsuarioSanitizado}</p>" +
                              $"<p><b>Clave temporal:</b> {contrasenaTemporal}</p>" +
                              $"<p>Por su seguridad, se le pedirá que cambie la clave la primera vez que inicie sesión.</p>";
            }
            else
            {
                mensajeEmail = $"<h3>Usuario creado correctamente.</h3>" +
                              $"<p>Sus credenciales de acceso son:</p>" +
                              $"<p><b>Nombre de usuario:</b> {nombreUsuarioSanitizado}</p>" +
                              $"<p><b>Clave:</b> {contrasenaFinal}</p>" +
                              $"<p>Su cuenta está lista para usar.</p>";
            }

            // Enviar email con datos sanitizados (opcional, no debe fallar la creación)
            try
            {
                await _correoService.Enviar(correoSanitizado, "¡Bienvenido a SistemaVenta!", mensajeEmail);
                _logger.LogInformation("Email enviado exitosamente a: {Correo}", correoSanitizado);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo enviar el email a: {Correo}. El usuario fue creado exitosamente.", correoSanitizado);
            }

            // Preparar respuesta con información del usuario creado
            var respuesta = new UsuarioCreadoResponseDTO
            {
                IdUsuario = 0, // No tenemos el ID porque no modificamos el SP
                NombreCompleto = nombreCompletoSanitizado,
                NombreUsuario = nombreUsuarioSanitizado,
                Correo = correoSanitizado,
                ContrasenaTemporal = esContrasenaTemporal ? contrasenaTemporal : contrasenaFinal,
                EsContrasenaTemporal = esContrasenaTemporal,
                Mensaje = esContrasenaTemporal 
                    ? "Usuario creado con éxito. Contraseña temporal: " + contrasenaTemporal
                    : "Usuario creado con éxito. Contraseña: " + contrasenaFinal
            };

            _logger.LogInformation("Usuario creado exitosamente: {NombreUsuario} | Contraseña: {Contrasena}", 
                nombreUsuarioSanitizado, 
                esContrasenaTemporal ? contrasenaTemporal : contrasenaFinal);
            return Ok(respuesta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando usuario");
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador,Ventas")] // Permitir que vendedores editen su propio perfil
    public async Task<IActionResult> Editar(int id, [FromBody] UsuarioDTO dto)
    {
        if (id != dto.IdUsuario) return BadRequest("El ID del usuario no coincide.");

        // Extraer el ID del usuario del token JWT
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            return Unauthorized("Token inválido o no contiene el ID del usuario.");
        }

        // Verificar si el usuario es administrador
        var isAdmin = User.IsInRole("Administrador");

        // Verificar propiedad del recurso
        if (!isAdmin && id != userId)
        {
            return StatusCode(403, new
            {
                message = "Solo puede editar su propio perfil.",
                error = "Forbidden",
                statusCode = 403,
                timestamp = DateTime.UtcNow
            });
        }

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

    [HttpGet("TestAcceso/{id}")]
    [Authorize(Roles = "Administrador,Ventas")] // Endpoint de prueba para verificar acceso
    public async Task<IActionResult> TestAcceso(int id)
    {
        try
        {
            // Extraer el ID del usuario del token JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized("Token inválido o no contiene el ID del usuario.");
            }

            // Verificar si el usuario es administrador
            var isAdmin = User.IsInRole("Administrador");

            // Obtener información del usuario actual
            var nombreUsuario = User.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido";
            var rolUsuario = User.FindFirst(ClaimTypes.Role)?.Value ?? "Sin rol";

            // Obtener el usuario solicitado
            var usuarioSolicitado = await _usuarioService.ObtenerPorId(id);
            if (usuarioSolicitado == null || usuarioSolicitado.IdUsuario == 0)
            {
                return NotFound($"No se encontró el usuario con ID: {id}");
            }

            // Verificar propiedad del recurso
            if (!isAdmin && id != userId)
            {
                return StatusCode(403, new
                {
                    message = $"No tiene permisos para acceder a este usuario. Usuario solicitado ID: {id}, su ID: {userId}",
                    error = "Forbidden",
                    statusCode = 403,
                    timestamp = DateTime.UtcNow
                });
            }

            // Respuesta de prueba con información detallada
            var resultadoPrueba = new
            {
                Mensaje = "Acceso permitido - Prueba exitosa",
                UsuarioActual = new
                {
                    Id = userId,
                    Nombre = nombreUsuario,
                    Rol = rolUsuario,
                    EsAdministrador = isAdmin
                },
                UsuarioSolicitado = new
                {
                    Id = usuarioSolicitado.IdUsuario,
                    NombreCompleto = usuarioSolicitado.NombreCompleto,
                    NombreUsuario = usuarioSolicitado.NombreUsuario,
                    Correo = usuarioSolicitado.Correo,
                    Rol = usuarioSolicitado.RefRol?.Nombre,
                    Activo = usuarioSolicitado.Activo == 1
                },
                Verificacion = new
                {
                    PropiedadVerificada = true,
                    MotivoAcceso = isAdmin ? "Usuario es Administrador" : "Usuario solicita su propio perfil",
                    Timestamp = DateTime.UtcNow
                }
            };

            return Ok(resultadoPrueba);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en prueba de acceso");
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpGet("TestError")]
    [Authorize(Roles = "Administrador")]
    public IActionResult TestError()
    {
        // Provocar un error no controlado para demostrar el comportamiento por defecto
        throw new InvalidOperationException("Este es un error de prueba para ver el comportamiento por defecto.");
    }
}