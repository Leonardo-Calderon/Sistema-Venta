using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SVServices.Interfaces;
using Microsoft.Extensions.Logging;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class SeguridadController : ControllerBase
    {
        private readonly ISeguridadSQLService _seguridadSQLService;
        private readonly ILogger<SeguridadController> _logger;

        public SeguridadController(ISeguridadSQLService seguridadSQLService, ILogger<SeguridadController> logger)
        {
            _seguridadSQLService = seguridadSQLService;
            _logger = logger;
        }

        /// <summary>
        /// Analiza la seguridad de la creación de usuarios
        /// </summary>
        [HttpGet("analizar-creacion-usuario")]
        public async Task<IActionResult> AnalizarCreacionUsuario()
        {
            try
            {
                // Simular parámetros de creación de usuario
                var parametros = new Dictionary<string, object>
                {
                    { "IdRol", 2 },
                    { "NombreCompleto", "Juan Pérez" },
                    { "Correo", "juan.perez@empresa.com" },
                    { "NombreUsuario", "juan.perez" },
                    { "Clave", "clave_encriptada_sha256" }
                };

                var analisis = await _seguridadSQLService.AnalizarConsultaParametrizada("sp_crearUsuario", parametros);
                
                _logger.LogInformation("Análisis de seguridad de creación de usuario completado");
                return Ok(new { 
                    Mensaje = "Análisis de seguridad completado",
                    Analisis = analisis,
                    Fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al analizar seguridad de creación de usuario");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Simula una consulta vulnerable para demostrar la diferencia
        /// </summary>
        [HttpGet("simular-consulta-vulnerable")]
        public IActionResult SimularConsultaVulnerable()
        {
            try
            {
                var consultaVulnerable = "INSERT INTO Usuario (NombreCompleto, Correo) VALUES ('Juan Pérez', 'juan@test.com')";
                var simulacion = _seguridadSQLService.SimularConsultaVulnerable(consultaVulnerable);
                
                _logger.LogInformation("Simulación de consulta vulnerable completada");
                return Ok(new { 
                    Mensaje = "Simulación de consulta vulnerable completada",
                    Simulacion = simulacion,
                    Fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al simular consulta vulnerable");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Genera un reporte de seguridad para el stored procedure de usuarios
        /// </summary>
        [HttpGet("reporte-seguridad-usuario")]
        public async Task<IActionResult> ReporteSeguridadUsuario()
        {
            try
            {
                var parametros = new Dictionary<string, object>
                {
                    { "IdRol", 2 },
                    { "NombreCompleto", "María García" },
                    { "Correo", "maria@empresa.com" },
                    { "NombreUsuario", "maria.garcia" },
                    { "Clave", "clave_encriptada_sha256" }
                };

                var reporte = await _seguridadSQLService.GenerarReporteSeguridad("sp_crearUsuario", parametros);
                
                _logger.LogInformation("Reporte de seguridad de usuario generado");
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar reporte de seguridad de usuario");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Documenta el flujo de seguridad para operaciones CRUD
        /// </summary>
        [HttpGet("documentar-flujo/{operacion}/{entidad}")]
        public IActionResult DocumentarFlujo(string operacion, string entidad)
        {
            try
            {
                var documentacion = _seguridadSQLService.DocumentarFlujoSeguridad(operacion, entidad);
                
                _logger.LogInformation("Documentación de flujo de seguridad generada: {Operacion} {Entidad}", operacion, entidad);
                return Ok(new { 
                    Mensaje = "Documentación de flujo de seguridad generada",
                    Documentacion = documentacion,
                    Operacion = operacion,
                    Entidad = entidad,
                    Fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al documentar flujo de seguridad: {Operacion} {Entidad}", operacion, entidad);
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// Compara consultas seguras vs vulnerables
        /// </summary>
        [HttpGet("comparar-consultas")]
        public IActionResult CompararConsultas()
        {
            try
            {
                var comparacion = new
                {
                    ConsultaVulnerable = new
                    {
                        Descripcion = "Concatenación directa de strings SQL",
                        Ejemplo = "INSERT INTO Usuario (NombreCompleto) VALUES ('" + "Juan'; DROP TABLE Usuarios; --" + "')",
                        Riesgo = "ALTO - Vulnerable a SQL Injection",
                        Resultado = "Se ejecutaría: INSERT INTO Usuario (NombreCompleto) VALUES ('Juan'; DROP TABLE Usuarios; --')"
                    },
                    ConsultaSegura = new
                    {
                        Descripcion = "Stored Procedure con parámetros tipados",
                        Ejemplo = "EXEC sp_crearUsuario @NombreCompleto = @nombreCompleto",
                        Riesgo = "BAJO - Protegido contra SQL Injection",
                        Resultado = "El parámetro se trata como valor, no como código SQL"
                    },
                    Diferencias = new[]
                    {
                        "Separación de comando SQL y datos",
                        "Uso de parámetros tipados",
                        "Escape automático de caracteres especiales",
                        "Validación de tipos en SQL Server",
                        "Prevención de inyección de código"
                    }
                };

                _logger.LogInformation("Comparación de consultas completada");
                return Ok(comparacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al comparar consultas");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
} 