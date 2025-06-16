// En: SistemaVenta.Web/SistemaVenta.Web/Program.cs
using SistemaVenta.Web.Client.Pages; // Aseg�rate de tener una p�gina para referenciar, ej: Home, Counter, etc.
using SistemaVenta.Web.Client.Services.Interfaces;
using SistemaVenta.Web.Components;
using SistemaVenta.Web.Client.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAuthService, RemoteAuthService>();
// 1. A�adir servicios para componentes Razor y Blazor.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents(); // Indica que usaremos un proyecto cliente de WebAssembly

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

// 2. Mapear los componentes Razor.
//    Esta l�nea le dice a la aplicaci�n que renderice el componente 'App'
//    y que la interactividad la manejar� el proyecto Cliente.
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly); // Reemplaza 'Counter' con el nombre de cualquier p�gina de tu proyecto .Client

app.Run();