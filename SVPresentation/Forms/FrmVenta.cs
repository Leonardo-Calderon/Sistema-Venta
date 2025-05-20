using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SVPresentation.Utilidades;
using SVPresentation.ViewModels;
using SVRepository.Entities;
using SVServices.Interfaces;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static QuestPDF.Helpers.Colors;

namespace SVPresentation.Forms
{
    public partial class FrmVenta : Form
    {
        private readonly IProductoService _productoService;
        private readonly IVentaService _ventaService;
        private readonly INegocioService _negocioService;
        private readonly IServiceProvider _serviceProvider;
        private BindingList<DetalleVentaVM> _detalleVenta = new BindingList<DetalleVentaVM>();
        public FrmVenta(IProductoService productoService,
            IVentaService ventaService,
            INegocioService negocioService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _productoService = productoService;
            _ventaService = ventaService;
            _negocioService = negocioService;
            _serviceProvider = serviceProvider;

        }

        private void FrmVenta_Load(object sender, EventArgs e)
        {
            dgvDetalleVenta.ImplementarConfiguracion("Borrar");
            dgvDetalleVenta.DataSource = _detalleVenta;
            dgvDetalleVenta.Columns["IdProducto"].Visible = false;
            dgvDetalleVenta.Columns["CantidadValor"].Visible = false;
            dgvDetalleVenta.Columns["Producto"].FillWeight = 350;
            dgvDetalleVenta.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txbPagoCon.ValidarNumero();
        }

