using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SistemaVenta.Web;

public class ServerAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymousIdentity = new ClaimsIdentity();
        var anonymousUser = new ClaimsPrincipal(anonymousIdentity);
        return Task.FromResult(new AuthenticationState(anonymousUser));
    }
}