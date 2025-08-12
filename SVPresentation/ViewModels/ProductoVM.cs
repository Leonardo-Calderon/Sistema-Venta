using System.ComponentModel;

namespace SVPresentation.ViewModels
{
    /// <summary>
    /// ViewModel para la gestión de productos en la interfaz de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase representa los datos de un producto que se muestran en la interfaz de usuario.
    /// Utiliza Data Annotations para definir cómo se muestran las propiedades en los controles
    /// de la interfaz, especialmente en DataGridViews y formularios de gestión de productos.
    /// </remarks>
    public class ProductoVM
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        /// <remarks>
        /// Este es el identificador único del producto en el sistema.
        /// </remarks>
        public int IdProducto { get; set; }

        /// <summary>
        /// Código único del producto.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza como identificador único del producto para
        /// búsquedas y referencias en el sistema.
        /// </remarks>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción o nombre del producto.
        /// </summary>
        /// <remarks>
        /// Este valor describe el producto y se muestra en la interfaz de usuario.
        /// </remarks>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de la categoría a la que pertenece el producto.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para clasificar y organizar los productos.
        /// </remarks>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Nombre de la categoría a la que pertenece el producto.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario para identificar
        /// la categoría del producto de manera legible.
        /// </remarks>
        public string Categoria { get; set; } = string.Empty;

        /// <summary>
        /// Precio al que se compró el producto originalmente.
        /// </summary>
        /// <remarks>
        /// Esta propiedad se muestra como "Precio Compra" en la interfaz de usuario
        /// gracias al atributo DisplayName. Este valor representa el costo de
        /// adquisición del producto para el negocio.
        /// </remarks>
        [DisplayName("Precio Compra")]
        public string PrecioCompra { get; set; } = string.Empty;

        /// <summary>
        /// Precio al que se vende el producto al cliente.
        /// </summary>
        /// <remarks>
        /// Esta propiedad se muestra como "Precio Venta" en la interfaz de usuario
        /// gracias al atributo DisplayName. Este valor representa el precio de
        /// venta del producto.
        /// </remarks>
        [DisplayName("Precio Venta")]
        public string PrecioVenta { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de unidades disponibles en inventario.
        /// </summary>
        /// <remarks>
        /// Este valor indica cuántas unidades del producto están disponibles
        /// para la venta.
        /// </remarks>
        public int Cantidad { get; set; }

        /// <summary>
        /// Indica si el producto está activo en el sistema (1 = activo, 0 = inactivo).
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para controlar si el producto puede ser vendido.
        /// </remarks>
        public int Activo { get; set; }

        /// <summary>
        /// Representación textual del estado de habilitación del producto.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario para indicar si el producto
        /// está habilitado o deshabilitado de manera legible.
        /// </remarks>
        public string Habilitado { get; set; } = string.Empty;
    }
}
