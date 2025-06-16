// En: SistemaVenta.Web.Client/Services/Implementation/AuthService.cs
using Blazored.LocalStorage;
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
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<SessionDTO> Login(LoginDTO loginDto)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            var session = await result.Content.ReadFromJsonAsync<SessionDTO>();

            if (session != null && session.Token != null)
            {
                // Guardamos el token en el almacenamiento local del navegador
                await _localStorage.SetItemAsStringAsync("authToken", session.Token);

                // Notificamos a Blazor que el estado de autenticación ha cambiado
                await ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(session.Token);
            }

            return session;
        }

        public async Task Logout()
        {
            // Eliminamos el token del almacenamiento local
            await _localStorage.RemoveItemAsync("authToken");

            // Notificamos a Blazor que el usuario ha cerrado sesión
            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
        }
    }
}