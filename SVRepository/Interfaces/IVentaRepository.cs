

using SVRepository.Entities;

namespace SVRepository.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para la entidad Venta.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Repository para la gestión de ventas del sistema,
    /// proporcionando métodos para registrar ventas, obtener información de ventas específicas,
    /// listar ventas por fechas y generar reportes de ventas.
    /// Las ventas son el núcleo del sistema de gestión comercial.
    /// </remarks>
    public interface IVentaRepository
    {
        /// <summary>
        /// Registra una nueva venta en el sistema a partir de un XML que contiene la información de la venta.
        /// </summary>
        /// <param name="ventaXML">Cadena XML que contiene la información completa de la venta y sus detalles.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el número de venta generado o un mensaje de error.</returns>
        /// <remarks>
        /// Este método procesa un XML que contiene la información de la venta y sus detalles,
        /// registra la venta en la base de datos y retorna el número de venta generado automáticamente.
        /// </remarks>
        Task<string> Registrar(string ventaXML);

        /// <summary>
        /// Obtiene la información completa de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es la venta encontrada o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que busca una venta específica por su número
        /// y retorna la información completa incluyendo datos del cliente y usuario que registró la venta.
        /// </remarks>
        Task<Venta> Obtener(string numeroVenta);

        /// <summary>
        /// Obtiene los detalles de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta cuyos detalles se quieren obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de la venta.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todos los productos vendidos
        /// en una venta específica, incluyendo cantidades, precios y información del producto.
        /// </remarks>
        Task<List<DetalleVenta>> ObtenerDetalle(string numeroVenta);

        /// <summary>
        /// Obtiene una lista de ventas filtrada por rango de fechas y opcionalmente por término de búsqueda.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del rango de búsqueda en formato string.</param>
        /// <param name="fechaFin">Fecha de fin del rango de búsqueda en formato string.</param>
        /// <param name="buscar">Término opcional para filtrar las ventas por número de venta o nombre de cliente.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de ventas que cumplen con los criterios.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que retorna todas las ventas realizadas
        /// en el rango de fechas especificado, opcionalmente filtradas por el término de búsqueda.
        /// </remarks>
        Task<List<Venta>> Lista(string fechaInicio, string fechaFin, string buscar = "");

        /// <summary>
        /// Genera un reporte de ventas para un rango de fechas específico.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del reporte en formato string.</param>
        /// <param name="fechaFin">Fecha de fin del reporte en formato string.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de ventas para el reporte.</returns>
        /// <remarks>
        /// Este método ejecuta un stored procedure que genera un reporte detallado de todas las ventas
        /// realizadas en el rango de fechas especificado, incluyendo información de productos, precios y ganancias.
        /// </remarks>
        Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin);
    }
}