using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SistemaVenta.Web;

/// <summary>
/// Proveedor de estado de autenticación para el servidor Blazor.
/// </summary>
/// <remarks>
/// Esta clase proporciona el estado de autenticación para la aplicación
/// Blazor Server. En este caso, siempre retorna un usuario anónimo ya que
/// la autenticación real se maneja en el lado cliente (Blazor WebAssembly).
/// 
/// Esta implementación es necesaria para que el servidor pueda manejar
/// las peticiones de componentes Razor que requieren autenticación,
/// aunque la lógica de autenticación real se ejecuta en el navegador.
/// </remarks>
public class ServerAuthenticationStateProvider : AuthenticationStateProvider
{
    /// <summary>
    /// Obtiene el estado de autenticación actual.
    /// </summary>
    /// <returns>Una tarea que representa el estado de autenticación anónimo.</returns>
    /// <remarks>
    /// Este método siempre retorna un estado de autenticación anónimo
    /// ya que la autenticación real se maneja en el cliente Blazor WebAssembly.
    /// 
    /// El servidor no necesita manejar la autenticación directamente
    /// porque toda la lógica de autenticación se ejecuta en el navegador.
    /// </remarks>
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymousIdentity = new ClaimsIdentity();
        var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
        return Task.FromResult(new AuthenticationState(anonymousUser));
    }
}