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
            response.EnsureSuccessStatusCode();

            var sessionDto = await response.Content.ReadFromJsonAsync<SessionDTO>();

            if (sessionDto == null || string.IsNullOrWhiteSpace(sessionDto.Token))
                throw new Exception("No se recibió un token de sesión válido.");

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