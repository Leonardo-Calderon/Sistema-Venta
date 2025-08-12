
using System.ComponentModel;

namespace SVPresentation.ViewModels
{
    /// <summary>
    /// ViewModel para la generación de reportes de ventas en la interfaz de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase representa los datos detallados de ventas que se utilizan para generar
    /// reportes. Incluye información tanto de la venta como de los productos vendidos,
    /// permitiendo análisis detallados de las transacciones comerciales.
    /// </remarks>
    public class ReporteVentaVM
    {
        /// <summary>
        /// Número único que identifica la venta.
        /// </summary>
        /// <remarks>
        /// Este es el identificador único de la venta que se genera automáticamente
        /// al momento de crear la venta.
        /// </remarks>
        [DisplayName("Numero Venta")]
        public string NumeroVenta { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario que realizó la venta.
        /// </summary>
        /// <remarks>
        /// Este valor indica qué usuario del sistema procesó la venta.
        /// </remarks>
        [DisplayName("Nombre Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        /// <summary>
        /// Fecha en que se registró la venta.
        /// </summary>
        /// <remarks>
        /// Esta propiedad se muestra como "Fecha Registro" en la interfaz de usuario
        /// gracias al atributo DisplayName.
        /// </remarks>
        [DisplayName("Fecha Registro")]
        public string FechaRegistro { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del producto vendido.
        /// </summary>
        /// <remarks>
        /// Este valor identifica el producto específico que fue vendido en esta transacción.
        /// </remarks>
        public string Producto { get; set; } = string.Empty;

        /// <summary>
        /// Precio al que se compró el producto originalmente.
        /// </summary>
        /// <remarks>
        /// Este valor representa el costo de adquisición del producto para el negocio.
        /// </remarks>
        [DisplayName("Precio Compra")]
        public decimal PrecioCompra { get; set; }

        /// <summary>
        /// Precio al que se vendió el producto al cliente.
        /// </summary>
        /// <remarks>
        /// Este valor representa el precio de venta unitario del producto.
        /// </remarks>
        [DisplayName("Precio Venta")]
        public decimal PrecioVenta { get; set; }

        /// <summary>
        /// Cantidad de unidades del producto vendidas.
        /// </summary>
        /// <remarks>
        /// Este valor indica cuántas unidades del producto se vendieron en esta transacción.
        /// </remarks>
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio total de la venta para este producto.
        /// </summary>
        /// <remarks>
        /// Este valor se calcula multiplicando el precio de venta por la cantidad vendida.
        /// </remarks>
        [DisplayName("Precio Total")]
        public decimal PrecioTotal { get; set; }
    }
}
