

namespace SVRepository.Entities
{
    /// <summary>
    /// Representa un detalle individual de una venta en el sistema.
    /// </summary>
    /// <remarks>
    /// Cada detalle de venta contiene información sobre un producto específico vendido,
    /// incluyendo la cantidad, precio unitario y precio total. Esta entidad es parte
    /// de una relación uno a muchos con la entidad Venta.
    /// </remarks>
    public class DetalleVenta
    {
        /// <summary>
        /// Identificador único del detalle de venta.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de detalles de venta.
        /// </remarks>
        public int IdDetalleVenta { get; set; }

        /// <summary>
        /// Identificador de la venta a la que pertenece este detalle.
        /// </summary>
        /// <remarks>
        /// Este campo establece la relación con la tabla de ventas
        /// y permite agrupar todos los detalles de una venta específica.
        /// </remarks>
        public int IdVenta { get; set; }

        /// <summary>
        /// Usuario que registró la venta.
        /// </summary>
        /// <remarks>
        /// Contiene información del usuario que procesó la venta,
        /// incluyendo su nombre y permisos en el sistema.
        /// </remarks>
        public Usuario UsuarioRegistrado { get; set; } = new Usuario();

        /// <summary>
        /// Referencia a la venta principal.
        /// </summary>
        /// <remarks>
        /// Proporciona acceso a la información completa de la venta
        /// desde el detalle, incluyendo fecha, cliente y totales.
        /// </remarks>
        public Venta RefVenta { get; set; } = new Venta();

        /// <summary>
        /// Producto vendido en este detalle.
        /// </summary>
        /// <remarks>
        /// Contiene toda la información del producto vendido,
        /// incluyendo código, descripción, categoría y unidad de medida.
        /// </remarks>
        public Producto RefProducto { get; set; } = new Producto();

        /// <summary>
        /// Cantidad del producto vendido.
        /// </summary>
        /// <remarks>
        /// Representa la cantidad vendida del producto específico.
        /// Debe ser mayor que cero y respetar las unidades de medida del producto.
        /// </remarks>
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio unitario del producto al momento de la venta.
        /// </summary>
        /// <remarks>
        /// Este valor se captura al momento de la venta para mantener
        /// un historial preciso de los precios, independientemente de
        /// cambios futuros en el precio del producto.
        /// </remarks>
        public decimal PrecioVenta { get; set; }

        /// <summary>
        /// Precio total del detalle (cantidad * precio unitario).
        /// </summary>
        /// <remarks>
        /// Este valor se calcula automáticamente como el producto
        /// de la cantidad por el precio unitario del producto.
        /// </remarks>
        public decimal PrecioTotal { get; set; }
    }
}
