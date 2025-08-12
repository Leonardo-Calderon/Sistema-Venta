namespace SVPresentation.ViewModels
{
    /// <summary>
    /// ViewModel para la gestión de categorías en la interfaz de usuario.
    /// </summary>
    /// <remarks>
    /// Esta clase representa los datos de una categoría que se muestran en la interfaz de usuario.
    /// Incluye información sobre la categoría y su unidad de medida asociada, permitiendo
    /// la gestión y visualización de categorías de productos en el sistema.
    /// </remarks>
    public class CategoriaVM
    {
        /// <summary>
        /// Identificador único de la categoría.
        /// </summary>
        /// <remarks>
        /// Este es el identificador único de la categoría en el sistema.
        /// </remarks>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        /// <remarks>
        /// Este valor describe la categoría y se muestra en la interfaz de usuario.
        /// </remarks>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Identificador de la unidad de medida asociada a la categoría.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para determinar la unidad de medida que se aplica
        /// a los productos de esta categoría.
        /// </remarks>
        public int IdMedida { get; set; }

        /// <summary>
        /// Nombre de la unidad de medida asociada a la categoría.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario para identificar
        /// la unidad de medida de manera legible.
        /// </remarks>
        public string Medida { get; set; } = string.Empty;

        /// <summary>
        /// Indica si la categoría está activa en el sistema (1 = activa, 0 = inactiva).
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza para controlar si la categoría puede ser utilizada
        /// para clasificar productos.
        /// </remarks>
        public int Activo { get; set; }

        /// <summary>
        /// Representación textual del estado de habilitación de la categoría.
        /// </summary>
        /// <remarks>
        /// Este valor se muestra en la interfaz de usuario para indicar si la categoría
        /// está habilitada o deshabilitada de manera legible.
        /// </remarks>
        public string Habilitado { get; set; } = string.Empty;
    }
}
