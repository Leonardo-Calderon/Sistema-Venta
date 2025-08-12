// En: SistemaVenta.Web/SistemaVenta.Web/Program.cs

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using SistemaVenta.Web;
using SistemaVenta.Web.Components;

/// <summary>
/// Punto de entrada principal de la aplicación web del Sistema de Ventas.
/// </summary>
/// <remarks>
/// Este archivo configura la aplicación ASP.NET Core que sirve como servidor
/// para la aplicación Blazor WebAssembly. Incluye:
/// - Configuración de componentes Razor interactivos
/// - Configuración de autenticación basada en cookies
/// - Configuración de autorización
/// - Configuración del proveedor de estado de autenticación
/// - Configuración del pipeline de middleware
/// - Mapeo de componentes Blazor WebAssembly
/// </remarks>
var builder = WebApplication.CreateBuilder(args);

// Configurar componentes Razor con soporte para WebAssembly interactivo
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents(); 

// Configurar autenticación basada en cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/";
    });

// Configurar autorización con política por defecto que requiere autenticación
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
        .Build();
});

// Registrar el proveedor de estado de autenticación personalizado
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

var app = builder.Build();

// Configurar el pipeline de peticiones HTTP
if (app.Environment.IsDevelopment())
{
    // Habilitar depuración de WebAssembly en desarrollo
    app.UseWebAssemblyDebugging();
}
else
{
    // Configurar manejo de errores en producción
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Configurar middleware de seguridad y archivos estáticos
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Configurar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapear componentes Razor con soporte para WebAssembly interactivo
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SistemaVenta.Web.Client.Pages.Usuarios).Assembly); // Referencia a un tipo del proyecto Cliente

app.Run();