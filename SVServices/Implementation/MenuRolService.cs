

using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de menús por rol.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IMenuRolService y proporciona la lógica de negocio
    /// para la gestión de menús asociados a roles específicos. Actúa como una capa de abstracción
    /// entre los controladores y los repositorios, permitiendo agregar validaciones y lógica
    /// de negocio adicional. Esta funcionalidad es esencial para el control de acceso basado en roles (RBAC).
    /// </remarks>
    public class MenuRolService : IMenuRolService
    {
        /// <summary>
        /// Instancia del repositorio de menús por rol para acceder a los datos.
        /// </summary>
        private readonly IMenuRolRepository _menuRolRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase MenuRolService.
        /// </summary>
        /// <param name="menuRolRepository">Instancia del repositorio de menús por rol.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de menús por rol que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public MenuRolService(IMenuRolRepository menuRolRepository)
        {
            _menuRolRepository = menuRolRepository ?? throw new ArgumentNullException(nameof(menuRolRepository));
        }

        /// <summary>
        /// Obtiene una lista de menús disponibles para un rol específico.
        /// </summary>
        /// <param name="idRol">Identificador único del rol para el cual se obtienen los menús.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de menús disponibles para el rol.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica,
        /// como verificar que el rol existe o aplicar filtros adicionales.
        /// </remarks>
        public async Task<List<MenuRol>> Lista(int idRol)
        {
            return await _menuRolRepository.Lista(idRol);
        }
    }
}
