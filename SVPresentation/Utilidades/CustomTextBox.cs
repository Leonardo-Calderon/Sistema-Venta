namespace SVPresentation.Utilidades
{
    /// <summary>
    /// Clase estática que proporciona métodos de extensión para personalizar controles TextBox.
    /// </summary>
    /// <remarks>
    /// Esta clase contiene métodos de extensión que agregan funcionalidad adicional a los
    /// controles TextBox, como validación de entrada y restricciones de caracteres.
    /// </remarks>
    public static class CustomTextBox
    {
        /// <summary>
        /// Agrega validación para permitir solo números y un punto decimal en un TextBox.
        /// </summary>
        /// <param name="textBox">El TextBox al que se aplicará la validación.</param>
        /// <remarks>
        /// Este método de extensión agrega un evento KeyPress al TextBox que:
        /// - Permite solo dígitos (0-9)
        /// - Permite un solo punto decimal
        /// - Permite teclas de control (backspace, delete, etc.)
        /// - Bloquea cualquier otro carácter
        /// 
        /// Es útil para campos que deben contener solo valores numéricos como precios,
        /// cantidades, etc.
        /// </remarks>
        public static void ValidarNumero(this TextBox textBox)
        {
            textBox.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' ||
                    e.KeyChar == '.' && textBox.Text.Contains("."))
                {
                    e.Handled = true;
                }
            };
        }
    }
}
