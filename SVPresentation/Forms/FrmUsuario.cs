using SVPresentation.Utilidades;
using SVPresentation.Utilidades.Objetos;
using SVPresentation.ViewModels;
using SVRepository.Entities;
using SVServices.Interfaces;
using System.Data;

namespace SVPresentation.Forms
{
    public partial class FrmUsuario : Form
    {

        private readonly IRolService _rolService;
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoService _correoService;

        public FrmUsuario(IRolService rolService, IUsuarioService usuarioService, ICorreoService correoService)
        {
            InitializeComponent();
            _rolService = rolService;
            _usuarioService = usuarioService;
            _correoService = correoService;
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


        private async Task MostrarUsuarios(string buscar = "")
        {
            var listaUsuarios = await _usuarioService.Lista(buscar);

            var listaVM = listaUsuarios.Select(static item => new UsuarioVM
            {
                IdUsuario = item.IdUsuario,
                IdRol = item.RefRol.IdRol,
                Rol = item.RefRol.Nombre,
                NombreCompleto = item.NombreCompleto,
                Correo = item.Correo,
                NombreUsuario = item.NombreUsuario,
                Activo = item.Activo,
                Habilitado = item.Activo == 1 ? "Si" : "No"
            }).ToList();
            dgvUsuarios.DataSource = listaVM;
            dgvUsuarios.Columns["IdUsuario"].Visible = false;
            dgvUsuarios.Columns["IdRol"].Visible = false;
            dgvUsuarios.Columns["Activo"].Visible = false;
        }

        private async void FrmUsuario_Load(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
            dgvUsuarios.ImplementarConfiguracion("Editar", "Eliminar");
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            await MostrarUsuarios();

            OpcionCombo[] itemsHabiliato = new OpcionCombo[]
            {
                new OpcionCombo { Texto = "Si", Valor = 1 },
                new OpcionCombo { Texto = "No", Valor = 0 }
            };

            var listaRol = await _rolService.Lista();
            var items = listaRol.Select(item => new OpcionCombo { Texto = item.Nombre, Valor = item.IdRol }).ToArray();
            cbxRolNuevo.InsertarItems(items);
            cbxRolEditar.InsertarItems(items);
            cbxHabilitado.InsertarItems(itemsHabiliato);

        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            await MostrarUsuarios(txbBuscar.Text);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            cbxRolNuevo.SelectedIndex = 0;
            txbNomComNuevo.Text = "";
            txbCorreoNuevo.Text = "";
            txbNomUsuNuevo.Text = "";

            MostrarTab(tabNuevo.Name);
        }

        private void btnVolverNuevo_Click(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
        }

        private async void btnGuardarNuevo_Click(object sender, EventArgs e)
        {
            if (txbNomComNuevo.Text.Trim() == "")
            {
                MessageBox.Show("El nombre completo es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txbCorreoNuevo.Text.Trim() == "")
            {
                MessageBox.Show("El correo es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txbNomUsuNuevo.Text.Trim() == "")
            {
                MessageBox.Show("El nombre de usuario es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var claveGenerada = Util.GenerarCode();
            var claveEncriptada = Util.ConvertirASha256(claveGenerada);

            var objeto = new Usuario
            {

                RefRol = new Rol { IdRol = ((OpcionCombo)cbxRolNuevo.SelectedItem!).Valor },
                NombreCompleto = txbNomComNuevo.Text.Trim(),
                Correo = txbCorreoNuevo.Text.Trim(),
                NombreUsuario = txbNomUsuNuevo.Text.Trim(),
                Clave = claveEncriptada
            };

            var respuesta = await _usuarioService.Crear(objeto);

            if (respuesta != "")
            {
                MessageBox.Show(respuesta);
            }
            else
            {
                var mensaje = $"Usuario creado correctamente. \n\n" +
                    $"Nombre de usuario: {txbNomUsuNuevo.Text.Trim()} \n" +
                    $"Clave: {claveGenerada} \n\n" +
                    $"Por favor cambie la clave al iniciar sesión.";

                await _correoService.Enviar(objeto.Correo, "Usuario creado", mensaje);

                await MostrarUsuarios();
                MostrarTab(tabLista.Name);
            }
        }

        private async void dgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            /**
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "ColumnaAccion")
            {
                var usuarioSeleccionado = (UsuarioVM)dgvUsuarios.CurrentRow.DataBoundItem;

                cbxRolEditar.EstablecerValor(usuarioSeleccionado.IdRol);
                txbNomComEditar.Text = usuarioSeleccionado.NombreCompleto;
                tbxCorreoEditar.Text = usuarioSeleccionado.Correo;
                txbNomUsuEditar.Text = usuarioSeleccionado.NombreUsuario;
                cbxHabilitado.EstablecerValor(usuarioSeleccionado.Activo);

                MostrarTab(tabEditar.Name);
                txbNomComEditar.Select();


            }
            **/
            var grid = (DataGridView)sender;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var columnName = grid.Columns[e.ColumnIndex].Name;
                var usuarioSeleccionado = (UsuarioVM)grid.Rows[e.RowIndex].DataBoundItem;

                if (columnName == "ColumnaEditar")
                {

                    cbxRolEditar.EstablecerValor(usuarioSeleccionado.IdRol);
                    txbNomComEditar.Text = usuarioSeleccionado.NombreCompleto;
                    tbxCorreoEditar.Text = usuarioSeleccionado.Correo;
                    txbNomUsuEditar.Text = usuarioSeleccionado.NombreUsuario;
                    cbxHabilitado.EstablecerValor(usuarioSeleccionado.Activo);

                    MostrarTab(tabEditar.Name);
                    txbNomComEditar.Select();
                }
                else if (columnName == "ColumnaEliminar")
                {
                    var confirmacion = MessageBox.Show(
                        $"¿Está seguro de eliminar al usuario {usuarioSeleccionado.NombreCompleto}?",
                        "Confirmar eliminación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (confirmacion == DialogResult.Yes)
                    {
                        var resultado = await _usuarioService.Eliminar(usuarioSeleccionado.IdUsuario);

                        if (string.IsNullOrEmpty(resultado))
                        {
                            await MostrarUsuarios();
                            MessageBox.Show("Usuario eliminado correctamente");
                        }
                        else
                        {
                            MessageBox.Show(resultado, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnVolverEditar_Click(object sender, EventArgs e)
        {
            MostrarTab(tabLista.Name);
        }

        private async void btnGuardarEditar_Click(object sender, EventArgs e)
        {
            if (txbNomComEditar.Text.Trim() == "")
            {
                MessageBox.Show("El nombre completo es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tbxCorreoEditar.Text.Trim() == "")
            {
                MessageBox.Show("El correo es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txbNomUsuEditar.Text.Trim() == "")
            {
                MessageBox.Show("El nombre de usuario es requerido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var usuarioSeleccionado = (UsuarioVM)dgvUsuarios.CurrentRow.DataBoundItem;

            var objeto = new Usuario
            {
                IdUsuario = usuarioSeleccionado.IdUsuario,
                RefRol = new Rol { IdRol = ((OpcionCombo)cbxRolEditar.SelectedItem!).Valor },
                NombreCompleto = txbNomComEditar.Text.Trim(),
                Correo = tbxCorreoEditar.Text.Trim(),
                NombreUsuario = txbNomUsuEditar.Text.Trim(),
                Activo = ((OpcionCombo)cbxHabilitado.SelectedItem!).Valor
            };

            var respuesta = await _usuarioService.Editar(objeto);

            if (respuesta != "")
            {
                MessageBox.Show(respuesta);
            }
            else
            {

                await MostrarUsuarios();
                MostrarTab(tabLista.Name);
            }
        }
    }
}
