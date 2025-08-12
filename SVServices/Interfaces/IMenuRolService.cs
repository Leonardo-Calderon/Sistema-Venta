

using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de menús por rol.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de menús asociados a roles específicos,
    /// proporcionando una capa de abstracción entre los controladores y los repositorios.
    /// Esta funcionalidad es esencial para el control de acceso basado en roles (RBAC).
    /// </remarks>
    public interface IMenuRolService
    {
        /// <summary>
        /// Obtiene una lista de menús disponibles para un rol específico.
        /// </summary>
        /// <param name="idRol">Identificador único del rol para el cual se obtienen los menús.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de menús disponibles para el rol.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// Los menús retornados están filtrados según los permisos del rol especificado.
        /// </remarks>
        Task<List<MenuRol>> Lista(int idRol);
    }
}
