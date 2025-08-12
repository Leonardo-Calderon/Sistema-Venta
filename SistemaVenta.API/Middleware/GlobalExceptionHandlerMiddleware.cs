using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace SistemaVenta.API.Middleware
{
    /// <summary>
    /// Middleware para el manejo global de excepciones en la aplicación API.
    /// </summary>
    /// <remarks>
    /// Este middleware captura todas las excepciones no controladas que ocurren
    /// durante el procesamiento de las peticiones HTTP. Proporciona:
    /// - Logging detallado de errores para el equipo de desarrollo
    /// - Respuestas de error estructuradas y consistentes para los clientes
    /// - Manejo específico de diferentes tipos de excepciones
    /// - Información de contexto útil para depuración
    /// 
    /// Se ejecuta temprano en el pipeline para asegurar que todas las excepciones
    /// sean capturadas y manejadas de manera uniforme.
    /// </remarks>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del middleware de manejo global de excepciones.
        /// </summary>
        /// <param name="next">El siguiente middleware en el pipeline.</param>
        /// <param name="logger">Logger para registrar información de errores.</param>
        /// <remarks>
        /// El constructor recibe el siguiente middleware en el pipeline y un logger
        /// para registrar información detallada de las excepciones que ocurran.
        /// </remarks>
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Procesa la petición HTTP y maneja cualquier excepción que ocurra.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición actual.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método es el punto de entrada del middleware. Ejecuta el siguiente
        /// middleware en el pipeline y captura cualquier excepción que ocurra,
        /// delegando el manejo específico al método HandleExceptionAsync.
        /// </remarks>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Maneja una excepción específica y genera una respuesta HTTP apropiada.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición actual.</param>
        /// <param name="exception">La excepción que se debe manejar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método:
        /// 1. Registra el error detallado para el equipo de desarrollo
        /// 2. Determina el tipo de error y código de estado apropiado
        /// 3. Maneja tipos específicos de excepciones (SQL, Argument, Unauthorized)
        /// 4. Genera una respuesta JSON estructurada con información útil
        /// 5. Incluye información de contexto para facilitar la depuración
        /// </remarks>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Registrar el error real para el equipo de desarrollo
            _logger.LogError(exception, "Error no controlado en la aplicación: {Message}", exception.Message);

            // Determinar el tipo de error y el código de estado apropiado
            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "Ha ocurrido un error interno en el servidor.";
            var userMessage = "Por favor, inténtelo de nuevo más tarde. Si el problema persiste, contacte al administrador del sistema.";

            // Manejar tipos específicos de errores
            if (exception is SqlException sqlEx)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = "Error de base de datos";
                userMessage = "Error en la base de datos. Verifique los datos ingresados.";
                _logger.LogError(sqlEx, "Error de SQL: {Number} - {Message}", sqlEx.Number, sqlEx.Message);
            }
            else if (exception is ArgumentException argEx)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = "Datos inválidos";
                userMessage = argEx.Message;
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "No autorizado";
                userMessage = "No tiene permisos para realizar esta acción.";
            }

            // Preparar respuesta estructurada
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                error = errorMessage,
                message = userMessage,
                timestamp = DateTime.UtcNow,
                requestId = context.TraceIdentifier,
                path = context.Request.Path,
                method = context.Request.Method
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }
} 