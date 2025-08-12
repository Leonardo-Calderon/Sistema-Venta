
using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de unidades de medida.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IMedidaService y proporciona la lógica de negocio
    /// para la gestión de unidades de medida. Actúa como una capa de abstracción entre
    /// los controladores y los repositorios, permitiendo agregar validaciones y lógica
    /// de negocio adicional.
    /// </remarks>
    public class MedidaService : IMedidaService
    {
        /// <summary>
        /// Instancia del repositorio de medidas para acceder a los datos.
        /// </summary>
        private readonly IMedidaRepository _medidaRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase MedidaService.
        /// </summary>
        /// <param name="medidaRepository">Instancia del repositorio de medidas.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de medidas que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public MedidaService(IMedidaRepository medidaRepository)
        {
            _medidaRepository = medidaRepository ?? throw new ArgumentNullException(nameof(medidaRepository));
        }

        /// <summary>
        /// Obtiene una lista de todas las unidades de medida disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de unidades de medida.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<List<Medida>> Lista()
        {
            return await _medidaRepository.Lista();
        }
    }
}
