using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace SistemaVenta.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // 1. Registrar el error real para el equipo de desarrollo
            _logger.LogError(exception, "Error no controlado en la aplicación: {Message}", exception.Message);

            // 2. Determinar el tipo de error y el código de estado apropiado
            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "Ha ocurrido un error interno en el servidor.";
            var userMessage = "Por favor, inténtelo de nuevo más tarde. Si el problema persiste, contacte al administrador del sistema.";

            // 3. Manejar tipos específicos de errores
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

            // 4. Preparar respuesta
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