// En: SistemaVenta.API/Program.cs
using SVRepository;
using SVServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Shared.DTOs;

// Envolvemos todo en un bloque try-catch para capturar errores de arranque
try
{
    var builder = WebApplication.CreateBuilder(args);

    // 1. Agregar servicios básicos para la API
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // 2. Configurar la Inyección de Dependencias de tus proyectos
    builder.Services.RegisterRepositoryDependencies(builder.Configuration);
    builder.Services.RegisterServiceDependencies(builder.Configuration);
    // Añadimos esto para el endpoint de descarga de PDF que creamos
    builder.Services.AddHttpClient();

    // 3. Configurar CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("NuevaPolitica", app =>
        {
            app.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
        });
    });

    // 4. Configurar la autenticación con JWT
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

    // 5. Configurar el pipeline de peticiones HTTP
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
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
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    // Si algo falla durante el arranque, lo capturamos aquí
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("!!!!!!!!!! ERROR FATAL AL INICIAR LA APLICACIÓN !!!!!!!!!!");
    Console.WriteLine(ex.ToString()); // Imprime el error completo con todos sus detalles
    Console.ResetColor();
    Console.WriteLine("\nPresiona cualquier tecla para cerrar...");
    Console.ReadKey();
}