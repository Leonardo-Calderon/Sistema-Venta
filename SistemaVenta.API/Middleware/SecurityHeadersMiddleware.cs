using Microsoft.AspNetCore.Http;

namespace SistemaVenta.API.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Agregar cabeceras de seguridad antes de procesar la respuesta
            AddSecurityHeaders(context);

            await _next(context);
        }

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