using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Implementations;
using SistemaVenta.Web.Client.Services.Interfaces;

/// <summary>
/// Punto de entrada principal de la aplicación cliente Blazor WebAssembly.
/// </summary>
/// <remarks>
/// Este archivo configura la aplicación Blazor WebAssembly que se ejecuta
/// en el navegador del cliente. Incluye:
/// - Configuración de servicios de almacenamiento local
/// - Configuración de autenticación y autorización
/// - Configuración del cliente HTTP para comunicación con la API
/// - Registro de todos los servicios de la aplicación
/// - Configuración del proveedor de estado de autenticación personalizado
/// </remarks>
var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Configurar almacenamiento local para persistencia de datos en el navegador
builder.Services.AddBlazoredLocalStorage();

// Configurar servicios de autenticación y autorización
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// Registrar el proveedor de estado de autenticación personalizado
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Configurar cliente HTTP para comunicación con la API
builder.Services.AddScoped(sp =>
{
    var apiUrl = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;
    return new HttpClient { BaseAddress = new Uri(apiUrl) };
});

// Registrar todos los servicios de la aplicación
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IMedidaService, MedidaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<INegocioService, NegocioService>();

// Registrar servicio CSRF para protección contra ataques Cross-Site Request Forgery
builder.Services.AddScoped<ICsrfService, CsrfService>();

await builder.Build().RunAsync();