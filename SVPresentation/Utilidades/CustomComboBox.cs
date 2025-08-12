
using SVPresentation.Utilidades.Objetos;

namespace SVPresentation.Utilidades
{
    /// <summary>
    /// Clase estática que proporciona métodos de extensión para personalizar controles ComboBox.
    /// </summary>
    /// <remarks>
    /// Esta clase contiene métodos de extensión que agregan funcionalidad adicional a los
    /// controles ComboBox, como la inserción de opciones y la selección de valores específicos.
    /// </remarks>
    public static class CustomComboBox
    {
        /// <summary>
        /// Inserta un array de opciones en un ComboBox y configura sus propiedades de visualización.
        /// </summary>
        /// <param name="combo">El ComboBox al que se insertarán las opciones.</param>
        /// <param name="items">Array de opciones OpcionCombo que se insertarán en el ComboBox.</param>
        /// <remarks>
        /// Este método de extensión:
        /// - Agrega todas las opciones del array al ComboBox
        /// - Configura "Texto" como la propiedad que se muestra al usuario
        /// - Configura "Valor" como la propiedad que se utiliza internamente
        /// - Selecciona automáticamente la primera opción
        /// 
        /// Es útil para poblar ComboBox con datos de categorías, roles, etc.
        /// </remarks>
        public static void InsertarItems(this ComboBox combo, OpcionCombo[] items)
        {
            combo.Items.AddRange(items);
            combo.DisplayMember = "Texto";
            combo.ValueMember = "Valor";
            combo.SelectedIndex = 0;
        }

        /// <summary>
        /// Establece la opción seleccionada en un ComboBox basándose en un valor específico.
        /// </summary>
        /// <param name="combo">El ComboBox en el que se establecerá la selección.</param>
        /// <param name="valor">El valor de la opción que se desea seleccionar.</param>
        /// <remarks>
        /// Este método de extensión busca en todas las opciones del ComboBox la que tenga
        /// el valor especificado y la selecciona. Si no encuentra ninguna opción con ese
        /// valor, no se realiza ninguna selección.
        /// 
        /// Es útil para establecer valores por defecto o restaurar selecciones previas.
        /// </remarks>
        public static void EstablecerValor(this ComboBox combo, int valor)
        {
            foreach (OpcionCombo opcion in combo.Items)
            {
                if (opcion.Valor == valor)
                {
                    combo.SelectedItem = opcion;
                    break;
                }
            }
        }
    }
}