using System.Drawing;
using System.Windows.Forms;

namespace SVPresentation.Utilidades
{
    public static class CustomDataGridView
    {
        // Colores de Encabezado
        private static readonly Color HeaderNormalColor = Color.FromArgb(44, 110, 203);
        private static readonly Color HeaderHoverColor = Color.FromArgb(30, 90, 180);
        private static readonly Color HeaderTextColor = Color.White;
        private static int HoveredHeaderColumnIndex = -1;

        // Fuentes (creadas una vez para eficiencia)
        private static readonly Font HeaderFont = new Font("Segoe UI SemiBold", 9.5f, FontStyle.Bold);
        private static readonly Font CellFont = new Font("Segoe UI", 9);
        private static readonly Font ButtonFont = new Font("Segoe UI", 9, FontStyle.Bold);

        // Colores de Celdas
        private static readonly Color DefaultCellBackColor = Color.White;
        private static readonly Color DefaultCellForeColor = Color.FromArgb(64, 64, 64);
        private static readonly Color DefaultSelectionBackColor = Color.FromArgb(223, 239, 255);
        private static readonly Color DefaultSelectionForeColor = Color.FromArgb(44, 110, 203);
        private static readonly Color AlternatingRowBackColor = Color.FromArgb(250, 251, 253);
        private static readonly Color GridBackColor = Color.FromArgb(245, 247, 251);
        private static readonly Color GridLinesColor = Color.FromArgb(228, 231, 237);

        // Colores de Botones
        private static readonly Color DeleteButtonBackColor = Color.FromArgb(219, 64, 64);
        private static readonly Color DeleteButtonHoverBackColor = Color.FromArgb(183, 42, 42);
        private static readonly Color DefaultButtonBackColor = Color.FromArgb(44, 110, 203);
        private static readonly Color DefaultButtonHoverBackColor = Color.FromArgb(30, 90, 180);
        private static readonly Color ButtonTextColor = Color.White;


        public static void ImplementarConfiguracion(this DataGridView datagrid, params string[] textosBotones)
        {
            // Configuraciones base
            datagrid.BorderStyle = BorderStyle.None;
            datagrid.AllowUserToAddRows = false;
            datagrid.AllowUserToDeleteRows = false;
            datagrid.AllowUserToResizeColumns = false;
            datagrid.AllowUserToResizeRows = false;
            datagrid.AllowUserToOrderColumns = false; // Ya estaba
            datagrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            datagrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            datagrid.MultiSelect = false;
            datagrid.RowHeadersVisible = false;
            datagrid.ReadOnly = true; // Asegura que el usuario no pueda editar directamente las celdas
            datagrid.BackgroundColor = GridBackColor;
            datagrid.GridColor = GridLinesColor;
            datagrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            datagrid.ColumnHeadersHeight = 38;
            datagrid.EnableHeadersVisualStyles = false; // Crucial para el pintado personalizado de encabezados
            datagrid.RowTemplate.Height = 36;
            datagrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            datagrid.AdvancedRowHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;


            // Eventos para efecto hover en encabezados
            datagrid.CellMouseMove += (sender, e) =>
            {
                if (e.RowIndex == -1 && e.ColumnIndex >= 0) // Encabezado de columna
                {
                    if (HoveredHeaderColumnIndex != e.ColumnIndex)
                    {
                        if (HoveredHeaderColumnIndex != -1)
                        {
                            datagrid.InvalidateColumn(HoveredHeaderColumnIndex); // Invalida el anterior
                        }
                        HoveredHeaderColumnIndex = e.ColumnIndex;
                        datagrid.InvalidateColumn(e.ColumnIndex); // Invalida el actual
                    }
                }
                else if (HoveredHeaderColumnIndex != -1) // El mouse salió del área de encabezados
                {
                    int oldHoveredColumn = HoveredHeaderColumnIndex;
                    HoveredHeaderColumnIndex = -1;
                    datagrid.InvalidateColumn(oldHoveredColumn);
                }
            };

            datagrid.CellMouseLeave += (sender, e) =>
            {
                // Si el mouse sale de una celda de encabezado que estaba en "hover"
                if (e.RowIndex == -1 && HoveredHeaderColumnIndex == e.ColumnIndex)
                {
                    int oldHoveredColumn = HoveredHeaderColumnIndex;
                    HoveredHeaderColumnIndex = -1;
                    datagrid.InvalidateColumn(oldHoveredColumn);
                }
            };

            datagrid.CellPainting += (sender, e) =>
            {
                // Dibujado de encabezados
                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    Color backgroundColor = e.ColumnIndex == HoveredHeaderColumnIndex ? HeaderHoverColor : HeaderNormalColor;

                    using (Brush backBrush = new SolidBrush(backgroundColor))
                    {
                        e.Graphics.FillRectangle(backBrush, e.CellBounds);
                    }

                    TextRenderer.DrawText(
                        e.Graphics,
                        datagrid.Columns[e.ColumnIndex].HeaderText,
                        HeaderFont,
                        e.CellBounds,
                        HeaderTextColor, // El color del texto no cambia con el hover en tu lógica original
                        TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis
                    );

                    e.Handled = true;
                }
                // Dibujado de botones
                else if (e.ColumnIndex >= 0 &&
                         datagrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                         e.RowIndex >= 0)
                {
                    var buttonColumn = datagrid.Columns[e.ColumnIndex] as DataGridViewButtonColumn;
                    Color backColor;
                    bool isHovering = (e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected; // Selected se usa para hover en botones

                    // Determinar color de fondo basado en el tipo de botón y si está en hover
                    if (buttonColumn.Text == "Eliminar") // Asumiendo que el Text original se usa para diferenciar
                    {
                        backColor = isHovering ? DeleteButtonHoverBackColor : DeleteButtonBackColor;
                    }
                    else
                    {
                        backColor = isHovering ? DefaultButtonHoverBackColor : DefaultButtonBackColor;
                    }

                    // Dibujar fondo
                    using (Brush backBrush = new SolidBrush(backColor))
                    {
                        e.Graphics.FillRectangle(backBrush, e.CellBounds);
                    }

                    // Dibujar texto del botón (centrado)
                    TextRenderer.DrawText(e.Graphics,
                                          buttonColumn.Text, // O el valor de la celda si UseColumnTextForButtonValue es true
                                          buttonColumn.DefaultCellStyle.Font, // Usa la fuente definida para la columna de botón
                                          e.CellBounds,
                                          buttonColumn.DefaultCellStyle.ForeColor, // Usa el color de texto definido
                                          TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);


                    // Dibujar borde (opcional, si quieres un borde alrededor del texto/botón dentro de la celda)
                    // El borde de la celda ya está manejado por GridColor y CellBorderStyle
                    // Si deseas un borde específico para el "botón" dentro de la celda:
                    /*Rectangle buttonBounds = e.CellBounds;
                    buttonBounds.Inflate(-2, -2); // Un pequeño margen
                    ControlPaint.DrawBorder(e.Graphics, buttonBounds,
                        GridLinesColor, 1, ButtonBorderStyle.Solid, // Usando GridLinesColor para consistencia
                        GridLinesColor, 1, ButtonBorderStyle.Solid,
                        GridLinesColor, 1, ButtonBorderStyle.Solid,
                        GridLinesColor, 1, ButtonBorderStyle.Solid);*/

                    e.Handled = true;
                }
            };

            // Estilos para celdas
            datagrid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = CellFont,
                BackColor = DefaultCellBackColor,
                ForeColor = DefaultCellForeColor,
                SelectionBackColor = DefaultSelectionBackColor,
                SelectionForeColor = DefaultSelectionForeColor,
                Padding = new Padding(3)
            };

