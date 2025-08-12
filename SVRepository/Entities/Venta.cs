

namespace SVRepository.Entities
{
    /// <summary>
    /// Representa una venta en el sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Una venta es una transacción comercial que incluye uno o más productos vendidos
    /// a un cliente. Cada venta tiene un número único, información del cliente,
    /// totales de pago y una lista de detalles que especifican los productos vendidos.
    /// </remarks>
    public class Venta
    {
        /// <summary>
        /// Identificador único de la venta.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de ventas.
        /// </remarks>
        public int IdVenta { get; set; }

        /// <summary>
        /// Número único de la venta.
        /// </summary>
        /// <remarks>
        /// Número alfanumérico que identifica de forma única la venta.
        /// Se genera automáticamente y se utiliza para referencia en documentos
        /// como facturas, tickets y reportes.
        /// </remarks>
        public string NumeroVenta { get; set; } = string.Empty;

        /// <summary>
        /// Usuario que registró la venta.
        /// </summary>
        /// <remarks>
        /// Información del usuario que procesó la venta en el sistema,
        /// incluyendo su nombre y permisos.
        /// </remarks>
        public Usuario UsuarioRegistrado { get; set; } = new Usuario();

        /// <summary>
        /// Nombre del cliente que realizó la compra.
        /// </summary>
        /// <remarks>
        /// Nombre del cliente para el cual se registra la venta.
        /// Se utiliza para identificación en documentos y reportes.
        /// </remarks>
        public string NombreCliente { get; set; } = string.Empty;

        /// <summary>
        /// Precio total de la venta.
        /// </summary>
        /// <remarks>
        /// Suma total de todos los productos vendidos en esta venta.
        /// Se calcula automáticamente sumando los precios totales de cada detalle.
        /// </remarks>
        public decimal precioTotal { get; set; }

        /// <summary>
        /// Cantidad con la que pagó el cliente.
        /// </summary>
        /// <remarks>
        /// Monto que entregó el cliente para pagar la venta.
        /// Se utiliza para calcular el cambio a devolver.
        /// </remarks>
        public decimal PagoCon { get; set; }

        /// <summary>
        /// Cambio devuelto al cliente.
        /// </summary>
        /// <remarks>
        /// Diferencia entre el monto pagado por el cliente y el total de la venta.
        /// Se calcula automáticamente como PagoCon - precioTotal.
        /// </remarks>
        public decimal Cambio { get; set; }

        /// <summary>
        /// Fecha y hora en que se registró la venta.
        /// </summary>
        /// <remarks>
        /// Timestamp que indica cuándo se procesó la venta en el sistema.
        /// Se utiliza para reportes, auditorías y análisis de ventas por tiempo.
        /// </remarks>
        public string FechaRegistro { get; set; } = string.Empty;

        /// <summary>
        /// Indica si la venta está activa en el sistema.
        /// </summary>
        /// <remarks>
        /// Valores posibles:
        /// - 1: Venta activa (válida y procesada)
        /// - 0: Venta inactiva (cancelada o anulada)
        /// Se utiliza para controlar el estado de las ventas.
        /// </remarks>
        public int Activo { get; set; }

        /// <summary>
        /// Lista de detalles de la venta.
        /// </summary>
        /// <remarks>
        /// Colección que contiene todos los productos vendidos en esta venta,
        /// incluyendo cantidades, precios unitarios y precios totales por producto.
        /// </remarks>
        public List<DetalleVenta> RefDetalleVenta { get; set; } = new List<DetalleVenta>();
    }
}
