// En: SistemaVenta.Web/SistemaVenta.Web.Client/Auth/CustomAuthenticationStateProvider.cs
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace SistemaVenta.Web.Client.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            
            try
            {
                var token = await _localStorage.GetItemAsStringAsync("authToken");
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new AuthenticationState(_anonymous);
                }

                var claimsPrincipal = CreateClaimsPrincipalFromToken(token);
                return new AuthenticationState(claimsPrincipal);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Proveedor Cliente: EXCEPCIÓN -> {ex.Message}");
                return new AuthenticationState(_anonymous);
            }
        }

        public Task NotifyUserAuthentication(string token)
        {
            var authenticatedUser = CreateClaimsPrincipalFromToken(token);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            // Notificamos a Blazor del cambio de estado
            NotifyAuthenticationStateChanged(authState);

            return Task.CompletedTask;
        }

        public Task NotifyUserLogout()
        {
            var authState = Task.FromResult(new AuthenticationState(_anonymous));

            // Notificamos a Blazor que el usuario ya no está autenticado
            NotifyAuthenticationStateChanged(authState);

            return Task.CompletedTask;
        }

        private ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims,
                authenticationType: "jwtAuth",
                nameType: ClaimTypes.Name,
                roleType: ClaimTypes.Role);

            return new ClaimsPrincipal(identity);
        }
    }
}