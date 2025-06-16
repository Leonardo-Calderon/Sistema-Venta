// En: SistemaVenta.Web/SistemaVenta.Web.Client/Program.cs
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Interfaces;
using SistemaVenta.Web.Client.Services.Implementations;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// --- CONFIGURACIÓN DEL HTTPCLIENT ---

// Registra el manejador que añadirá el token a las peticiones
builder.Services.AddScoped<AuthorizationMessageHandler>();

// --- AÑADE ESTA LÍNEA AQUÍ ---
// Define la variable con la dirección de tu API. 
// Reemplaza 7206 por el puerto correcto si es diferente.
string apiBaseAddress = "https://localhost:7206/";
// ---------------------------------

// Configura el HttpClient principal para usar el manejador
builder.Services.AddHttpClient("ApiHttpClient", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress); // Ahora la variable 'apiBaseAddress' sí existe
}).AddHttpMessageHandler<AuthorizationMessageHandler>();

// La forma correcta de hacer que un HttpClient esté disponible para inyección en Blazor WASM
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiHttpClient"));


// --- REGISTRO DE SERVICIOS DEL CLIENTE ---
// Aquí es donde registras las implementaciones de tus servicios
builder.Services.AddScoped<IAuthService, AuthService>();
// builder.Services.AddScoped<IMenuService, MenuService>(); // Descomenta cuando lo crees
// etc...


await builder.Build().RunAsync();