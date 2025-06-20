using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTOs;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<SessionDTO> Login(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            // Lanza una excepción si la respuesta no es exitosa para un mejor manejo de errores en la UI
            response.EnsureSuccessStatusCode();

            var sessionDto = await response.Content.ReadFromJsonAsync<SessionDTO>();

            if (sessionDto == null || string.IsNullOrWhiteSpace(sessionDto.Token))
                throw new Exception("No se recibió un token de sesión válido.");

            // El proveedor se encarga de guardar el token y notificar a la app
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                .NotifyUserAuthentication(sessionDto.Token);

            return sessionDto;
        }

        public async Task Logout()
        {
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
        }
    }
}