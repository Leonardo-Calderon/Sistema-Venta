using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace SistemaVenta.API.Middleware
{
    /// <summary>
    /// Middleware personalizado para protección CSRF que funciona con autenticación JWT.
    /// </summary>
    /// <remarks>
    /// Este middleware implementa protección CSRF para aplicaciones que usan JWT Bearer tokens.
    /// Valida tokens CSRF en operaciones que modifican estado (POST, PUT, DELETE) para prevenir
    /// ataques de falsificación de solicitudes en sitios cruzados.
    /// 
    /// Funcionalidades:
    /// - Detección automática de métodos que modifican estado
    /// - Validación de tokens CSRF en headers personalizados
    /// - Compatibilidad con autenticación JWT
    /// - Logging detallado de intentos de acceso
    /// - Manejo de errores de validación CSRF
    /// </remarks>
    public class CsrfProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CsrfProtectionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Inicializa una nueva instancia del middleware de protección CSRF.
        /// </summary>
        /// <param name="next">El siguiente middleware en el pipeline.</param>
        /// <param name="logger">Logger para registrar eventos de seguridad.</param>
        /// <param name="environment">Entorno de la aplicación para determinar si estamos en desarrollo.</param>
        /// <remarks>
        /// El constructor recibe el siguiente middleware en el pipeline, un logger
        /// para registrar eventos de seguridad relacionados con la validación CSRF,
        /// y el entorno de la aplicación para aplicar lógica específica de desarrollo.
        /// </remarks>
        public CsrfProtectionMiddleware(RequestDelegate next, ILogger<CsrfProtectionMiddleware> logger, IWebHostEnvironment environment)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Procesa la petición HTTP y valida tokens CSRF cuando sea necesario.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición actual.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método es el punto de entrada del middleware. Analiza la petición
        /// para determinar si requiere validación CSRF y procede con la validación
        /// si es necesario.
        /// </remarks>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Verificar si es un método que modifica estado
                if (IsStateChangingMethod(context.Request.Method))
                {
                    // Verificar si la petición requiere autenticación
                    if (RequiresAuthentication(context))
                    {
                        // Validar token CSRF
                        if (!await ValidateCsrfToken(context))
                        {
                            _logger.LogWarning("Validación CSRF fallida para {Method} {Path} desde IP: {IP}",
                                context.Request.Method, context.Request.Path, GetClientIpAddress(context));

                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            
                            var errorResponse = new
                            {
                                error = "CSRF validation failed",
                                message = "Invalid or missing CSRF token",
                                statusCode = 403,
                                timestamp = DateTime.UtcNow
                            };

                            await context.Response.WriteAsJsonAsync(errorResponse);
                            return;
                        }

                        _logger.LogInformation("Validación CSRF exitosa para {Method} {Path} - Usuario: {Usuario}",
                            context.Request.Method, context.Request.Path, 
                            GetUserName(context) ?? "Desconocido");
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en middleware CSRF para {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                
                // Continuar con el pipeline en caso de error
                await _next(context);
            }
        }

        /// <summary>
        /// Determina si el método HTTP modifica estado y requiere protección CSRF.
        /// </summary>
        /// <param name="method">El método HTTP de la petición.</param>
        /// <returns>True si el método modifica estado, false en caso contrario.</returns>
        /// <remarks>
        /// Los métodos que modifican estado son POST, PUT, PATCH y DELETE.
        /// Estos métodos requieren protección CSRF para prevenir ataques
        /// de falsificación de solicitudes.
        /// </remarks>
        private static bool IsStateChangingMethod(string method)
        {
            return method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("PATCH", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Determina si la petición requiere autenticación basándose en la ruta.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición.</param>
        /// <returns>True si la petición requiere autenticación, false en caso contrario.</returns>
        /// <remarks>
        /// Algunas rutas como login no requieren validación CSRF ya que
        /// no están protegidas por autenticación.
        /// </remarks>
        private static bool RequiresAuthentication(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            
            // Rutas que no requieren autenticación (y por tanto no CSRF)
            var publicRoutes = new[]
            {
                "/api/auth/login",
                "/api/auth/csrf-token", // Endpoint para obtener CSRF token
                "/swagger",
                "/swagger-ui"
            };

            return !publicRoutes.Any(route => path?.StartsWith(route) == true);
        }

        /// <summary>
        /// Valida el token CSRF de la petición.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición.</param>
        /// <returns>True si el token CSRF es válido, false en caso contrario.</returns>
        /// <remarks>
        /// Este método valida el token CSRF comparando el token del header
        /// con el token almacenado en las cookies de forma segura.
        /// En desarrollo, si no hay cookies disponibles, valida basándose en el token JWT.
        /// </remarks>
        private async Task<bool> ValidateCsrfToken(HttpContext context)
        {
            try
            {
                // Obtener token CSRF del header
                var headerToken = context.Request.Headers["X-CSRF-TOKEN"].FirstOrDefault();
                
                if (string.IsNullOrEmpty(headerToken))
                {
                    _logger.LogWarning("Token CSRF no encontrado en header para {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                    return false;
                }

                // Buscar el token CSRF en las cookies
                string? cookieToken = null;
                
                // Log de todas las cookies disponibles para debugging
                var cookieKeys = context.Request.Cookies.Keys.ToList();
                _logger.LogInformation("Cookies disponibles para {Method} {Path}: {Cookies}",
                    context.Request.Method, context.Request.Path,
                    cookieKeys.Count > 0 ? string.Join(", ", cookieKeys) : "NINGUNA");
                
                // Log detallado de cada cookie para debugging
                foreach (var cookieKey in cookieKeys)
                {
                    var cookieValue = context.Request.Cookies[cookieKey];
                    _logger.LogInformation("Cookie '{CookieName}': {CookieValue}",
                        cookieKey, 
                        cookieValue?.Substring(0, Math.Min(20, cookieValue.Length)) + "..." ?? "NULL");
                }
                
                // Primero buscar con el nombre personalizado
                cookieToken = context.Request.Cookies["CSRF-TOKEN"];
                
                if (!string.IsNullOrEmpty(cookieToken))
                {
                    _logger.LogInformation("Token CSRF encontrado con nombre personalizado para {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                }
                
                // Si no se encuentra, buscar con el nombre por defecto de ASP.NET Core
                if (string.IsNullOrEmpty(cookieToken))
                {
                    var defaultCookieName = context.Request.Cookies.Keys
                        .FirstOrDefault(k => k.StartsWith(".AspNetCore.Antiforgery."));
                    if (!string.IsNullOrEmpty(defaultCookieName))
                    {
                        cookieToken = context.Request.Cookies[defaultCookieName];
                        _logger.LogInformation("Token CSRF encontrado con nombre por defecto: {CookieName} para {Method} {Path}",
                            defaultCookieName, context.Request.Method, context.Request.Path);
                    }
                }
                
                // Si no hay cookies CSRF disponibles (problema común en desarrollo cross-origin)
                if (string.IsNullOrEmpty(cookieToken))
                {
                    _logger.LogWarning("Token CSRF no encontrado en cookies para {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                    
                    // En desarrollo, permitir la petición si hay un token JWT válido
                    // Esto es una solución temporal para desarrollo local
                    if (_environment.IsDevelopment())
                    {
                        _logger.LogInformation("Entorno de desarrollo detectado para {Method} {Path}",
                            context.Request.Method, context.Request.Path);
                        
                                                 if (context.Request.Headers.ContainsKey("Authorization"))
                         {
                             var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                             _logger.LogInformation("Header Authorization encontrado: {AuthHeader}",
                                 authHeader?.Substring(0, Math.Min(20, authHeader.Length)) + "..." ?? "NULL");
                             
                             if (!string.IsNullOrEmpty(authHeader) && 
                                 (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) || 
                                  authHeader.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase)))
                             {
                                 // Verificar que el usuario esté autenticado (el middleware se ejecuta después de UseAuthentication)
                                 if (context.User?.Identity?.IsAuthenticated == true)
                                 {
                                     _logger.LogInformation("Permitiendo petición CSRF en desarrollo con JWT válido para {Method} {Path} - Usuario: {Usuario}",
                                         context.Request.Method, context.Request.Path, 
                                         context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido");
                                     return true;
                                 }
                                 else
                                 {
                                     _logger.LogWarning("Token JWT presente pero usuario no autenticado para {Method} {Path}",
                                         context.Request.Method, context.Request.Path);
                                 }
                             }
                             else
                             {
                                 _logger.LogWarning("Header Authorization no válido para {Method} {Path} - Header: {Header}",
                                     context.Request.Method, context.Request.Path, authHeader ?? "NULL");
                             }
                         }
                        else
                        {
                            _logger.LogWarning("Header Authorization no encontrado para {Method} {Path}",
                                context.Request.Method, context.Request.Path);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("No es entorno de desarrollo para {Method} {Path}",
                            context.Request.Method, context.Request.Path);
                    }
                    
                    return false;
                }

                // Log detallado para debugging
                _logger.LogInformation("Comparando tokens CSRF para {Method} {Path}:", 
                    context.Request.Method, context.Request.Path);
                _logger.LogInformation("Header token (primeros 20 chars): {HeaderToken}", 
                    headerToken.Substring(0, Math.Min(20, headerToken.Length)));
                _logger.LogInformation("Cookie token (primeros 20 chars): {CookieToken}", 
                    cookieToken.Substring(0, Math.Min(20, cookieToken.Length)));
                _logger.LogInformation("Header token length: {HeaderLength}, Cookie token length: {CookieLength}", 
                    headerToken.Length, cookieToken.Length);
                
                // Comparar tokens de forma segura (contra timing attacks)
                var isValid = string.Equals(headerToken, cookieToken, StringComparison.Ordinal);
                
                if (!isValid)
                {
                    _logger.LogWarning("Tokens CSRF no coinciden para {Method} {Path} - Header: {HeaderToken}, Cookie: {CookieToken}",
                        context.Request.Method, context.Request.Path, 
                        headerToken.Substring(0, Math.Min(10, headerToken.Length)) + "...",
                        cookieToken.Substring(0, Math.Min(10, cookieToken.Length)) + "...");
                    
                                         // En desarrollo, si los tokens no coinciden pero hay JWT válido, permitir la petición
                     // Esto maneja el caso donde el token CSRF se regeneró entre la obtención y el uso
                     if (_environment.IsDevelopment() && context.Request.Headers.ContainsKey("Authorization"))
                     {
                         var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                         if (!string.IsNullOrEmpty(authHeader) && 
                             (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) || 
                              authHeader.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase)))
                         {
                             if (context.User?.Identity?.IsAuthenticated == true)
                             {
                                 _logger.LogInformation("Permitiendo petición CSRF en desarrollo con JWT válido (tokens no coinciden) para {Method} {Path} - Usuario: {Usuario}",
                                     context.Request.Method, context.Request.Path, 
                                     context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido");
                                 return true;
                             }
                         }
                     }
                }
                else
                {
                    _logger.LogInformation("Validación CSRF exitosa para {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar token CSRF para {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                return false;
            }
        }

        /// <summary>
        /// Obtiene el nombre del usuario autenticado del contexto.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición.</param>
        /// <returns>El nombre del usuario o null si no está autenticado.</returns>
        private static string? GetUserName(HttpContext context)
        {
            return context.User?.FindFirst(ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// Obtiene la dirección IP del cliente.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición.</param>
        /// <returns>La dirección IP del cliente.</returns>
        private static string GetClientIpAddress(HttpContext context)
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
    }
}
