// En: SistemaVenta.Web/SistemaVenta.Web.Client/Program.cs
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Interfaces;
using SistemaVenta.Web.Client.Services.Implementations;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// --- CONFIGURACIÓN DEL HTTPCLIENT ---

// Registra el manejador que añadirá el token a las peticiones
builder.Services.AddScoped<AuthorizationMessageHandler>();

// --- AÑADE ESTA LÍNEA AQUÍ ---
// Define la variable con la dirección de tu API. 
// Reemplaza 7206 por el puerto correcto si es diferente.
string apiBaseAddress = builder.Configuration["ApiUrl"] ?? "https://localhost:7206/";
// ---------------------------------

// Configura el HttpClient principal para usar el manejador
// En Program.cs de Web.Client
builder.Services.AddHttpClient("ApiHttpClient", client =>
{
    client.BaseAddress = new Uri(apiBaseAddress);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
})
.AddHttpMessageHandler<AuthorizationMessageHandler>();

// La forma correcta de hacer que un HttpClient esté disponible para inyección en Blazor WASM
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiHttpClient"));


// --- REGISTRO DE SERVICIOS DEL CLIENTE ---
// Aquí es donde registras las implementaciones de tus servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();



await builder.Build().RunAsync();