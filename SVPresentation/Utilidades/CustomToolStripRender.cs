

namespace SVPresentation.Utilidades
{
    /// <summary>
    /// Renderizador personalizado para controles ToolStrip que proporciona un estilo visual personalizado.
    /// </summary>
    /// <remarks>
    /// Esta clase hereda de ToolStripProfessionalRenderer y personaliza la apariencia visual
    /// de los elementos de menú en los controles ToolStrip. Define colores específicos para
    /// el estado normal y seleccionado de los elementos de menú.
    /// </remarks>
    public class CustomToolStripRender : ToolStripProfessionalRenderer
    {
        /// <summary>
        /// Renderiza el fondo de un elemento de menú con colores personalizados.
        /// </summary>
        /// <param name="e">Argumentos del evento que contienen información sobre el elemento a renderizar.</param>
        /// <remarks>
        /// Este método se ejecuta cada vez que se necesita renderizar el fondo de un elemento
        /// de menú. Aplica diferentes colores según el estado del elemento:
        /// - Estado seleccionado: Color azul claro (#387ED1)
        /// - Estado normal: Color azul más oscuro (#2C6ECB)
        /// 
        /// En ambos casos, el texto se establece en color blanco para mejor contraste.
        /// </remarks>
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                // Color para elemento seleccionado (hover)
                Color hoverColor = Color.FromArgb(56, 126, 209);
                e.Graphics.FillRectangle(new SolidBrush(hoverColor), e.Item.ContentRectangle);
                e.Item.ForeColor = Color.White;
            }
            else
            {
                // Color para elemento normal
                Color hoverColor = Color.FromArgb(44, 110, 203);
                e.Graphics.FillRectangle(new SolidBrush(hoverColor), e.Item.ContentRectangle);
                e.Item.ForeColor = Color.White;
            }
        }
    }
}
