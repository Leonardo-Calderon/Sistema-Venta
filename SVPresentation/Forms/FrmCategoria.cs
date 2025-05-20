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
    public partial class FrmCategoria : Form
    {
        private readonly IMedidaService _medidaService;
        private readonly ICategoriaService _categoriaService;
        public FrmCategoria(ICategoriaService categoriaService, IMedidaService medidaService)
        {
            InitializeComponent();
            _categoriaService = categoriaService;
            _medidaService = medidaService;
        }

        private void tabNuevo_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnVolverNuevo_Click(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private async Task MostrarCategorias(string buscar = "")
        {
            var listacategorias = await _categoriaService.Lista(buscar);

            var listaVM = listacategorias.Select(static item => new CategoriaVM
            {
                IdCategoria = item.IdCategoria,
                Nombre = item.Nombre,
                IdMedida = item.RefMedida.IdMedida,
                Medida = item.RefMedida.Nombre,
                Activo = item.Activo,
                Habilitado = item.Activo == 1 ? "Si" : "No"
            }).ToList();
            dgvCategoriasLista.DataSource = listaVM;
            dgvCategoriasLista.Columns["IdCategoria"].Visible = false;
            dgvCategoriasLista.Columns["IdMedida"].Visible = false;
            dgvCategoriasLista.Columns["Activo"].Visible = false;
        }

        private async void FrmCategoria_Load(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
            dgvCategoriasLista.ImplementarConfiguracion("Editar");
            dgvCategoriasLista.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            await MostrarCategorias();

            OpcionCombo[] itemsHabiliato = new OpcionCombo[]
            {
                new OpcionCombo { Texto = "Si", Valor = 1 },
                new OpcionCombo { Texto = "No", Valor = 0 }
            };

            var listaMedidas = await _medidaService.Lista();
            var items = listaMedidas.Select(item => new OpcionCombo { Texto = item.Nombre, Valor = item.IdMedida }).ToArray();
            cbxMedidaNuevo.InsertarItems(items);
            cbxMedidaEditar.InsertarItems(items);
            cbxHabilitadoEditar.InsertarItems(itemsHabiliato);

        }

        private void dgvCategorias_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validar que el clic fue en una celda válida y no en el encabezado
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Nombre esperado de la columna de acción (botón Editar)
            // Este nombre debe coincidir con el 'Name' que se le dio al DataGridViewButtonColumn
            // al momento de crearlo en el método CustomDataGridView.ImplementarConfiguracion
            string nombreColumnaAccion = "ColumnaEditar"; // Asumiendo que el texto del botón es "Editar"

            if (dgvCategoriasLista.Columns[e.ColumnIndex].Name == nombreColumnaAccion)
            {
                // Obtener el objeto CategoriaVM de la fila actual
                // Asegúrate de que dgvCategoriasLista.DataSource esté correctamente casteado a una lista de CategoriaVM o
                // que el DataBoundItem sea del tipo correcto.
                var categoriaSeleccionada = (CategoriaVM)dgvCategoriasLista.Rows[e.RowIndex].DataBoundItem;

                if (categoriaSeleccionada != null)
                {
                    // Cargar datos en los controles de la pestaña de edición
                    txbNombreEditar.Text = categoriaSeleccionada.Nombre; // Asumiendo que Nombre es string
                                                                         // Para los ComboBox, necesitas establecer el valor seleccionado.
                                                                         // El método de extensión 'EstablecerValor' que tienes es útil aquí.
                    cbxMedidaEditar.EstablecerValor(categoriaSeleccionada.IdMedida);
                    cbxHabilitadoEditar.EstablecerValor(categoriaSeleccionada.Activo);

                    // Cambiar a la pestaña de edición
                    MostrarTab(tabEditar.Name);
                    txbNombreEditar.Select(); // Opcional: poner el foco en el primer campo
                }
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await MostrarCategorias(txbBuscar.Text);
        }

        private void btnNuevoLista_Click(object sender, EventArgs e)
        {
            txbNombreNuevo.Text = "";
            cbxMedidaNuevo.SelectedIndex = 0;
            txbNombreNuevo.Select();

            MostrarTab(tabNuevo.Name);
        }

        private void btnVolverEditar_Click(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
        }

        private async void btnGuardarNuevo_Click(object sender, EventArgs e)
        {
            if (txbNombreNuevo.Text.Trim() == "")
            {
                MessageBox.Show("El nombre de la categoria es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var medida = (OpcionCombo)cbxMedidaNuevo.SelectedItem!;
            var objeto = new Categoria
            {
                Nombre = txbNombreNuevo.Text.Trim(),
                RefMedida = new Medida { IdMedida = medida.Valor }
            };
            var respuesta = await _categoriaService.Crear(objeto);
            if (respuesta != "")
            {
                MessageBox.Show(respuesta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                await MostrarCategorias();
                MostrarTab(tabLista.Name);
            }

        }

        private async void btnGuardarEditar_Click(object sender, EventArgs e)
        {
            if (txbNombreEditar.Text.Trim() == "")
            {
                MessageBox.Show("El nombre de la categoria es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var categoriaSeleccionada = (CategoriaVM)dgvCategoriasLista.CurrentRow.DataBoundItem;
            var objeto = new Categoria
            {
                IdCategoria = categoriaSeleccionada.IdCategoria,
                Nombre = txbNombreEditar.Text.Trim(),
                RefMedida = new Medida { IdMedida = ((OpcionCombo)cbxMedidaEditar.SelectedItem!).Valor },
                Activo = ((OpcionCombo)cbxHabilitadoEditar.SelectedItem!).Valor,
            };
            var respuesta = await _categoriaService.Editar(objeto);
            if (respuesta != "")
            {
                MessageBox.Show(respuesta, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                await MostrarCategorias();
                MostrarTab(tabLista.Name);
            }
        }

        private void cbxMedidaNuevo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
