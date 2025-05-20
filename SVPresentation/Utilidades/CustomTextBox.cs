namespace SVPresentation.Utilidades
{
    public static class CustomTextBox
    {
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
