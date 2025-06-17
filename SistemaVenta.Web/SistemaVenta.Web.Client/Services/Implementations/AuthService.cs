// En: SistemaVenta.Web.Client/Services/Implementation/AuthService.cs
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTOs;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net;
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
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error {response.StatusCode}: {errorContent}");
                }

                var sessionDto = await response.Content.ReadFromJsonAsync<SessionDTO>();

                // Guardar el token en localStorage
                await _localStorage.SetItemAsStringAsync("authToken", sessionDto.Token);


                // Notificar cambio de estado de autenticación
                await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                    .NotifyUserAuthentication(sessionDto.Token);

                return sessionDto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en AuthService: {ex}");
                throw;
            }
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