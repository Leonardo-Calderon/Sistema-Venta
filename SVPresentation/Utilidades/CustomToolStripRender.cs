

namespace SVPresentation.Utilidades
{
    public class CustomToolStripRender : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                Color hoverColor = Color.FromArgb(56, 126, 209);
                e.Graphics.FillRectangle(new SolidBrush(hoverColor), e.Item.ContentRectangle);
                e.Item.ForeColor = Color.White;
            }
            else
            {
                Color hoverColor = Color.FromArgb(44, 110, 203);
                e.Graphics.FillRectangle(new SolidBrush(hoverColor), e.Item.ContentRectangle);
                e.Item.ForeColor = Color.White;
            }
        }
    }
}