            datagrid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = AlternatingRowBackColor,
                ForeColor = DefaultCellForeColor, // Mantener el mismo color de texto
                SelectionBackColor = DefaultSelectionBackColor, // Mantener el mismo color de selección
                SelectionForeColor = DefaultSelectionForeColor,
                Font = CellFont,
                Padding = new Padding(3)
            };

            // Configuración de botones
            if (textosBotones != null && textosBotones.Length > 0)
            {
                foreach (var texto in textosBotones)
                {
                    var btnColumn = new DataGridViewButtonColumn
                    {
                        Text = texto,
                        Name = $"Columna{texto.Replace(" ", "")}",
                        HeaderText = "", // Los encabezados de las columnas de botones suelen estar vacíos
                        UseColumnTextForButtonValue = true, // El texto del botón será el mismo para todas las filas
                        Width = 85,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                        FlatStyle = FlatStyle.Flat, // Importante para que el pintado personalizado funcione bien
                    };

                    // Estilos base para la celda del botón
                    btnColumn.DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Font = ButtonFont,
                        ForeColor = ButtonTextColor,
                        Padding = new Padding(2),
                        Alignment = DataGridViewContentAlignment.MiddleCenter,
                        // El BackColor y SelectionBackColor se manejarán en CellPainting para el efecto hover
                        // pero podemos definir uno por defecto aquí si CellPainting no se manejara para botones.
                        BackColor = texto == "Eliminar" ? DeleteButtonBackColor : DefaultButtonBackColor,
                        SelectionBackColor = texto == "Eliminar" ? DeleteButtonHoverBackColor : DefaultButtonHoverBackColor
                    };

                    // No es necesario CellTemplate.Style.SelectionBackColor/ForeColor si manejas el hover en CellPainting
                    // Si no lo manejas en CellPainting, entonces sí:
                    /*
                    btnColumn.CellTemplate.Style.SelectionBackColor = texto == "Eliminar"
                        ? DeleteButtonHoverBackColor
                        : DefaultButtonHoverBackColor;
                    btnColumn.CellTemplate.Style.SelectionForeColor = ButtonTextColor;
                    */

                    datagrid.Columns.Add(btnColumn);
                }
            }
        }

        // Considera añadir un método para liberar recursos si fuera necesario, 
        // aunque para fuentes estáticas, se liberarán cuando el AppDomain se descargue.
        // public static void CleanupResources()
        // {
        //    HeaderFont?.Dispose();
        //    CellFont?.Dispose();
        //    ButtonFont?.Dispose();
        // }
    }
}