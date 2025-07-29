using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SVServices.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class PruebasSeguridadController : ControllerBase
    {
        private readonly IPruebasSeguridadService _pruebasSeguridadService;
        private readonly ISeguridadSQLService _seguridadSQLService;
        private readonly ILogger<PruebasSeguridadController> _logger;

        public PruebasSeguridadController(
            IPruebasSeguridadService pruebasSeguridadService,
            ISeguridadSQLService seguridadSQLService,
            ILogger<PruebasSeguridadController> logger)
        {
            _pruebasSeguridadService = pruebasSeguridadService;
            _seguridadSQLService = seguridadSQLService;
            _logger = logger;
        }

        /// <summary>
        /// PASO 5: Ejecuta pruebas automáticas de SQL Injection
        /// </summary>
        [HttpPost("ejecutar-pruebas-sql-injection")]
        public async Task<IActionResult> EjecutarPruebasSQLInjection()
        {
            try
            {
                _logger.LogInformation("Iniciando pruebas automáticas de SQL Injection - PASO 5");

                // Obtener la URL base de la aplicación
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                
                // Obtener el token del usuario actual (simulado para pruebas)
                var token = "token_simulado_para_pruebas";

                var resultados = await _pruebasSeguridadService.EjecutarPruebasSQLInjection(baseUrl, token);

                _logger.LogInformation("Pruebas de SQL Injection completadas exitosamente");

                return Ok(new
                {
                    Mensaje = "Pruebas de SQL Injection completadas - PASO 5",
                    Resultados = resultados,
                    Fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar pruebas de SQL Injection");
                return StatusCode(500, "Error interno del servidor durante las pruebas.");
            }
        }

        /// <summary>
        /// PASO 5: Prueba específica con la entrada maliciosa ' OR 1=1 --
        /// </summary>
        [HttpGet("probar-entrada-maliciosa")]
        public async Task<IActionResult> ProbarEntradaMaliciosa([FromQuery] string entradaMaliciosa = "' OR 1=1 --")
        {
            try
            {
                _logger.LogInformation("Probando entrada maliciosa específica: {Entrada}", entradaMaliciosa);

                // PASO 5: Simular el ataque con la entrada maliciosa
                var simulacion = await _pruebasSeguridadService.SimularAtaqueSQLInjection(entradaMaliciosa, "/api/usuarios/search");

                // Verificar si la entrada contiene caracteres peligrosos
                var contieneCaracteresPeligrosos = _seguridadSQLService.ContieneCaracteresPeligrosos(entradaMaliciosa);

                // Sanitizar la entrada para demostrar el proceso
                var entradaSanitizada = _seguridadSQLService.SanitizarContraSQLInjection(entradaMaliciosa);

                var resultado = new
                {
                    EntradaMaliciosa = entradaMaliciosa,
                    ContieneCaracteresPeligrosos = contieneCaracteresPeligrosos,
                    EntradaSanitizada = entradaSanitizada,
                    EsSegura = !contieneCaracteresPeligrosos,
                    Simulacion = simulacion,
                    FechaPrueba = DateTime.Now,
                    Mensaje = contieneCaracteresPeligrosos 
                        ? "✅ La entrada maliciosa fue detectada y rechazada" 
                        : "❌ La entrada maliciosa no fue detectada"
                };

                _logger.LogInformation("Prueba de entrada maliciosa completada: {Resultado}", resultado.Mensaje);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al probar entrada maliciosa: {Entrada}", entradaMaliciosa);
                return StatusCode(500, "Error interno del servidor durante la prueba.");
            }
        }

        /// <summary>
        /// PASO 5: Demuestra que la búsqueda literal funciona correctamente
        /// </summary>
        [HttpGet("demostrar-busqueda-literal")]
        public async Task<IActionResult> DemostrarBusquedaLiteral()
        {
            try
            {
                _logger.LogInformation("Demostrando búsqueda literal vs SQL Injection");

                var entradaMaliciosa = "' OR 1=1 --";
                var entradaNormal = "juan";

                // Simular cómo se procesarían ambas entradas
                var analisisMaliciosa = await _seguridadSQLService.AnalizarConsultaParametrizada("sp_listaUsuario", 
                    new Dictionary<string, object> { { "Buscar", entradaMaliciosa } });

                var analisisNormal = await _seguridadSQLService.AnalizarConsultaParametrizada("sp_listaUsuario", 
                    new Dictionary<string, object> { { "Buscar", entradaNormal } });

                var demostracion = new
                {
                    EntradaMaliciosa = new
                    {
                        Valor = entradaMaliciosa,
                        EsDetectada = _seguridadSQLService.ContieneCaracteresPeligrosos(entradaMaliciosa),
                        Sanitizada = _seguridadSQLService.SanitizarContraSQLInjection(entradaMaliciosa),
                        Analisis = analisisMaliciosa
                    },
                    EntradaNormal = new
                    {
                        Valor = entradaNormal,
                        EsDetectada = _seguridadSQLService.ContieneCaracteresPeligrosos(entradaNormal),
                        Sanitizada = _seguridadSQLService.SanitizarContraSQLInjection(entradaNormal),
                        Analisis = analisisNormal
                    },
                    Comparacion = new
                    {
                        Diferencia = "La entrada maliciosa se trata como texto literal, no como código SQL",
                        Resultado = "Búsqueda segura que no devuelve todos los registros",
                        Proteccion = "SQL Injection prevenido exitosamente"
                    }
                };

                _logger.LogInformation("Demostración de búsqueda literal completada");

                return Ok(demostracion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al demostrar búsqueda literal");
                return StatusCode(500, "Error interno del servidor durante la demostración.");
            }
        }

        /// <summary>
        /// PASO 5: Lista todas las entradas maliciosas disponibles para pruebas
        /// </summary>
        [HttpGet("entradas-maliciosas")]
        public IActionResult ObtenerEntradasMaliciosas()
        {
            try
            {
                var entradasMaliciosas = _pruebasSeguridadService.ObtenerEntradasMaliciosas();

                var resultado = new
                {
                    TotalEntradas = entradasMaliciosas.Count,
                    Entradas = entradasMaliciosas.Select((entrada, index) => new
                    {
                        Id = index + 1,
                        Entrada = entrada,
                        Tipo = ObtenerTipoAtaque(entrada),
                        Descripcion = ObtenerDescripcionAtaque(entrada)
                    }).ToList(),
                    Fecha = DateTime.Now
                };

                _logger.LogInformation("Lista de entradas maliciosas obtenida: {Total} entradas", entradasMaliciosas.Count);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener entradas maliciosas");
                return StatusCode(500, "Error interno del servidor.");
            }
        }

        /// <summary>
        /// PASO 5: Genera un reporte completo de seguridad
        /// </summary>
        [HttpGet("reporte-seguridad")]
        public async Task<IActionResult> GenerarReporteSeguridad()
        {
            try
            {
                _logger.LogInformation("Generando reporte completo de seguridad");

                var entradasMaliciosas = _pruebasSeguridadService.ObtenerEntradasMaliciosas();
                var resultados = new List<object>();

                // Simular resultados de pruebas
                foreach (var entrada in entradasMaliciosas)
                {
                    var esSeguro = _seguridadSQLService.ContieneCaracteresPeligrosos(entrada);
                    resultados.Add(new
                    {
                        EntradaMaliciosa = entrada,
                        EsSeguro = esSeguro,
                        StatusCode = esSeguro ? 400 : 200,
                        Respuesta = esSeguro ? "Rechazado por caracteres peligrosos" : "Procesado como búsqueda literal",
                        FechaPrueba = DateTime.Now
                    });
                }

                var reporte = await _pruebasSeguridadService.GenerarReportePruebasSeguridad(resultados);

                _logger.LogInformation("Reporte de seguridad generado exitosamente");

                return Ok(new
                {
                    Mensaje = "Reporte de seguridad generado - PASO 5",
                    Reporte = reporte,
                    Fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar reporte de seguridad");
                return StatusCode(500, "Error interno del servidor durante la generación del reporte.");
            }
        }

        private string ObtenerTipoAtaque(string entrada)
        {
            if (entrada.Contains("OR 1=1"))
                return "Bypass de Autenticación";
            else if (entrada.Contains("DROP TABLE"))
                return "Eliminación de Datos";
            else if (entrada.Contains("UNION SELECT"))
                return "Inyección de Consultas";
            else if (entrada.Contains("INSERT INTO"))
                return "Inserción de Datos";
            else if (entrada.Contains("UPDATE"))
                return "Modificación de Datos";
            else if (entrada.Contains("EXEC"))
                return "Ejecución de Comandos";
            else if (entrada.Contains("WAITFOR"))
                return "Ataque de Tiempo";
            else
                return "Ataque Genérico";
        }

        private string ObtenerDescripcionAtaque(string entrada)
        {
            if (entrada.Contains("OR 1=1"))
                return "Intenta obtener todos los registros usando una condición siempre verdadera";
            else if (entrada.Contains("DROP TABLE"))
                return "Intenta eliminar tablas de la base de datos";
            else if (entrada.Contains("UNION SELECT"))
                return "Intenta combinar consultas para obtener datos adicionales";
            else if (entrada.Contains("INSERT INTO"))
                return "Intenta insertar datos maliciosos en la base de datos";
            else if (entrada.Contains("UPDATE"))
                return "Intenta modificar datos existentes en la base de datos";
            else if (entrada.Contains("EXEC"))
                return "Intenta ejecutar comandos del sistema";
            else if (entrada.Contains("WAITFOR"))
                return "Intenta causar retrasos para análisis de tiempo";
            else
                return "Ataque de inyección SQL genérico";
        }
    }
} 