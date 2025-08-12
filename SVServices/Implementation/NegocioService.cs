
using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de información del negocio.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz INegocioService y proporciona la lógica de negocio
    /// para la gestión de información del negocio. Actúa como una capa de abstracción entre
    /// los controladores y los repositorios, permitiendo agregar validaciones y lógica
    /// de negocio adicional. La información del negocio incluye datos fiscales, de contacto
    /// y configuración del sistema.
    /// </remarks>
    public class NegocioService : INegocioService
    {
        /// <summary>
        /// Instancia del repositorio de negocio para acceder a los datos.
        /// </summary>
        private readonly INegocioRepository _negocioRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase NegocioService.
        /// </summary>
        /// <param name="negocioRepository">Instancia del repositorio de negocio.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de negocio que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public NegocioService(INegocioRepository negocioRepository)
        {
            _negocioRepository = negocioRepository ?? throw new ArgumentNullException(nameof(negocioRepository));
        }

        /// <summary>
        /// Obtiene la información de configuración del negocio.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es un objeto Negocio con la información del negocio.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<Negocio> Obtener()
        {
            return await _negocioRepository.Obtener();
        }

        /// <summary>
        /// Actualiza la información de configuración del negocio.
        /// </summary>
        /// <param name="objeto">Objeto Negocio con los datos actualizados del negocio.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio antes de actualizar, como verificar
        /// que los datos son válidos y cumplen con las reglas de negocio.
        /// </remarks>
        public async Task Editar(Negocio objeto)
        {
            if (objeto == null)
                throw new ArgumentNullException(nameof(objeto));

            await _negocioRepository.Editar(objeto);
        }
    }
}
