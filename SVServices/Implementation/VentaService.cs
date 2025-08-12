
using SVRepository.Entities;
using SVRepository.Interfaces;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para la gestión de ventas del sistema.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz IVentaService y proporciona la lógica de negocio
    /// para la gestión de ventas. Actúa como una capa de abstracción entre los controladores
    /// y los repositorios, permitiendo agregar validaciones y lógica de negocio adicional.
    /// Las ventas son el núcleo del sistema de gestión comercial e incluyen funcionalidades
    /// para registro de ventas, consulta de detalles y generación de reportes.
    /// </remarks>
    public class VentaService : IVentaService
    {
        /// <summary>
        /// Instancia del repositorio de ventas para acceder a los datos.
        /// </summary>
        private readonly IVentaRepository _ventaRepository;

        /// <summary>
        /// Inicializa una nueva instancia de la clase VentaService.
        /// </summary>
        /// <param name="ventaRepository">Instancia del repositorio de ventas.</param>
        /// <remarks>
        /// El constructor recibe una instancia del repositorio de ventas que será utilizada
        /// para realizar las operaciones de base de datos.
        /// </remarks>
        public VentaService(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository ?? throw new ArgumentNullException(nameof(ventaRepository));
        }

        /// <summary>
        /// Registra una nueva venta en el sistema a partir de un XML que contiene la información de la venta.
        /// </summary>
        /// <param name="ventaXML">Cadena XML que contiene la información completa de la venta y sus detalles.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es el número de venta generado o un mensaje de error.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones de negocio como verificar stock disponible, validar precios, etc.
        /// </remarks>
        public async Task<string> Registrar(string ventaXML)
        {
            if (string.IsNullOrWhiteSpace(ventaXML))
                throw new ArgumentException("El XML de la venta no puede estar vacío.", nameof(ventaXML));

            return await _ventaRepository.Registrar(ventaXML);
        }

        /// <summary>
        /// Obtiene la información completa de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta a obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es la venta encontrada o un objeto vacío si no existe.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<Venta> Obtener(string numeroVenta)
        {
            if (string.IsNullOrWhiteSpace(numeroVenta))
                throw new ArgumentException("El número de venta no puede estar vacío.", nameof(numeroVenta));

            return await _ventaRepository.Obtener(numeroVenta);
        }

        /// <summary>
        /// Obtiene los detalles de una venta específica por su número de venta.
        /// </summary>
        /// <param name="numeroVenta">Número único de la venta cuyos detalles se quieren obtener.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de la venta.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<List<DetalleVenta>> ObtenerDetalle(string numeroVenta)
        {
            if (string.IsNullOrWhiteSpace(numeroVenta))
                throw new ArgumentException("El número de venta no puede estar vacío.", nameof(numeroVenta));

            return await _ventaRepository.ObtenerDetalle(numeroVenta);
        }

        /// <summary>
        /// Obtiene una lista de ventas filtrada por rango de fechas y opcionalmente por término de búsqueda.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del rango de búsqueda en formato string.</param>
        /// <param name="fechaFin">Fecha de fin del rango de búsqueda en formato string.</param>
        /// <param name="buscar">Término opcional para filtrar las ventas por número de venta o nombre de cliente.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de ventas que cumplen con los criterios.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar validaciones adicionales o lógica de negocio específica.
        /// </remarks>
        public async Task<List<Venta>> Lista(string fechaInicio, string fechaFin, string buscar = "")
        {
            return await _ventaRepository.Lista(fechaInicio, fechaFin, buscar ?? string.Empty);
        }

        /// <summary>
        /// Genera un reporte de ventas para un rango de fechas específico.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del reporte en formato string.</param>
        /// <param name="fechaFin">Fecha de fin del reporte en formato string.</param>
        /// <returns>Una tarea que representa la operación asíncrona. El resultado es una lista de detalles de ventas para el reporte.</returns>
        /// <remarks>
        /// Este método delega la operación al repositorio correspondiente. En el futuro,
        /// se pueden agregar lógica adicional de negocio como cálculos de totales, filtros adicionales, etc.
        /// </remarks>
        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            return await _ventaRepository.Reporte(fechaInicio, fechaFin);
        }
    }
}
