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
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
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
    
    // Configurar CORS con orígenes específicos para mayor seguridad
    app.UseCors(builder => builder
        .WithOrigins(
            "https://localhost:7289",
            "https://localhost:5001",
            "https://localhost:7189"   
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
    );
    
    app.UseAuthentication();
    app.UseAuthorization();
    
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