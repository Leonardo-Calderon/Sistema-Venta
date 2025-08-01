using Microsoft.AspNetCore.Http;
using System.Text;

namespace SistemaVenta.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Log de la request
                _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

                // Si es un POST, loggear el body (solo para login)
                if (context.Request.Method == "POST" && context.Request.Path.Value?.Contains("/api/auth/login") == true)
                {
                    context.Request.EnableBuffering();
                    var body = await ReadRequestBodyAsync(context.Request);
                    _logger.LogInformation($"Login Request Body: {body}");
                    context.Request.Body.Position = 0;
                }

                await _next(context);

                // Log de la response
                _logger.LogInformation($"Response: {context.Response.StatusCode} for {context.Request.Method} {context.Request.Path}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing request: {context.Request.Method} {context.Request.Path}");
                throw;
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true);
            return await reader.ReadToEndAsync();
        }
    }
} 