using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Implementations;
using SistemaVenta.Web.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// --- SERVICIOS DE AUTENTICACI�N ---
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// El proveedor de autenticaci�n personalizado se registra como Scoped
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// --- CONFIGURACI�N DE HTTPCLIENT (SIMPLIFICADO) ---
// Registramos un �nico HttpClient que ser� usado en toda la aplicaci�n.
// CustomAuthenticationStateProvider se encargar� de a�adirle el token.
builder.Services.AddScoped(sp =>
{
    // Obtenemos la URL base de la configuraci�n (wwwroot/appsettings.json)
    var apiUrl = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;
    return new HttpClient { BaseAddress = new Uri(apiUrl) };
});


// --- REGISTRO DE SERVICIOS DEL CLIENTE ---
// Los servicios que dependen de HttpClient usar�n la instancia configurada arriba
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IMedidaService, MedidaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();
// ... registra aqu� tus otros servicios ...

await builder.Build().RunAsync();