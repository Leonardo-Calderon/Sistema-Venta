
using Microsoft.Extensions.DependencyInjection;
using SVPresentation.Utilidades;
using SVPresentation.ViewModels;
using SVRepository.Entities;
using SVServices.Interfaces;
using System.Threading.Tasks;

namespace SVPresentation.Forms
{
    public partial class FrmHistorial : Form
    {
        private readonly IVentaService _ventasService;
        private readonly IServiceProvider _serviceProvider;
        public FrmHistorial(IVentaService ventasService, IServiceProvider proveedoresService)
        {
            _ventasService = ventasService;
            _serviceProvider = proveedoresService;
            InitializeComponent();
        }

        private async Task MostrarVenta()
        {
            var listaVenta = await _ventasService.Lista(
                    dtpFechaInicio.Value.ToString("dd/MM/yyyy"),
                    dtpFechaFin.Value.ToString("dd/MM/yyyy"),
                    txbBuscar.Text.Trim()
                );

            var listaVM = listaVenta.Select(item => new VentaVM
            {
                FechaRegistro = item.FechaRegistro,
                NumeroVenta = item.NumeroVenta,
                Usuario = item.UsuarioRegistrado.NombreUsuario,
                Cliente = item.NombreCliente,
                Total = item.precioTotal.ToString("0.00")
            }).ToList();

            dgvVenta.DataSource = listaVM;
        }

        private void FrmHistorial_Load(object sender, EventArgs e)
        {
            dgvVenta.ImplementarConfiguracion("Ver");
            dgvVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await MostrarVenta();
        }

        private void dgvVenta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return; // Validar índices

            var column = dgvVenta.Columns[e.ColumnIndex];
            if (column.Name == "ColumnaVer") // Nombre exacto de la columna
            {
                var filaSeleccionada = (VentaVM)dgvVenta.Rows[e.RowIndex].DataBoundItem;
                var _formDetalle = _serviceProvider.GetRequiredService<FrmDetalleVenta>();
                _formDetalle._numeroVenta = filaSeleccionada.NumeroVenta;
                _formDetalle.ShowDialog();
            }
        }
    }
}
