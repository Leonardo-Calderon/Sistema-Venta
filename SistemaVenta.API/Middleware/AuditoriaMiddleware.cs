using Microsoft.AspNetCore.Http;
using SVServices.Interfaces;
using System.Security.Claims;

namespace SistemaVenta.API.Middleware
{
    public class AuditoriaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditoriaMiddleware> _logger;

        public AuditoriaMiddleware(RequestDelegate next, ILogger<AuditoriaMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuditoriaService auditoriaService)
        {
            var startTime = DateTime.UtcNow;
            var originalBodyStream = context.Response.Body;

            try
            {
                // Capturar información de la petición
                var endpoint = context.Request.Path;
                var metodo = context.Request.Method;
                var ipAddress = GetClientIpAddress(context);
                var userAgent = context.Request.Headers["User-Agent"].ToString();

                // Verificar si el usuario está autenticado
                var usuario = context.User;
                var resultado = "Pendiente";
                var detalles = "";

                // Continuar con el pipeline
                using var memoryStream = new MemoryStream();
                context.Response.Body = memoryStream;

                await _next(context);

                // Capturar información de la respuesta
                memoryStream.Position = 0;
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
                memoryStream.Position = 0;
                await memoryStream.CopyToAsync(originalBodyStream);

                // Determinar el resultado basado en el código de estado
                resultado = DetermineResult(context.Response.StatusCode);

                // Registrar el acceso si el usuario está autenticado
                if (usuario.Identity?.IsAuthenticated == true)
                {
                    detalles = $"IP: {ipAddress}, User-Agent: {userAgent}, Status: {context.Response.StatusCode}";
                    await auditoriaService.RegistrarAcceso(usuario, endpoint, metodo, resultado, detalles);
                }
                else if (endpoint.Value?.StartsWith("/api/auth/") == true)
                {
                    // Registrar intentos de autenticación
                    var nombreUsuario = await ExtractUsernameFromRequest(context.Request);
                    await auditoriaService.RegistrarAutenticacion(nombreUsuario, resultado, ipAddress, $"Status: {context.Response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en middleware de auditoría");
                
                // Restaurar el stream original en caso de error
                context.Response.Body = originalBodyStream;
                
                // Registrar el error si el usuario está autenticado
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var endpoint = context.Request.Path;
                    var metodo = context.Request.Method;
                    await auditoriaService.RegistrarAcceso(context.User, endpoint, metodo, "Error", $"Excepción: {ex.Message}");
                }
                
                throw;
            }
        }

        private string GetClientIpAddress(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            
            // Verificar headers de proxy
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            else if (context.Request.Headers.ContainsKey("X-Real-IP"))
            {
                ip = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            }

            return ip ?? "Desconocida";
        }

        private string DetermineResult(int statusCode)
        {
            return statusCode switch
            {
                200 => "Permitido",
                201 => "Permitido",
                204 => "Permitido",
                400 => "Error_Cliente",
                401 => "No_Autenticado",
                403 => "Denegado",
                404 => "No_Encontrado",
                500 => "Error_Servidor",
                _ => "Otro"
            };
        }

        private async Task<string> ExtractUsernameFromRequest(HttpRequest request)
        {
            try
            {
                if (request.Method == "POST" && request.ContentType?.Contains("application/json") == true)
                {
                    request.EnableBuffering();
                    request.Body.Position = 0;
                    
                    using var reader = new StreamReader(request.Body, leaveOpen: true);
                    var body = await reader.ReadToEndAsync();
                    request.Body.Position = 0;

                    // Buscar el nombre de usuario en el JSON
                    if (body.Contains("nombreUsuario"))
                    {
                        // Extracción simple del nombre de usuario
                        var startIndex = body.IndexOf("\"nombreUsuario\":") + 16;
                        var endIndex = body.IndexOf("\"", startIndex);
                        if (startIndex > 15 && endIndex > startIndex)
                        {
                            return body.Substring(startIndex, endIndex - startIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo extraer el nombre de usuario de la petición");
            }

            return "Desconocido";
        }
    }
} 