using SVServices.Interfaces;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace SVServices.Implementation
{
    public class AuditoriaService : IAuditoriaService
    {
        private readonly ILogger<AuditoriaService> _logger;

        public AuditoriaService(ILogger<AuditoriaService> logger)
        {
            _logger = logger;
        }

        public async Task RegistrarAcceso(ClaimsPrincipal usuario, string endpoint, string metodo, string resultado, string detalles = "")
        {
            try
            {
                var userId = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Desconocido";
                var nombreUsuario = usuario.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido";
                var rol = usuario.FindFirst(ClaimTypes.Role)?.Value ?? "Sin rol";

                var mensaje = $"ACCESO - Usuario: {nombreUsuario} (ID: {userId}, Rol: {rol}) | " +
                             $"Endpoint: {metodo} {endpoint} | " +
                             $"Resultado: {resultado} | " +
                             $"Detalles: {detalles} | " +
                             $"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC";

                // Registrar en log estructurado
                _logger.LogInformation(mensaje);

                // Aquí se podría agregar el registro en base de datos
                // await _auditoriaRepository.RegistrarAcceso(userId, endpoint, metodo, resultado, detalles);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar acceso en auditoría");
            }
        }

        public async Task RegistrarAutorizacion(ClaimsPrincipal usuario, string recurso, string accion, string resultado, string motivo = "")
        {
            try
            {
                var userId = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Desconocido";
                var nombreUsuario = usuario.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido";
                var rol = usuario.FindFirst(ClaimTypes.Role)?.Value ?? "Sin rol";

                var mensaje = $"AUTORIZACIÓN - Usuario: {nombreUsuario} (ID: {userId}, Rol: {rol}) | " +
                             $"Recurso: {recurso} | " +
                             $"Acción: {accion} | " +
                             $"Resultado: {resultado} | " +
                             $"Motivo: {motivo} | " +
                             $"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC";

                // Registrar en log estructurado
                _logger.LogInformation(mensaje);

                // Aquí se podría agregar el registro en base de datos
                // await _auditoriaRepository.RegistrarAutorizacion(userId, recurso, accion, resultado, motivo);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar autorización en auditoría");
            }
        }

        public async Task RegistrarAutenticacion(string nombreUsuario, string resultado, string ipAddress, string detalles = "")
        {
            try
            {
                var mensaje = $"AUTENTICACIÓN - Usuario: {nombreUsuario} | " +
                             $"Resultado: {resultado} | " +
                             $"IP: {ipAddress} | " +
                             $"Detalles: {detalles} | " +
                             $"Timestamp: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC";

                // Registrar en log estructurado
                _logger.LogInformation(mensaje);

                // Aquí se podría agregar el registro en base de datos
                // await _auditoriaRepository.RegistrarAutenticacion(nombreUsuario, resultado, ipAddress, detalles);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar autenticación en auditoría");
            }
        }

        public async Task<List<object>> ObtenerHistorialAuditoria(int idUsuario, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                // Aquí se implementaría la consulta a la base de datos
                // var historial = await _auditoriaRepository.ObtenerHistorialPorUsuario(idUsuario, fechaInicio, fechaFin);
                
                // Por ahora retornamos una lista vacía
                var historial = new List<object>();
                
                _logger.LogInformation($"Consulta de historial de auditoría para usuario {idUsuario} desde {fechaInicio} hasta {fechaFin}");

                return await Task.FromResult(historial);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de auditoría");
                return await Task.FromResult(new List<object>());
            }
        }

        public async Task<object> ObtenerEstadisticasAuditoria(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                // Aquí se implementaría la consulta de estadísticas a la base de datos
                // var estadisticas = await _auditoriaRepository.ObtenerEstadisticas(fechaInicio, fechaFin);
                
                // Por ahora retornamos un objeto con estadísticas simuladas
                var estadisticas = new
                {
                    Periodo = $"{fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}",
                    TotalAccesos = 0,
                    AccesosPermitidos = 0,
                    AccesosDenegados = 0,
                    TotalAutenticaciones = 0,
                    AutenticacionesExitosas = 0,
                    AutenticacionesFallidas = 0,
                    UsuariosActivos = 0,
                    EndpointsMasAccedidos = new List<object>(),
                    UsuariosMasActivos = new List<object>()
                };
                
                _logger.LogInformation($"Consulta de estadísticas de auditoría desde {fechaInicio} hasta {fechaFin}");

                return await Task.FromResult(estadisticas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estadísticas de auditoría");
                return await Task.FromResult(new object());
            }
        }
    }
} 