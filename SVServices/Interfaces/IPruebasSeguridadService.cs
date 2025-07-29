using System.Net.Http;

namespace SVServices.Interfaces
{
    public interface IPruebasSeguridadService
    {
        /// <summary>
        /// Ejecuta pruebas de SQL Injection contra los endpoints de búsqueda
        /// </summary>
        /// <param name="baseUrl">URL base de la API</param>
        /// <param name="token">Token de autenticación</param>
        /// <returns>Resultado de las pruebas de seguridad</returns>
        Task<object> EjecutarPruebasSQLInjection(string baseUrl, string token);

        /// <summary>
        /// Prueba un endpoint específico con entradas maliciosas
        /// </summary>
        /// <param name="endpoint">Endpoint a probar</param>
        /// <param name="entradasMaliciosas">Lista de entradas maliciosas</param>
        /// <param name="httpClient">Cliente HTTP configurado</param>
        /// <returns>Resultados de las pruebas</returns>
        Task<List<object>> ProbarEndpointConEntradasMaliciosas(string endpoint, List<string> entradasMaliciosas, HttpClient httpClient);

        /// <summary>
        /// Genera un reporte de seguridad con los resultados de las pruebas
        /// </summary>
        /// <param name="resultados">Resultados de las pruebas</param>
        /// <returns>Reporte detallado de seguridad</returns>
        Task<string> GenerarReportePruebasSeguridad(List<object> resultados);

        /// <summary>
        /// Valida que una respuesta sea segura (no contenga datos sensibles)
        /// </summary>
        /// <param name="respuesta">Respuesta HTTP</param>
        /// <param name="entradaMaliciosa">Entrada maliciosa utilizada</param>
        /// <returns>True si la respuesta es segura</returns>
        bool ValidarRespuestaSegura(string respuesta, string entradaMaliciosa);

        /// <summary>
        /// Obtiene la lista de entradas maliciosas para pruebas
        /// </summary>
        /// <returns>Lista de entradas maliciosas</returns>
        List<string> ObtenerEntradasMaliciosas();

        /// <summary>
        /// Simula un ataque de SQL Injection y documenta el resultado
        /// </summary>
        /// <param name="entradaMaliciosa">Entrada maliciosa a probar</param>
        /// <param name="endpoint">Endpoint objetivo</param>
        /// <returns>Documentación del resultado de la prueba</returns>
        Task<string> SimularAtaqueSQLInjection(string entradaMaliciosa, string endpoint);
    }
} 