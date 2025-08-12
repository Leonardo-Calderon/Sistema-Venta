using SVRepository.Interfaces;
using SVRepository.Entities;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de roles de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IRolService y proporciona la lógica de negocio
    /// para la gestión de roles. Actúa como una capa de abstracción entre los controladores
    /// y los repositorios, permitiendo agregar validaciones y lógica de negocio adicional.
    /// </remarks>
    public class RolService : IRolService
    {
        /// <summary>
        /// Instancia del repositorio de roles para acceder a los datos.
        /// </summary>
        private readonly IRolRepository _rolRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase RolService.
        /// </summary>
        /// <param name="rolRepository">Instancia del repositorio de roles.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de roles que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public RolService(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository ?? throw new ArgumentNullException(nameof(rolRepository));
        }

        /// <summary>
        /// Obtiene una lista de todos los roles disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de roles.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public Task<List<Rol>> Lista()
        {
            return _rolRepository.Lista();
        }
    }
}
