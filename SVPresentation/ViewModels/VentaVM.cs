using System.ComponentModel;

namespace SVPresentation.ViewModels
{
    /// <summary>
    /// ViewModel para la gestión de ventas en la interfaz de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase representa los datos de una venta que se muestran en la interfaz de usuario.
    /// Utiliza Data Annotations para definir cómo se muestran las propiedades en los controles
    /// de la interfaz, especialmente en DataGridViews.
    /// </remarks>
    public class VentaVM
    {
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
        /// Número único que identifica la venta.
        /// </summary>
        /// <remarks>
        /// Este es el identificador único de la venta que se genera automáticamente
        /// al momento de crear la venta.
        /// </remarks>
        public string NumeroVenta { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario que realizó la venta.
        /// </summary>
        /// <remarks>
        /// Este valor indica qué usuario del sistema procesó la venta.
        /// </remarks>
        public string Usuario { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del cliente para quien se realizó la venta.
        /// </summary>
        /// <remarks>
        /// Este valor identifica al cliente que compró los productos.
        /// </remarks>
        public string Cliente { get; set; } = string.Empty;

        /// <summary>
        /// Monto total de la venta.
        /// </summary>
        /// <remarks>
        /// Este valor representa la suma total de todos los productos vendidos
        /// en esta transacción.
        /// </remarks>
        public string Total { get; set; } = string.Empty;
    }
}
