namespace SVRepository.Entities
{
    /// <summary>
    /// Representa una categoría de productos en el sistema de ventas.
    /// </summary>
    /// <remarks>
    /// Las categorías se utilizan para organizar y clasificar los productos del sistema.
    /// Cada categoría está asociada a una unidad de medida específica que define
    /// cómo se venden los productos de esa categoría.
    /// </remarks>
    public class Categoria
    {
        /// <summary>
        /// Identificador único de la categoría.
        /// </summary>
        /// <remarks>
        /// Este valor es generado automáticamente por la base de datos
        /// y se utiliza como clave primaria en la tabla de categorías.
        /// </remarks>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Nombre descriptivo de la categoría.
        /// </summary>
        /// <remarks>
        /// Este campo es obligatorio y debe ser único en el sistema.
        /// Se utiliza para identificar y mostrar la categoría en la interfaz de usuario.
        /// </remarks>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Unidad de medida asociada a esta categoría.
        /// </summary>
        /// <remarks>
        /// Define la unidad de medida que se utilizará para los productos
        /// de esta categoría (ej: kilogramos, litros, unidades, etc.).
        /// </remarks>
        public Medida RefMedida { get; set; } = new Medida();

        /// <summary>
        /// Indica si la categoría está activa en el sistema.
        /// </summary>
        /// <remarks>
        /// Valores posibles:
        /// - 1: Categoría activa (visible y utilizable)
        /// - 0: Categoría inactiva (oculta y no utilizable)
        /// </remarks>
        public int Activo { get; set; }
    }
}
