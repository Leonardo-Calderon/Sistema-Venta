using Microsoft.AspNetCore.Components.Authorization;
using Shared.DTOs;
using SistemaVenta.Web.Client.Auth;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de autenticación para la aplicación cliente.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IAuthService y proporciona la funcionalidad
    /// para manejar la autenticación de usuarios en la aplicación Blazor WebAssembly.
    /// 
    /// Responsabilidades:
    /// - Comunicación con la API de autenticación
    /// - Gestión del estado de autenticación
    /// - Almacenamiento y eliminación de tokens JWT
    /// - Manejo de errores de autenticación
    /// - Notificación de cambios en el estado de autenticación
    /// </remarks>
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ICsrfService _csrfService;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de autenticación.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para comunicación con la API.</param>
        /// <param name="authenticationStateProvider">Proveedor de estado de autenticación.</param>
        /// <remarks>
        /// El constructor recibe las dependencias necesarias para el funcionamiento
        /// del servicio, incluyendo el cliente HTTP para comunicación con la API
        /// y el proveedor de estado de autenticación para gestionar el estado
        /// de la sesión del usuario.
        /// </remarks>
        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ICsrfService csrfService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authenticationStateProvider = authenticationStateProvider ?? throw new ArgumentNullException(nameof(authenticationStateProvider));
            _csrfService = csrfService ?? throw new ArgumentNullException(nameof(csrfService));
        }

        /// <summary>
        /// Autentica un usuario con las credenciales proporcionadas.
        /// </summary>
        /// <param name="loginDto">Datos de login del usuario (nombre de usuario y contraseña).</param>
        /// <returns>
        /// Los datos de sesión del usuario si la autenticación es exitosa.
        /// </returns>
        /// <exception cref="HttpRequestException">Se lanza cuando la petición HTTP falla.</exception>
        /// <exception cref="Exception">Se lanza cuando no se recibe un token válido.</exception>
        /// <remarks>
        /// Este método:
        /// 1. Envía las credenciales a la API de autenticación
        /// 2. Verifica que la respuesta sea exitosa
        /// 3. Deserializa la respuesta para obtener los datos de sesión
        /// 4. Valida que se haya recibido un token válido
        /// 5. Notifica al proveedor de autenticación sobre el inicio de sesión
        /// 6. Retorna los datos de sesión del usuario
        /// 
        /// Si la autenticación falla, lanza una excepción con el mensaje de error.
        /// </remarks>
        public async Task<SessionDTO> Login(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            response.EnsureSuccessStatusCode();

            var sessionDto = await response.Content.ReadFromJsonAsync<SessionDTO>();

            if (sessionDto == null || string.IsNullOrWhiteSpace(sessionDto.Token))
                throw new Exception("No se recibió un token de sesión válido.");

            // Obtener token CSRF después del login exitoso
            try
            {
                await _csrfService.GetCsrfTokenAsync();
            }
            catch (Exception ex)
            {
                // Log del error pero no fallar el login
                Console.WriteLine($"Error al obtener token CSRF después del login: {ex.Message}");
            }

            await ((CustomAuthenticationStateProvider)_authenticationStateProvider)
                .NotifyUserAuthentication(sessionDto.Token);

            return sessionDto;
        }

        /// <summary>
        /// Cierra la sesión del usuario actual.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método:
        /// 1. Notifica al proveedor de autenticación sobre el cierre de sesión
        /// 2. El proveedor se encarga de eliminar el token del almacenamiento local
        /// 3. Limpia el estado de autenticación
        /// 
        /// Después de llamar a este método, el usuario será redirigido a la página de login
        /// y perderá acceso a las funcionalidades que requieren autenticación.
        /// </remarks>
        public async Task Logout()
        {
            // Limpiar token CSRF al cerrar sesión
            try
            {
                await _csrfService.ClearCsrfTokenAsync();
            }
            catch (Exception ex)
            {
                // Log del error pero no fallar el logout
                Console.WriteLine($"Error al limpiar token CSRF durante logout: {ex.Message}");
            }

            await ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
        }
    }
}