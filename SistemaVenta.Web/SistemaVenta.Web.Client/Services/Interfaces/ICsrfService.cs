namespace SistemaVenta.Web.Client.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de tokens CSRF en la aplicación cliente.
    /// </summary>
    /// <remarks>
    /// Esta interfaz define las operaciones necesarias para manejar tokens CSRF
    /// en la aplicación Blazor WebAssembly, incluyendo la obtención y gestión
    /// de tokens para proteger las operaciones que modifican estado.
    /// 
    /// Responsabilidades:
    /// - Obtener tokens CSRF de la API
    /// - Almacenar tokens CSRF localmente
    /// - Proporcionar tokens para requests HTTP
    /// - Manejar renovación de tokens CSRF
    /// - Validar tokens CSRF antes de su uso
    /// </remarks>
    public interface ICsrfService
    {
        /// <summary>
        /// Obtiene un token CSRF válido de la API.
        /// </summary>
        /// <returns>
        /// El token CSRF si se obtiene exitosamente.
        /// </returns>
        /// <exception cref="HttpRequestException">Se lanza cuando la petición HTTP falla.</exception>
        /// <exception cref="Exception">Se lanza cuando no se recibe un token válido.</exception>
        /// <remarks>
        /// Este método:
        /// 1. Realiza una petición GET a la API para obtener el token CSRF
        /// 2. Verifica que la respuesta sea exitosa
        /// 3. Extrae el token CSRF de la respuesta
        /// 4. Almacena el token localmente para uso posterior
        /// 5. Retorna el token CSRF
        /// 
        /// El token CSRF se obtiene de la API y se almacena localmente
        /// para ser usado en operaciones que modifican estado.
        /// </remarks>
        Task<string> GetCsrfTokenAsync();

        /// <summary>
        /// Obtiene el token CSRF actual, renovándolo si es necesario.
        /// </summary>
        /// <returns>
        /// El token CSRF actual o un nuevo token si el actual no es válido.
        /// </returns>
        /// <remarks>
        /// Este método verifica si existe un token CSRF válido localmente.
        /// Si no existe o ha expirado, obtiene un nuevo token de la API.
        /// 
        /// Es el método principal que deben usar los servicios para obtener
        /// tokens CSRF antes de realizar operaciones que modifican estado.
        /// </remarks>
        Task<string> GetCurrentCsrfTokenAsync();

        /// <summary>
        /// Limpia el token CSRF almacenado localmente.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método elimina el token CSRF del almacenamiento local.
        /// Se debe llamar cuando el usuario cierra sesión o cuando
        /// se detecta que el token ha expirado.
        /// </remarks>
        Task ClearCsrfTokenAsync();

        /// <summary>
        /// Verifica si el token CSRF actual es válido.
        /// </summary>
        /// <returns>
        /// True si el token CSRF es válido, false en caso contrario.
        /// </returns>
        /// <remarks>
        /// Este método verifica si existe un token CSRF válido localmente
        /// sin realizar una petición a la API. Útil para verificaciones
        /// rápidas antes de realizar operaciones.
        /// </remarks>
        Task<bool> IsCsrfTokenValidAsync();
    }
}
