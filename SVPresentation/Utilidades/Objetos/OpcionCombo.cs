namespace SVPresentation.Utilidades.Objetos
{
    /// <summary>
    /// Clase que representa una opción para un control ComboBox.
    /// </summary>
    /// <remarks>
    /// Esta clase se utiliza para poblar controles ComboBox con opciones que tienen
    /// un texto visible para el usuario y un valor asociado para el procesamiento.
    /// Es especialmente útil para mostrar datos como roles, categorías, etc.
    /// </remarks>
    public class OpcionCombo
    {
        /// <summary>
        /// Texto que se muestra al usuario en el ComboBox.
        /// </summary>
        /// <remarks>
        /// Este es el texto visible que el usuario verá en la lista desplegable
        /// del ComboBox.
        /// </remarks>
        public string Texto { get; set; } = string.Empty;

        /// <summary>
        /// Valor asociado a la opción para el procesamiento interno.
        /// </summary>
        /// <remarks>
        /// Este valor se utiliza internamente para identificar la opción seleccionada
        /// y realizar operaciones como guardar en base de datos o validaciones.
        /// </remarks>
        public int Valor { get; set; }
    }
}
