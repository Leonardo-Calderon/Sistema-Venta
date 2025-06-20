using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Implementations;
using SistemaVenta.Web.Client.Services.Interfaces;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// --- SERVICIOS DE AUTENTICACIÓN ---
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// El proveedor de autenticación personalizado se registra como Scoped
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// --- CONFIGURACIÓN DE HTTPCLIENT (SIMPLIFICADO) ---
// Registramos un único HttpClient que será usado en toda la aplicación.
// CustomAuthenticationStateProvider se encargará de añadirle el token.
builder.Services.AddScoped(sp =>
{
    // Obtenemos la URL base de la configuración (wwwroot/appsettings.json)
    var apiUrl = builder.Configuration["ApiUrl"] ?? builder.HostEnvironment.BaseAddress;
    return new HttpClient { BaseAddress = new Uri(apiUrl) };
});


// --- REGISTRO DE SERVICIOS DEL CLIENTE ---
// Los servicios que dependen de HttpClient usarán la instancia configurada arriba
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IMedidaService, MedidaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();
// ... registra aquí tus otros servicios ...

await builder.Build().RunAsync();