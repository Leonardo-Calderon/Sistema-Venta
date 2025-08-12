using Shared.DTOs;

namespace SistemaVenta.Web.Client.Services.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio de gestión de usuarios en la aplicación cliente.
    /// </summary>
    /// <remarks>
    /// Esta interfaz define los métodos necesarios para manejar las operaciones CRUD
    /// de usuarios en la aplicación Blazor WebAssembly. Proporciona funcionalidad
    /// para listar, crear, editar y eliminar usuarios del sistema.
    /// 
    /// La implementación de esta interfaz se encarga de:
    /// - Comunicación con la API de usuarios
    /// - Manejo de respuestas HTTP
    /// - Validación de datos antes del envío
    /// - Gestión de errores de comunicación
    /// </remarks>
    public interface IUsuarioService
    {
        /// <summary>
        /// Obtiene una lista de usuarios con opción de búsqueda.
        /// </summary>
        /// <param name="buscar">Término de búsqueda opcional para filtrar usuarios por nombre.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. El resultado contiene
        /// la lista de usuarios que coinciden con el criterio de búsqueda.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición GET a la API de usuarios con el término
        /// de búsqueda opcional. Si no se proporciona término de búsqueda, retorna
        /// todos los usuarios del sistema.
        /// </remarks>
        Task<List<UsuarioDTO>> Lista(string buscar);

        /// <summary>
        /// Crea un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="usuario">Datos del usuario a crear.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. El resultado contiene
        /// la respuesta HTTP de la operación de creación.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición POST a la API de usuarios con los datos
        /// del nuevo usuario. La respuesta HTTP indica si la operación fue exitosa
        /// o si hubo errores de validación.
        /// </remarks>
        Task<HttpResponseMessage> Crear(UsuarioCrearDTO usuario);

        /// <summary>
        /// Edita un usuario existente en el sistema.
        /// </summary>
        /// <param name="usuario">Datos actualizados del usuario.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. El resultado contiene
        /// la respuesta HTTP de la operación de edición.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición PUT a la API de usuarios con los datos
        /// actualizados del usuario. La respuesta HTTP indica si la operación fue
        /// exitosa o si hubo errores de validación.
        /// </remarks>
        Task<HttpResponseMessage> Editar(UsuarioDTO usuario);

        /// <summary>
        /// Elimina un usuario del sistema.
        /// </summary>
        /// <param name="id">ID del usuario a eliminar.</param>
        /// <returns>
        /// Una tarea que representa la operación asíncrona. El resultado contiene
        /// la respuesta HTTP de la operación de eliminación.
        /// </returns>
        /// <remarks>
        /// Este método envía una petición DELETE a la API de usuarios con el ID
        /// del usuario a eliminar. La respuesta HTTP indica si la operación fue
        /// exitosa o si hubo errores.
        /// </remarks>
        Task<HttpResponseMessage> Eliminar(int id);
    }
}