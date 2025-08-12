using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad Medida.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de unidades de medida,
    /// proporcionando métodos para obtener las unidades de medida disponibles en el sistema.
    /// Las medidas se utilizan para categorizar productos y definir sus unidades de venta.
    /// </remarks>
    public interface IMedidaRepository
    {
        /// <summary>
        /// Obtiene una lista de todas las unidades de medida disponibles en el sistema.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de unidades de medida.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todas las unidades de medida
        /// configuradas en el sistema, incluyendo su abreviatura, nombre, equivalente y valor.
        /// </remarks>
        Task<List<Medida>> Lista();
    }
}
