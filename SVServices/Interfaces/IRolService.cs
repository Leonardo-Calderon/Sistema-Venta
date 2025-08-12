
using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de roles de usuario.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de roles, proporcionando
    /// una capa de abstracción entre los controladores y los repositorios. Los roles definen
    /// los permisos y accesos que tiene cada usuario en el sistema.
    /// </remarks>
    public interface IRolService
    {
        /// <summary>
        /// Obtiene una lista de todos los roles disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de roles.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<List<Rol>> Lista();
    }
}
