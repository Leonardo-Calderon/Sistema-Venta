using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de unidades de medida.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de unidades de medida,
    /// proporcionando una capa de abstracción entre los controladores y los repositorios.
    /// Las unidades de medida definen cómo se cuantifican los productos en el sistema.
    /// </remarks>
    public interface IMedidaService
    {
        /// <summary>
        /// Obtiene una lista de todas las unidades de medida disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de unidades de medida.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<List<Medida>> Lista();
    }
}
