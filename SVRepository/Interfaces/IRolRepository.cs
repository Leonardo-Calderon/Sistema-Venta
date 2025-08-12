
using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad Rol.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de roles de usuario,
    /// proporcionando métodos para obtener los roles disponibles en el sistema.
    /// Los roles definen los permisos y accesos que tiene cada usuario en el sistema.
    /// </remarks>
    public interface IRolRepository
    {
        /// <summary>
        /// Obtiene una lista de todos los roles disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de roles.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todos los roles
        /// configurados en el sistema, incluyendo su identificador y nombre.
        /// </remarks>
        Task<List<Rol>> Lista();
    }
}
