using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SVServices.Interfaces;
using System.Security.Claims;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")] // Solo administradores pueden consultar auditoría
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaService _auditoriaService;
        private readonly ILogger<AuditoriaController> _logger;

        public AuditoriaController(IAuditoriaService auditoriaService, ILogger<AuditoriaController> logger)
        {
            _auditoriaService = auditoriaService;
            _logger = logger;
        }

        [HttpGet("historial/{idUsuario}")]
        public async Task<IActionResult> ObtenerHistorialUsuario(int idUsuario, [FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                // Validar fechas
                if (fechaInicio > fechaFin)
                {
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
                }

                // Limitar el rango de fechas a máximo 30 días
                if ((fechaFin - fechaInicio).Days > 30)
                {
                    return BadRequest("El rango de fechas no puede exceder 30 días.");
                }

                var historial = await _auditoriaService.ObtenerHistorialAuditoria(idUsuario, fechaInicio, fechaFin);

                return Ok(new
                {
                    UsuarioId = idUsuario,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TotalRegistros = historial.Count,
                    Registros = historial
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de auditoría para usuario {UsuarioId}", idUsuario);
                return StatusCode(500, "Error interno del servidor al obtener historial de auditoría.");
            }
        }

        [HttpGet("estadisticas")]
        public async Task<IActionResult> ObtenerEstadisticas([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                // Validar fechas
                if (fechaInicio > fechaFin)
                {
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
                }

                // Limitar el rango de fechas a máximo 90 días
                if ((fechaFin - fechaInicio).Days > 90)
                {
                    return BadRequest("El rango de fechas no puede exceder 90 días.");
                }

                var estadisticas = await _auditoriaService.ObtenerEstadisticasAuditoria(fechaInicio, fechaFin);

                return Ok(estadisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de auditoría");
                return StatusCode(500, "Error interno del servidor al obtener estadísticas de auditoría.");
            }
        }

        [HttpGet("mi-actividad")]
        [Authorize(Roles = "Administrador,Vendedor")] // Permitir que cualquier usuario autenticado vea su propia actividad
        public async Task<IActionResult> ObtenerMiActividad([FromQuery] DateTime fechaInicio, [FromQuery] DateTime fechaFin)
        {
            try
            {
                // Extraer el ID del usuario del token JWT
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized("Token inválido o no contiene el ID del usuario.");
                }

                // Validar fechas
                if (fechaInicio > fechaFin)
                {
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
                }

                // Limitar el rango de fechas a máximo 7 días para usuarios normales
                if ((fechaFin - fechaInicio).Days > 7)
                {
                    return BadRequest("El rango de fechas no puede exceder 7 días.");
                }

                var historial = await _auditoriaService.ObtenerHistorialAuditoria(userId, fechaInicio, fechaFin);

                return Ok(new
                {
                    UsuarioId = userId,
                    NombreUsuario = User.FindFirst(ClaimTypes.Name)?.Value,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    TotalRegistros = historial.Count,
                    Registros = historial
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener actividad del usuario actual");
                return StatusCode(500, "Error interno del servidor al obtener actividad.");
            }
        }

        [HttpGet("resumen-diario")]
        public async Task<IActionResult> ObtenerResumenDiario([FromQuery] DateTime fecha)
        {
            try
            {
                var fechaInicio = fecha.Date;
                var fechaFin = fecha.Date.AddDays(1).AddSeconds(-1);

                var estadisticas = await _auditoriaService.ObtenerEstadisticasAuditoria(fechaInicio, fechaFin);

                return Ok(new
                {
                    Fecha = fecha.Date,
                    Estadisticas = estadisticas
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener resumen diario de auditoría");
                return StatusCode(500, "Error interno del servidor al obtener resumen diario.");
            }
        }
    }
} 