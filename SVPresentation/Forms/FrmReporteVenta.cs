

using ClosedXML.Excel;
using SVPresentation.Utilidades;
using SVPresentation.ViewModels;
using SVServices.Interfaces;
using System.Data;

namespace SVPresentation.Forms
{
    public partial class FrmReporteVenta : Form
    {
        private readonly IVentaService _ventaService;
        public FrmReporteVenta(IVentaService ventaService)
        {
            _ventaService = ventaService;
            InitializeComponent();
        }

        private void FrmReporteVenta_Load(object sender, EventArgs e)
        {
            dgvReporte.ImplementarConfiguracion();

        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var lista = await _ventaService.Reporte(
                    dtpFechaInicio.Value.ToString("dd/MM/yyyy"),
                    dtpFechaFin.Value.ToString("dd/MM/yyyy")
                );
            var listaVM = lista.Select(item => new ReporteVentaVM
            {
                NumeroVenta = item.RefVenta.NumeroVenta,
                NombreUsuario = item.RefVenta.UsuarioRegistrado.NombreUsuario,
                FechaRegistro = item.RefVenta.FechaRegistro,
                Producto = item.RefProducto.Descripcion,
                PrecioCompra = item.RefProducto.PrecioCompra,
                PrecioVenta = item.PrecioVenta,
                Cantidad = item.Cantidad,
                PrecioTotal = item.PrecioTotal
            }).ToList();

            dgvReporte.DataSource = listaVM;
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgvReporte.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataTable tabla = new DataTable();
            // Si el DataSource es directamente una lista de ReporteVentaVM, esta conversión es correcta.
            // Sin embargo, si el DataGridView está enlazado a un BindingSource o a otro tipo de colección,
            // es posible que necesites acceder a la lista de otra manera. Asumo que la conversión es correcta por ahora.
            List<ReporteVentaVM> detalles = (List<ReporteVentaVM>)dgvReporte.DataSource;

            foreach (DataGridViewColumn columna in dgvReporte.Columns)
            {
                tabla.Columns.Add(columna.HeaderText, typeof(string));
            }
            foreach (var item in detalles)
            {
                tabla.Rows.Add(
                    item.NumeroVenta,
                    item.NombreUsuario,
                    item.FechaRegistro,
                    item.Producto,
                    item.PrecioCompra,
                    item.PrecioVenta,
                    item.Cantidad,
                    item.PrecioTotal
                );
            }
            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.FileName = $"ReporteVenta_{DateTime.Now.ToString("dd-MM-yyyy-HHmmss")}.xlsx"; // Sugiero usar guiones para evitar problemas con nombres de archivo
                saveFile.Filter = "Excel Files|*.xlsx";
                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add("Reporte de Ventas");
                        hoja.Cell(1, 1).InsertTable(tabla.AsEnumerable());

                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(saveFile.FileName);
                        MessageBox.Show("Exportado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al exportar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}