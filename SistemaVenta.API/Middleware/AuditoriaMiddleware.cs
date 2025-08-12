using Microsoft.AspNetCore.Http;
using SVServices.Interfaces;
using System.Security.Claims;

namespace SistemaVenta.API.Middleware
{
    /// <summary>
    /// Middleware para registrar auditoría de todas las peticiones HTTP en la API.
    /// </summary>
    /// <remarks>
    /// Este middleware captura y registra información detallada de todas las peticiones
    /// HTTP que llegan a la API, incluyendo:
    /// - Información del usuario autenticado
    /// - Detalles de la petición (endpoint, método, IP, User-Agent)
    /// - Información de la respuesta (código de estado, tiempo de respuesta)
    /// - Intentos de autenticación (exitosos y fallidos)
    /// - Errores y excepciones
    /// 
    /// Proporciona trazabilidad completa de las actividades en el sistema
    /// para fines de seguridad, cumplimiento y depuración.
    /// </remarks>
    public class AuditoriaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditoriaMiddleware> _logger;

        /// <summary>
        /// Inicializa una nueva instancia del middleware de auditoría.
        /// </summary>
        /// <param name="next">El siguiente middleware en el pipeline.</param>
        /// <param name="logger">Logger para registrar información de auditoría.</param>
        /// <remarks>
        /// El constructor recibe el siguiente middleware en el pipeline y un logger
        /// para registrar información detallada de las actividades de auditoría.
        /// </remarks>
        public AuditoriaMiddleware(RequestDelegate next, ILogger<AuditoriaMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Procesa la petición HTTP y registra información de auditoría.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición actual.</param>
        /// <param name="auditoriaService">Servicio de auditoría para registrar las actividades.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método es el punto de entrada del middleware. Captura información
        /// de la petición, procesa la respuesta y registra los detalles de auditoría
        /// según el tipo de actividad (acceso autenticado, autenticación, errores).
        /// 
        /// Utiliza un MemoryStream para capturar el cuerpo de la respuesta sin
        /// interferir con el flujo normal de la petición.
        /// </remarks>
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

                // Continuar con el pipeline usando MemoryStream para capturar la respuesta
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

        /// <summary>
        /// Obtiene la dirección IP real del cliente, considerando headers de proxy.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición actual.</param>
        /// <returns>La dirección IP del cliente o "Desconocida" si no se puede determinar.</returns>
        /// <remarks>
        /// Este método verifica múltiples headers para obtener la IP real del cliente,
        /// considerando que la aplicación puede estar detrás de un proxy o load balancer.
        /// Verifica en orden: X-Forwarded-For, X-Real-IP, y finalmente la IP de conexión.
        /// </remarks>
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

        /// <summary>
        /// Determina el resultado de la petición basándose en el código de estado HTTP.
        /// </summary>
        /// <param name="statusCode">El código de estado HTTP de la respuesta.</param>
        /// <returns>Una cadena que describe el resultado de la petición.</returns>
        /// <remarks>
        /// Este método categoriza los códigos de estado HTTP en resultados legibles:
        /// - 2xx: "Exitoso"
        /// - 4xx: "Error del Cliente"
        /// - 5xx: "Error del Servidor"
        /// - Otros: "Desconocido"
        /// </remarks>
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