using Blazored.LocalStorage;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de gestión de tokens CSRF para la aplicación cliente.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz ICsrfService y proporciona la funcionalidad
    /// para manejar tokens CSRF en la aplicación Blazor WebAssembly.
    /// 
    /// Responsabilidades:
    /// - Comunicación con la API para obtener tokens CSRF
    /// - Almacenamiento local de tokens CSRF
    /// - Gestión del ciclo de vida de tokens CSRF
    /// - Manejo de errores de comunicación
    /// - Validación de tokens CSRF
    /// </remarks>
    public class CsrfService : ICsrfService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private const string CsrfTokenKey = "csrfToken";
        private const string CsrfTokenExpiryKey = "csrfTokenExpiry";

        /// <summary>
        /// Inicializa una nueva instancia del servicio CSRF.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para comunicación con la API.</param>
        /// <param name="localStorage">Servicio de almacenamiento local.</param>
        /// <remarks>
        /// El constructor recibe las dependencias necesarias para el funcionamiento
        /// del servicio, incluyendo el cliente HTTP para comunicación con la API
        /// y el servicio de almacenamiento local para persistir tokens CSRF.
        /// </remarks>
        public CsrfService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        }

        /// <summary>
        /// Obtiene un token CSRF válido de la API.
        /// </summary>
        /// <returns>
        /// El token CSRF si se obtiene exitosamente.
        /// </returns>
        /// <exception cref="HttpRequestException">Se lanza cuando la petición HTTP falla.</exception>
        /// <exception cref="Exception">Se lanza cuando no se recibe un token válido.</exception>
        /// <remarks>
        /// Este método realiza una petición GET a la API para obtener un nuevo
        /// token CSRF. El token se almacena localmente con una marca de tiempo
        /// para controlar su validez.
        /// </remarks>
        public async Task<string> GetCsrfTokenAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/auth/csrf-token");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<CsrfTokenResponse>();
                
                if (result?.csrfToken == null)
                {
                    throw new Exception("No se recibió un token CSRF válido de la API.");
                }

                // Almacenar el token con marca de tiempo (válido por 1 hora)
                var expiryTime = DateTime.UtcNow.AddHours(1);
                // Asegurar que el token se almacene sin comillas
                var cleanToken = result.csrfToken?.Trim('"') ?? string.Empty;
                await _localStorage.SetItemAsync(CsrfTokenKey, cleanToken);
                await _localStorage.SetItemAsync(CsrfTokenExpiryKey, expiryTime);

                return cleanToken;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException($"Error al obtener token CSRF: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error inesperado al obtener token CSRF: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene el token CSRF actual, renovándolo si es necesario.
        /// </summary>
        /// <returns>
        /// El token CSRF actual o un nuevo token si el actual no es válido.
        /// </returns>
        /// <remarks>
        /// Este método verifica si existe un token CSRF válido localmente.
        /// Si no existe o ha expirado, obtiene un nuevo token de la API.
        /// Es el método principal que deben usar los servicios para obtener
        /// tokens CSRF antes de realizar operaciones que modifican estado.
        /// </remarks>
        public async Task<string> GetCurrentCsrfTokenAsync()
        {
            try
            {
                // En desarrollo, siempre obtener un token fresco para evitar problemas de sincronización
                // En producción, se puede optimizar para usar tokens almacenados
                return await GetCsrfTokenAsync();
            }
            catch (Exception ex)
            {
                // En caso de error, intentar obtener un nuevo token
                return await GetCsrfTokenAsync();
            }
        }

        /// <summary>
        /// Limpia el token CSRF almacenado localmente.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método elimina el token CSRF y su marca de tiempo del
        /// almacenamiento local. Se debe llamar cuando el usuario cierra
        /// sesión o cuando se detecta que el token ha expirado.
        /// </remarks>
        public async Task ClearCsrfTokenAsync()
        {
            try
            {
                await _localStorage.RemoveItemAsync(CsrfTokenKey);
                await _localStorage.RemoveItemAsync(CsrfTokenExpiryKey);
            }
            catch (Exception ex)
            {
                // Log del error pero no lanzar excepción para no interrumpir el flujo
                Console.WriteLine($"Error al limpiar token CSRF: {ex.Message}");
            }
        }

        /// <summary>
        /// Verifica si el token CSRF actual es válido.
        /// </summary>
        /// <returns>
        /// True si el token CSRF es válido, false en caso contrario.
        /// </returns>
        /// <remarks>
        /// Este método verifica si existe un token CSRF válido localmente
        /// sin realizar una petición a la API. Verifica tanto la existencia
        /// del token como su marca de tiempo de expiración.
        /// </remarks>
        public async Task<bool> IsCsrfTokenValidAsync()
        {
            try
            {
                // Verificar si existe el token
                var token = await _localStorage.GetItemAsStringAsync(CsrfTokenKey);
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }

                // Verificar si existe la marca de tiempo
                var expiryTime = await _localStorage.GetItemAsync<DateTime?>(CsrfTokenExpiryKey);
                if (!expiryTime.HasValue)
                {
                    return false;
                }

                // Verificar si el token ha expirado
                if (DateTime.UtcNow > expiryTime.Value)
                {
                    // Limpiar token expirado
                    await ClearCsrfTokenAsync();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                // En caso de error, considerar el token como inválido
                Console.WriteLine($"Error al verificar token CSRF: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Clase interna para deserializar la respuesta del token CSRF.
        /// </summary>
        private class CsrfTokenResponse
        {
            public string? csrfToken { get; set; }
        }
    }
}
