using Shared.DTOs;
using SistemaVenta.Web.Client.Services.Interfaces;
using System.Net.Http.Json;

namespace SistemaVenta.Web.Client.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de gestión de usuarios para la aplicación cliente.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IUsuarioService y proporciona la funcionalidad
    /// para manejar las operaciones CRUD de usuarios en la aplicación Blazor WebAssembly.
    /// 
    /// Responsabilidades:
    /// - Comunicación con la API de usuarios
    /// - Manejo de respuestas HTTP
    /// - Serialización y deserialización de datos
    /// - Gestión de errores de comunicación
    /// - Validación de respuestas HTTP
    /// </remarks>
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de usuarios.
        /// </summary>
        /// <param name="httpClient">Cliente HTTP para comunicación con la API.</param>
        /// <remarks>
        /// El constructor recibe el cliente HTTP necesario para realizar las
        /// peticiones a la API de usuarios. Este cliente debe estar configurado
        /// con la URL base de la API.
        /// </remarks>
        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Obtiene una lista de usuarios con opción de búsqueda.
        /// </summary>
        /// <param name="buscar">Término de búsqueda opcional para filtrar usuarios por nombre.</param>
        /// <returns>
        /// Una lista de usuarios que coinciden con el criterio de búsqueda.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición GET a la API de usuarios con el término
        /// de búsqueda opcional. Si no se proporciona término de búsqueda, retorna
        /// todos los usuarios del sistema.
        /// 
        /// La respuesta se deserializa automáticamente a una lista de UsuarioDTO.
        /// </remarks>
        public async Task<List<UsuarioDTO>> Lista(string buscar)
        {
            return await _httpClient.GetFromJsonAsync<List<UsuarioDTO>>($"api/usuarios?buscar={buscar}");
        }

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usuario">Datos del usuario a crear.</param>
        /// <returns>
        /// La respuesta HTTP de la operación de creación.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición POST a la API de usuarios con los datos
        /// del nuevo usuario. Los datos se serializan automáticamente a JSON
        /// antes de ser enviados.
        /// 
        /// La respuesta HTTP indica si la operación fue exitosa o si hubo
        /// errores de validación.
        /// </remarks>
        public async Task<HttpResponseMessage> Crear(UsuarioCrearDTO usuario)
        {
            return await _httpClient.PostAsJsonAsync("api/usuarios", usuario);
        }

        /// <summary>
        /// Edita un usuario existente en el sistema.
        /// </summary>
        /// <param name="usuario">Datos actualizados del usuario.</param>
        /// <returns>
        /// La respuesta HTTP de la operación de edición.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición PUT a la API de usuarios con los datos
        /// actualizados del usuario. Los datos se serializan automáticamente a JSON
        /// antes de ser enviados.
        /// 
        /// La respuesta HTTP indica si la operación fue exitosa o si hubo
        /// errores de validación.
        /// </remarks>
        public async Task<HttpResponseMessage> Editar(UsuarioDTO usuario)
        {
            return await _httpClient.PutAsJsonAsync($"api/usuarios/{usuario.IdUsuario}", usuario);
        }

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>
        /// La respuesta HTTP de la operación de eliminación.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición DELETE a la API de usuarios con el ID
        /// del usuario a eliminar.
        /// 
        /// La respuesta HTTP indica si la operación fue exitosa o si hubo errores.
        /// </remarks>
        public async Task<HttpResponseMessage> Eliminar(int id)
        {
            return await _httpClient.DeleteAsync($"api/usuarios/{id}");
        }
    }
}