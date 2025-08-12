

using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad MenuRol.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de menús y permisos por rol,
    /// proporcionando métodos para obtener los menús disponibles para un rol específico.
    /// Esta funcionalidad es esencial para el control de acceso y la navegación del sistema.
    /// </remarks>
    public interface IMenuRolRepository
    {
        /// <summary>
        /// Obtiene una lista de menús disponibles para un rol específico.
        /// </summary>
        /// <param name="idRol">Identificador único del rol para el cual se obtienen los menús.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de menús disponibles para el rol.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todos los menús que están
        /// habilitados para el rol especificado, incluyendo información sobre menús padre
        /// y el estado activo de cada menú.
        /// </remarks>
        Task<List<MenuRol>> Lista(int idRol);
    }
}
