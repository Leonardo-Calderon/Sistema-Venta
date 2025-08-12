using Microsoft.AspNetCore.Http;

namespace SistemaVenta.API.Middleware
{
    /// <summary>
    /// Middleware para agregar cabeceras de seguridad HTTP a todas las respuestas de la API.
    /// </summary>
    /// <remarks>
    /// Este middleware implementa medidas de seguridad defensivas agregando cabeceras HTTP
    /// que protegen contra diversos tipos de ataques web comunes:
    /// - Clickjacking (X-Frame-Options)
    /// - Cross-Site Scripting (Content-Security-Policy)
    /// - MIME sniffing (X-Content-Type-Options)
    /// - Information disclosure (Referrer-Policy)
    /// - Cross-domain policies (X-Permitted-Cross-Domain-Policies)
    /// - Feature policies (Permissions-Policy)
    /// 
    /// Se ejecuta en cada petición para asegurar que todas las respuestas incluyan
    /// estas cabeceras de seguridad.
    /// </remarks>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Inicializa una nueva instancia del middleware de cabeceras de seguridad.
        /// </summary>
        /// <param name="next">El siguiente middleware en el pipeline.</param>
        /// <remarks>
        /// El constructor recibe el siguiente middleware en el pipeline para
        /// continuar el procesamiento de la petición después de agregar las
        /// cabeceras de seguridad.
        /// </remarks>
        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// Procesa la petición HTTP y agrega cabeceras de seguridad a la respuesta.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición actual.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método es el punto de entrada del middleware. Agrega las cabeceras
        /// de seguridad antes de procesar la petición y luego continúa con el
        /// siguiente middleware en el pipeline.
        /// </remarks>
        public async Task InvokeAsync(HttpContext context)
        {
            // Agregar cabeceras de seguridad antes de procesar la respuesta
            AddSecurityHeaders(context);

            await _next(context);
        }

        /// <summary>
        /// Agrega las cabeceras de seguridad HTTP a la respuesta.
        /// </summary>
        /// <param name="context">El contexto HTTP de la petición actual.</param>
        /// <remarks>
        /// Este método agrega múltiples cabeceras de seguridad que protegen contra:
        /// - X-Frame-Options: Previene que la página sea embebida en iframes (Clickjacking)
        /// - Content-Security-Policy: Define políticas de seguridad de contenido (XSS)
        /// - X-Content-Type-Options: Previene el sniffing de tipos MIME
        /// - Referrer-Policy: Controla qué información de referente se envía
        /// - X-Permitted-Cross-Domain-Policies: Restringe políticas cross-domain
        /// - Permissions-Policy: Controla el acceso a características del navegador
        /// </remarks>
        private void AddSecurityHeaders(HttpContext context)
        {
            // X-Frame-Options: Previene Clickjacking
            context.Response.Headers.Add("X-Frame-Options", "DENY");

            // Content-Security-Policy: Previene XSS
            context.Response.Headers.Add("Content-Security-Policy", 
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https:; " +
                "font-src 'self'; " +
                "connect-src 'self'; " +
                "frame-ancestors 'none';");

            // X-Content-Type-Options: Previene MIME sniffing
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

            // Referrer-Policy: Controla información de referente
            context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");

            // X-Permitted-Cross-Domain-Policies: Controla políticas cross-domain
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");

            // Permissions-Policy: Controla características del navegador
            context.Response.Headers.Add("Permissions-Policy", 
                "geolocation=(), microphone=(), camera=(), payment=()");
        }
    }
} 