        private async Task AgregarProducto(string codigoProducto)
        {
            var producto = await _productoService.Obtener(codigoProducto);
            if (producto.IdProducto == 0)
            {
                MessageBox.Show("El producto no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbCodigoProducto.BackColor = Color.FromArgb(255, 227, 227);
                return;
            }
            txbCodigoProducto.BackColor = SystemColors.Window;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(producto.Descripcion);
            sb.AppendLine("Categoria: " + producto.RefCategoria.Nombre);
            sb.AppendLine("Precio: " + producto.PrecioVenta.ToString("0.00"));
            sb.AppendLine();
            sb.AppendLine("Ingrese la cantidad (" + producto.RefCategoria.RefMedida.Equivalente + " )");

            string cantidadString = Interaction.InputBox(sb.ToString(), "Producto", "1");

            if (string.IsNullOrEmpty(cantidadString))
            {
                return;
            }
            int cantidad;
            if (!int.TryParse(cantidadString, out cantidad))
            {
                MessageBox.Show("La cantidad no es válida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cantidad > producto.Cantidad)
            {
                MessageBox.Show("No hay suficiente cantidad", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var detalle = _detalleVenta.FirstOrDefault(x => x.IdProducto == producto.IdProducto);

            if (detalle == null)
            {
                decimal total = (cantidad * producto.PrecioVenta) / producto.RefCategoria.RefMedida.Valor;
                _detalleVenta.Add(new DetalleVentaVM
                {
                    IdProducto = producto.IdProducto,
                    Producto = producto.Descripcion,
                    Precio = producto.PrecioVenta,
                    CantidadValor = cantidad,
                    CantidadTexto = string.Concat(cantidad, " ", producto.RefCategoria.RefMedida.Equivalente),
                    Total = Convert.ToDecimal(total.ToString("0.00"))
                });
            }
            else
            {
                int index = _detalleVenta.IndexOf(detalle);
                int cantidadTotal = detalle.CantidadValor + cantidad;
                decimal total = (cantidadTotal * producto.PrecioVenta) / producto.RefCategoria.RefMedida.Valor;

                detalle.CantidadValor = cantidadTotal;
                detalle.CantidadTexto = string.Concat(cantidadTotal + " " + producto.RefCategoria.RefMedida.Equivalente);
                detalle.Total = Convert.ToDecimal(total.ToString("0.00"));
                _detalleVenta[index] = detalle;
            }

            decimal Total = _detalleVenta.Sum(x => x.Total);
            lblTotal.Text = Total.ToString("0.00");
            txbCodigoProducto.Text = "";

        }

        private async void txbCodigoProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                await AgregarProducto(txbCodigoProducto.Text.Trim());
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var _buscarProducto = _serviceProvider.GetRequiredService<FrmBuscarProducto>();
            var resultado = _buscarProducto.ShowDialog();

            if (resultado == DialogResult.OK)
            {
                var producto = _buscarProducto._productoSeleccionado;
                if (producto != null)
                {
                    txbCodigoProducto.Text = producto.Codigo;
                    await AgregarProducto(producto.Codigo);
                }
            }
        }

        private void dgvDetalleVenta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalleVenta.Columns[e.ColumnIndex].Name == "ColumnaAccion")
            {
                var filaSeleccionada = (DetalleVentaVM)dgvDetalleVenta.CurrentRow.DataBoundItem;
                var index = _detalleVenta.IndexOf(filaSeleccionada);
                _detalleVenta.RemoveAt(index);

                decimal Total = _detalleVenta.Sum(x => x.Total);
                lblTotal.Text = Total.ToString("0.00");

            }
        }

        private async void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            if (_detalleVenta.Count == 0)
            {
                MessageBox.Show("No hay productos");
                return;
            }

            decimal tempTotal = _detalleVenta.Sum(x => x.Total);
            var precioTotal = Convert.ToDecimal(tempTotal.ToString("0.00"));

            var pagoCon = txbPagoCon.Text.Trim() ==
                "" ? precioTotal : Convert.ToDecimal(txbPagoCon.Text.Trim());

            var cambio = txbCambio.Text.Trim() == "" ? 0 : Convert.ToDecimal(txbCambio.Text.Trim());

            XElement venta = new XElement("Venta",
                new XElement("IdUsuarioRegistro", UsuarioSesion.IdUsuario),
                new XElement("NombreCliente", txbNombreCliente.Text.Trim()),
                new XElement("PrecioTotal", precioTotal),
                new XElement("PagoCon", pagoCon),
                new XElement("Cambio", cambio)
            );
            XElement detallesVenta = new XElement("DetalleVenta");

            foreach (var item in _detalleVenta)
            {
                detallesVenta.Add(new XElement("Item",
                   new XElement("IdProducto", item.IdProducto),
                   new XElement("Cantidad", item.CantidadValor),
                   new XElement("PrecioVenta", item.Precio),
                   new XElement("PrecioTotal", item.Total)

                 ));
            }
            venta.Add(detallesVenta);

            var numeroVenta = await _ventaService.Registrar(venta.ToString());

            if (numeroVenta == "")
            {
                MessageBox.Show("Error al registrar la venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txbCodigoProducto.Text = "";
            txbNombreCliente.Text = "";
            _detalleVenta.Clear();
            lblTotal.Text = "0";
            txbPagoCon.Text = "";
            txbCambio.Text = "";
            txbCodigoProducto.Select();

            DialogResult resultado = MessageBox.Show(
                           $"Venta registrada con éxito. Número de venta: {numeroVenta}. ¿Desea imprimir la boleta?",
                           "Confirmación",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Information
                       );

            if (resultado == DialogResult.Yes)
            {
                var oNegocio = await _negocioService.Obtener();
                var oVenta = await _ventaService.Obtener(numeroVenta);
                var oDetalleVenta = await _ventaService.ObtenerDetalle(numeroVenta);
                oVenta.RefDetalleVenta = oDetalleVenta;

                MemoryStream imagenLogo;
                using (var httpClient = new HttpClient())
                {
                    var imageBytes = await httpClient.GetByteArrayAsync(oNegocio.URL);
                    imagenLogo = new MemoryStream(imageBytes);
                }
                var arrayPDF = Util.GeneratePDFVenta(oNegocio, oVenta, imagenLogo);

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Guardar boleta";
                    saveFileDialog.DefaultExt = "pdf";
                    saveFileDialog.AddExtension = true;
                    saveFileDialog.FileName = $"Boleta_{numeroVenta}.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        await File.WriteAllBytesAsync(saveFileDialog.FileName, arrayPDF);

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = saveFileDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                }
            }
        }

        private void txbPagoCon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
            {
                return;
            }

            decimal pagoCon;
            decimal total = _detalleVenta.Sum(x => x.Total);

            if (txbPagoCon.Text.Trim() != "")
            {
                if (decimal.TryParse(txbPagoCon.Text.Trim(), out pagoCon))
                {
                    if (pagoCon < total)
                    {
                        txbCambio.Text = "0.00";
                    }
                    else
                    {
                        decimal cambio = pagoCon - total;
                        txbCambio.Text = cambio.ToString("0.00");
                    }
                }
                else
                {
                    MessageBox.Show("El pago no es válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txbPagoCon.Text = "";
                    return;
                }

            }
        }
    }

}
