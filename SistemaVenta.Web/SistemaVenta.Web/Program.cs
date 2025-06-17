// En: SistemaVenta.Web/SistemaVenta.Web/Program.cs

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using SistemaVenta.Web;
using SistemaVenta.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. Añadir servicios para componentes Razor y Blazor.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents(); // Indica que usaremos un proyecto cliente de WebAssembly

// 2. Configurar la AUTENTICACIÓN del SERVIDOR.
//    Esto es solo para páginas que se rendericen en el servidor (si las hubiera).
//    El cliente WASM tiene su propia configuración.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Si un usuario no autenticado intenta acceder a un recurso del SERVIDOR,
        // será redirigido a la página de login (la raíz en este caso).
        options.LoginPath = "/";
    });

// 3. Configurar la AUTORIZACIÓN del SERVIDOR (ESTA ES LA CORRECCIÓN CLAVE).
//    Se establece una política por defecto que SOLO se aplica al esquema de Cookies.
//    Esto evita que el servidor interfiera con la autorización de JWT del cliente.
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
        .Build();
});

// 4. Registrar los proveedores de estado de autenticación.
//    - ServerAuthenticationStateProvider se usa solo en el servidor (devuelve anónimo).
//    - El cliente registrará el suyo (CustomAuthenticationStateProvider).
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// -- NO REGISTRES SERVICIOS DE LÓGICA (IUsuarioService, etc.) AQUÍ --
//    Estos servicios se resuelven en el cliente (WASM), no en el servidor.
//    Las implementaciones Dummy ya no son necesarias.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Activa la autenticación y autorización en el pipeline.
app.UseAuthentication();
app.UseAuthorization();

// 5. Mapear los componentes Razor.
//    Esta línea le dice a la aplicación que renderice el componente 'App'
//    y que la interactividad la manejará el proyecto Cliente.
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SistemaVenta.Web.Client.Pages.Usuarios).Assembly); // Referencia a un tipo del proyecto Cliente

app.Run();