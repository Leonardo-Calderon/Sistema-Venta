

using System.ComponentModel;

namespace SVPresentation.ViewModels
{
    /// <summary>
    /// ViewModel para la gestión de detalles de venta en la interfaz de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase representa los datos de un detalle específico de una venta que se muestran
    /// en la interfaz de usuario. Incluye información del producto vendido, cantidad,
    /// precio y total para ese producto específico.
    /// </remarks>
    public class DetalleVentaVM
    {
        /// <summary>
        /// Identificador único del producto vendido.
        /// </summary>
        /// <remarks>
        /// Este es el identificador único del producto en el sistema.
        /// </remarks>
        public int IdProducto { get; set; }

        /// <summary>
        /// Nombre del producto vendido.
        /// </summary>
        /// <remarks>
        /// Este valor identifica el producto específico que fue vendido.
        /// </remarks>
        public string Producto { get; set; } = string.Empty;

        /// <summary>
        /// Precio unitario del producto al momento de la venta.
        /// </summary>
        /// <remarks>
        /// Este valor representa el precio por unidad del producto vendido.
        /// </remarks>
        public decimal Precio { get; set; }

        /// <summary>
        /// Cantidad de unidades del producto vendidas como valor numérico.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para cálculos y operaciones matemáticas.
        /// </remarks>
        public int CantidadValor { get; set; }

        /// <summary>
        /// Cantidad de unidades del producto vendidas como texto.
        /// </summary>
        /// <remarks>
        /// Esta propiedad se muestra como "Cantidad" en la interfaz de usuario
        /// gracias al atributo DisplayName. Se utiliza para mostrar la cantidad
        /// de manera legible al usuario.
        /// </remarks>
        [DisplayName("Cantidad")]
        public string CantidadTexto { get; set; } = string.Empty;

        /// <summary>
        /// Precio total para este producto en la venta.
        /// </summary>
        /// <remarks>
        /// Este valor se calcula multiplicando el precio unitario por la cantidad vendida.
        /// </remarks>
        public decimal Total { get; set; }
    }
}
