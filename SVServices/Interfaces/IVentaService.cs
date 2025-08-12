
using SVRepository.Entities;

namespace SVServices.Interfaces
{
    /// <summary>
    /// Interfaz que define las operaciones de servicio para la gestión de ventas del sistema.
    /// </summary>
    /// <remarks>
    /// Esta interfaz implementa el patrón Service para la gestión de ventas, proporcionando
    /// una capa de abstracción entre los controladores y los repositorios. Las ventas son
    /// el núcleo del sistema de gestión comercial e incluyen funcionalidades para registro
    /// de ventas, consulta de detalles y generación de reportes.
    /// </remarks>
    public interface IVentaService
    {
        /// <summary>
        /// Registra una nueva venta en el sistema a partir de un XML que contiene la información de la venta.
        /// </summary>
        /// <param name="ventaXML">Cadena XML que contiene la información completa de la venta y sus detalles.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el número de venta generado o un mensaje de error.</returns>
        /// <remarks>
        /// Este método puede incluir validaciones de negocio antes de registrar la venta,
        /// como verificar stock disponible, validar precios, etc.
        /// </remarks>
        Task<string> Registrar(string ventaXML);

        /// <summary>
        /// Obtiene la información completa de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es la venta encontrada o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<Venta> Obtener(string numeroVenta);

        /// <summary>
        /// Obtiene los detalles de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta cuyos detalles se quieren obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de la venta.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
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
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como validaciones o transformaciones de datos.
        /// </remarks>
        Task<List<Venta>> Lista(string fechaInicio, string fechaFin, string buscar = "");

        /// <summary>
        /// Genera un reporte de ventas para un rango de fechas específico.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del reporte en formato string.</param>
        /// <param name="fechaFin">Fecha de fin del reporte en formato string.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de ventas para el reporte.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente y puede incluir
        /// lógica adicional de negocio como cálculos de totales, filtros adicionales, etc.
        /// </remarks>
        Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin);
    }
}
