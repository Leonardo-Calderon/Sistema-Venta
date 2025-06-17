// En: SistemaVenta.Web/SistemaVenta.Web/Program.cs

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using SistemaVenta.Web;
using SistemaVenta.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// 1. A�adir servicios para componentes Razor y Blazor.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents(); // Indica que usaremos un proyecto cliente de WebAssembly

// 2. Configurar la AUTENTICACI�N del SERVIDOR.
//    Esto es solo para p�ginas que se rendericen en el servidor (si las hubiera).
//    El cliente WASM tiene su propia configuraci�n.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Si un usuario no autenticado intenta acceder a un recurso del SERVIDOR,
        // ser� redirigido a la p�gina de login (la ra�z en este caso).
        options.LoginPath = "/";
    });

// 3. Configurar la AUTORIZACI�N del SERVIDOR (ESTA ES LA CORRECCI�N CLAVE).
//    Se establece una pol�tica por defecto que SOLO se aplica al esquema de Cookies.
//    Esto evita que el servidor interfiera con la autorizaci�n de JWT del cliente.
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
        .Build();
});

// 4. Registrar los proveedores de estado de autenticaci�n.
//    - ServerAuthenticationStateProvider se usa solo en el servidor (devuelve an�nimo).
//    - El cliente registrar� el suyo (CustomAuthenticationStateProvider).
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

// -- NO REGISTRES SERVICIOS DE L�GICA (IUsuarioService, etc.) AQU� --
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

// Activa la autenticaci�n y autorizaci�n en el pipeline.
app.UseAuthentication();
app.UseAuthorization();

// 5. Mapear los componentes Razor.
//    Esta l�nea le dice a la aplicaci�n que renderice el componente 'App'
//    y que la interactividad la manejar� el proyecto Cliente.
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(SistemaVenta.Web.Client.Pages.Usuarios).Assembly); // Referencia a un tipo del proyecto Cliente

app.Run();