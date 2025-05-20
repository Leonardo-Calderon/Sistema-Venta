using SVPresentation.Utilidades;
using SVPresentation.Utilidades.Objetos;
using SVPresentation.ViewModels;
using SVRepository.Entities;
using SVServices.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SVPresentation.Forms
{
    public partial class FrmProducto : Form
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        public FrmProducto(ICategoriaService categoriaService, IProductoService productoService)
        {
            InitializeComponent();
            _categoriaService = categoriaService;
            _productoService = productoService;
        }

        private async void FrmProducto_Load(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
            dgvProductoLista.ImplementarConfiguracion("Editar");
            txbPrecioCompNuevo.ValidarNumero();
            txbPrecioVenNuevo.ValidarNumero();
            txbPrecioCompEditar.ValidarNumero();
            txbPrecioVenEditar.ValidarNumero();
            await MostrarProductos();

            OpcionCombo[] itemsHabiliato = new OpcionCombo[]
            {
                new OpcionCombo { Texto = "Si", Valor = 1 },
                new OpcionCombo { Texto = "No", Valor = 0 }
            };

            var listaCategorias = await _categoriaService.Lista();
            var items = listaCategorias
                .Where(item => item.Activo == 1)
                .Select(item => new OpcionCombo { Texto = item.Nombre, Valor = item.IdCategoria })
                .ToArray();
            cbxCategoriaNuevo.InsertarItems(items);
            cbxCategoriaEditar.InsertarItems(items);
            cbxHabilitadoEditar.InsertarItems(itemsHabiliato);

        }
        public void MostrarTab(string tabName)
        {
            var TabMenu = new TabPage[] { tabLista, tabNuevo, tabEditar };
            foreach (var tab in TabMenu)
            {
                if (tab.Name != tabName)
                {
                    tab.Parent = null;
                }
                else
                {
                    tab.Parent = tabControlMain;
                }
            }
        }

        private async Task MostrarProductos(string buscar = "")
        {
            var listaproductos = await _productoService.Lista(buscar);

            var listaVM = listaproductos.Select(item => new ProductoVM
            {
                IdProducto = item.IdProducto,
                Codigo = item.Codigo,
                Descripcion = item.Descripcion,
                IdCategoria = item.RefCategoria.IdCategoria,
                Categoria = item.RefCategoria.Nombre,
                PrecioCompra = item.PrecioCompra.ToString("0.00"),
                PrecioVenta = item.PrecioVenta.ToString("0.00"),
                Cantidad = item.Cantidad,
                Activo = item.Activo,
                Habilitado = item.Activo == 1 ? "Si" : "No"
            }).ToList();
            dgvProductoLista.DataSource = listaVM;
            dgvProductoLista.Columns["IdProducto"].Visible = false;
            dgvProductoLista.Columns["IdCategoria"].Visible = false;
            dgvProductoLista.Columns["Activo"].Visible = false;
            dgvProductoLista.Columns["Descripcion"].Width = 200;
        }

        private void tabNuevo_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void cbxCategoriaEditar_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await MostrarProductos(txbBuscar.Text);
        }

        private void btnNuevoLista_Click(object sender, EventArgs e)
        {
            MostrarTab(tabNuevo.Name);
            cbxCategoriaNuevo.SelectedIndex = 0;
            txbCodigoNuevo.Text = "";
            txbDescripcionNuevo.Text = "";
            txbPrecioCompNuevo.Text = "";
            txbPrecioVenNuevo.Text = "";
            txbCantidadNuevo.Value = 0;
            cbxCategoriaNuevo.Select();
        }

        private void btnVolverNuevo_Click(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
        }

        private async void btnGuardarNuevo_Click(object sender, EventArgs e)
        {
            if (txbCodigoNuevo.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el código del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbDescripcionNuevo.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la descripción del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbPrecioCompNuevo.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el precio de compra del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbPrecioVenNuevo.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el precio de venta del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbCantidadNuevo.Value == 0)
            {
                MessageBox.Show("Ingrese la cantidad del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            if (!decimal.TryParse(txbPrecioCompNuevo.Text, out precioCompra))
            {
                MessageBox.Show("El precio de compra no es un número válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPrecioCompNuevo.Focus();
                return;
            }
            if (!decimal.TryParse(txbPrecioVenNuevo.Text, out precioVenta))
            {
                MessageBox.Show("El precio de compra no es un número válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPrecioVenNuevo.Focus();
                return;
            }
            if (precioCompra > precioVenta)
            {
                MessageBox.Show("El precio de compra no puede ser mayor al precio de venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPrecioCompNuevo.Focus();
                return;
            }
            var producto = new Producto
            {
                RefCategoria = new Categoria
                {
                    IdCategoria = ((OpcionCombo)cbxCategoriaNuevo.SelectedItem!).Valor
                },
                Codigo = txbCodigoNuevo.Text.Trim(),
                Descripcion = txbDescripcionNuevo.Text.Trim(),
                PrecioCompra = precioCompra,
                PrecioVenta = precioVenta,
                Cantidad = (int)txbCantidadNuevo.Value
            };
            var respuesta = await _productoService.Crear(producto);
            if (respuesta != "")
            {
                MessageBox.Show(respuesta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                await MostrarProductos();
                MostrarTab(tabLista.Name);
            }
        }

private void dgvProductoLista_CellContentClick(object sender, DataGridViewCellEventArgs e)
{
    // Validar que el clic fue en una celda válida y no en el encabezado
    if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

    // Nombre esperado de la columna de acción (botón Editar)
    string nombreColumnaAccion = "ColumnaEditar"; // Asumiendo que el texto del botón es "Editar"

    if (dgvProductoLista.Columns[e.ColumnIndex].Name == nombreColumnaAccion)
    {
        // Obtener el objeto ProductoVM de la fila actual
        var productoSeleccionado = (ProductoVM)dgvProductoLista.Rows[e.RowIndex].DataBoundItem;

        if (productoSeleccionado != null)
        {
            // Cargar datos en los controles de la pestaña de edición
            cbxCategoriaEditar.EstablecerValor(productoSeleccionado.IdCategoria);
            txbCodigoEditar.Text = productoSeleccionado.Codigo;
            txbDescripcionEditar.Text = productoSeleccionado.Descripcion;
            txbPrecioCompEditar.Text = productoSeleccionado.PrecioCompra; // Asumiendo que PrecioCompra es string en VM
            txbPrecioVenEditar.Text = productoSeleccionado.PrecioVenta;   // Asumiendo que PrecioVenta es string en VM
            txbCantidadEditar.Value = productoSeleccionado.Cantidad;        // Para NumericUpDown
            cbxHabilitadoEditar.EstablecerValor(productoSeleccionado.Activo);

            // Cambiar a la pestaña de edición
            MostrarTab(tabEditar.Name);
            cbxCategoriaEditar.Select(); // Opcional: poner el foco en el primer campo
        }
    }
}

        private void btnVolverEditar_Click(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
        }

        private async void btnGuardarEditar_Click(object sender, EventArgs e)
        {
            if (txbCodigoEditar.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el código del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbDescripcionEditar.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese la descripción del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbPrecioCompEditar.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el precio de compra del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbPrecioVenEditar.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese el precio de venta del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (txbCantidadEditar.Value == 0)
            {
                MessageBox.Show("Ingrese la cantidad del producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            if (!decimal.TryParse(txbPrecioCompEditar.Text, out precioCompra))
            {
                MessageBox.Show("El precio de compra no es un número válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPrecioCompEditar.Focus();
                return;
            }
            if (!decimal.TryParse(txbPrecioVenEditar.Text, out precioVenta))
            {
                MessageBox.Show("El precio de compra no es un número válido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPrecioVenEditar.Focus();
                return;
            }
            if (precioCompra > precioVenta)
            {
                MessageBox.Show("El precio de compra no puede ser mayor al precio de venta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txbPrecioCompEditar.Focus();
                return;
            }
            var productoSeleccionado = (ProductoVM)dgvProductoLista.CurrentRow.DataBoundItem;
            var producto = new Producto
            {
                IdProducto = productoSeleccionado.IdProducto,
                RefCategoria = new Categoria
                {
                    IdCategoria = ((OpcionCombo)cbxCategoriaEditar.SelectedItem!).Valor
                },
                Codigo = txbCodigoEditar.Text.Trim(),
                Descripcion = txbDescripcionEditar.Text.Trim(),
                PrecioCompra = precioCompra,
                PrecioVenta = precioVenta,
                Cantidad = (int)txbCantidadEditar.Value,
                Activo = ((OpcionCombo)cbxHabilitadoEditar.SelectedItem!).Valor
            };
            var respuesta = await _productoService.Editar(producto);
            if (respuesta != "")
            {
                MessageBox.Show(respuesta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                await MostrarProductos();
                MostrarTab(tabLista.Name);
            }
        }
    }
}
