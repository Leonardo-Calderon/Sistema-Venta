namespace SVRepository.Entities
{
    /// <summary>
    /// Representa un producto en el sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Los productos son los elementos que se venden en el sistema. Cada producto
    /// tiene información sobre su categoría, precios, stock disponible y estado
    /// de activación en el sistema.
    /// </remarks>
    public class Producto
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de productos.
        /// </remarks>
        public int IdProducto { get; set; }

        /// <summary>
        /// Categoría a la que pertenece el producto.
        /// </summary>
        /// <remarks>
        /// Define la clasificación del producto y determina la unidad de medida
        /// que se utilizará para su venta (ej: kilogramos, litros, piezas, etc.).
        /// </remarks>
        public Categoria RefCategoria { get; set; } = new Categoria();

        /// <summary>
        /// Código único del producto.
        /// </summary>
        /// <remarks>
        /// Código alfanumérico que identifica de forma única al producto.
        /// Se utiliza para búsquedas rápidas y referencia en inventarios.
        /// </remarks>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción o nombre del producto.
        /// </summary>
        /// <remarks>
        /// Nombre descriptivo del producto que se mostrará en la interfaz
        /// de usuario y en los documentos de venta.
        /// </remarks>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Precio de compra del producto.
        /// </summary>
        /// <remarks>
        /// Precio al que se adquirió el producto del proveedor.
        /// Se utiliza para cálculos de ganancia y análisis de rentabilidad.
        /// </remarks>
        public decimal PrecioCompra { get; set; }

        /// <summary>
        /// Precio de venta del producto.
        /// </summary>
        /// <remarks>
        /// Precio al que se vende el producto al cliente.
        /// Este es el precio que se utiliza en las transacciones de venta.
        /// </remarks>
        public decimal PrecioVenta { get; set; }

        /// <summary>
        /// Cantidad disponible en inventario.
        /// </summary>
        /// <remarks>
        /// Número de unidades disponibles del producto para la venta.
        /// Se actualiza automáticamente con cada venta realizada.
        /// </remarks>
        public int Cantidad { get; set; }

        /// <summary>
        /// Indica si el producto está activo en el sistema.
        /// </summary>
        /// <remarks>
        /// Valores posibles:
        /// - 1: Producto activo (disponible para venta)
        /// - 0: Producto inactivo (no disponible para venta)
        /// </remarks>
        public int Activo { get; set; }
    }
}
