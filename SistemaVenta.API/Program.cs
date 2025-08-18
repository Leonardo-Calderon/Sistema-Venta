// En: SistemaVenta.API/Program.cs
using SVRepository;
using SVServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SistemaVenta.API.Middleware;

/// <summary>
/// Punto de entrada principal de la aplicación API del Sistema de Ventas.
/// </summary>
/// <remarks>
/// Este archivo configura toda la aplicación ASP.NET Core, incluyendo:
/// - Configuración de servicios y dependencias
/// - Configuración de autenticación JWT
/// - Configuración de CORS y seguridad
/// - Configuración del pipeline de middleware
/// - Manejo global de excepciones
/// 
/// La aplicación está envuelta en un bloque try-catch para capturar
/// errores fatales durante el arranque y proporcionar información
/// detallada de depuración.
/// </remarks>
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Configurar servicios básicos para la API
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configurar servicios anti-CSRF para protección contra ataques Cross-Site Request Forgery
    builder.Services.AddAntiforgery(options =>
    {
        options.HeaderName = "X-CSRF-TOKEN";
        options.Cookie.Name = "CSRF-TOKEN";
        options.Cookie.HttpOnly = true;
        // En desarrollo, permitir cookies no seguras para localhost
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        // En desarrollo, usar SameSite=None para permitir cross-origin en localhost
        // En producción, cambiar a SameSiteMode.Strict
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.IsEssential = true;
        // Configurar dominio para desarrollo cross-origin
        options.Cookie.Domain = null; // Permite que la cookie se envíe a cualquier subdominio
    });

    // Configurar la Inyección de Dependencias de los proyectos de la solución
    builder.Services.RegisterRepositoryDependencies(builder.Configuration);
    builder.Services.RegisterServiceDependencies(builder.Configuration);
    
    // Agregar HttpClient para operaciones HTTP externas (ej: descarga de PDF)
    builder.Services.AddHttpClient();

    // Configurar CORS para permitir comunicación con aplicaciones cliente
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("NuevaPolitica", app =>
        {
            app.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
        });
        
        // Política específica para desarrollo con credenciales
        options.AddPolicy("DevelopmentPolicy", app =>
        {
            app.WithOrigins(
                "https://localhost:7289",
                "https://localhost:5001",
                "https://localhost:7189"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithExposedHeaders("X-CSRF-TOKEN", "Set-Cookie")
            .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
    });

    // Configurar la autenticación con JWT Bearer Token
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
        });

    var app = builder.Build();

    // Configurar el pipeline de peticiones HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Habilitar HSTS (HTTP Strict Transport Security) para forzar conexiones HTTPS
    app.UseHsts();

    app.UseRouting();
    
    // Agregar middleware de manejo global de excepciones
    app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    
    // Agregar middleware de cabeceras de seguridad
    app.UseMiddleware<SecurityHeadersMiddleware>();
    
    // Configurar CORS ANTES del middleware CSRF para evitar bloqueos
    app.UseCors("DevelopmentPolicy");
    
    // Agregar middleware anti-CSRF para protección contra ataques Cross-Site Request Forgery
    app.UseAntiforgery();
    
    app.UseAuthentication();
    app.UseAuthorization();
    
    // Agregar middleware personalizado de protección CSRF para JWT DESPUÉS de la autenticación
    app.UseMiddleware<CsrfProtectionMiddleware>();
    
    // Agregar middleware de auditoría para registrar actividades
    app.UseMiddleware<AuditoriaMiddleware>();
    
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Capturar errores fatales durante el arranque de la aplicación
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("!!!!!!!!!! ERROR FATAL AL INICIAR LA APLICACIÓN !!!!!!!!!!");
    Console.WriteLine(ex.ToString()); // Imprime el error completo con todos sus detalles
    Console.ResetColor();
    Console.WriteLine("\nPresiona cualquier tecla para cerrar...");
    Console.ReadKey();
}