
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SVPresentation.Utilidades;
using SVServices.Interfaces;
using System.Threading.Tasks;

namespace SVPresentation.Forms
{
    public partial class FrmLogin : Form
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ICorreoService _correoService;
        private readonly IServiceProvider _serviceProvider;

        public FrmLogin(IUsuarioService usuarioService, ICorreoService correoService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _usuarioService = usuarioService;
            _correoService = correoService;
            _serviceProvider = serviceProvider;

        }

        private async void FrmLogin_Load(object sender, EventArgs e)
        {
            txbUsuario.Select();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void linkOlvideContrasena_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var correo = Interaction.InputBox("Ingrese su correo de usuario", "Olvide mi contraseña", "");
            var idUsuario = await _usuarioService.VerificarCorreo(correo);

            if (idUsuario == 0)
            {
                MessageBox.Show("No existe un usuario registrado con ese correo");
                return;
            }
            var claveGenerada = Util.GenerarCode();
            var claveEncriptada = Util.ConvertirASha256(claveGenerada);
            await _usuarioService.ActualizarClave(idUsuario, claveEncriptada, 1);

            var mensaje = $"Hola, su nueva contraseña temporal es:<br> {claveGenerada}";
            var asunto = "contraseña actualizada";
            await _correoService.Enviar(correo, asunto, mensaje);

            MessageBox.Show("Se ha enviado un correo con la nueva contraseña");
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            var encontrado = await _usuarioService.Login(txbUsuario.Text, Util.ConvertirASha256(txbContrasena.Text));
            if (encontrado.IdUsuario == 0)
            {
                MessageBox.Show("No se encontró al usuario");
                return;
            }
            if (encontrado.Activo == 0)
            {
                MessageBox.Show("El ususario está deshabilitado");
                return;
            }
            if (encontrado.ResetearClave == 1)
            {
                var _formActualizarClave = _serviceProvider.GetRequiredService<FrmActualizarClave>();
                _formActualizarClave._idUsuario = encontrado.IdUsuario;
                _formActualizarClave.ShowDialog();
                var resultado = _formActualizarClave.DialogResult;

                if (resultado== DialogResult.OK)
                {
                    txbContrasena.Text = "";
                    txbUsuario.Select();
                    MessageBox.Show("Contraseña actualizada, por favor inicie sesión nuevamente");
                }

                
            }
            else
            {
                UsuarioSesion.IdUsuario = encontrado.IdUsuario;
                UsuarioSesion.NombreUsuario = encontrado.NombreUsuario;
                UsuarioSesion.NombreCompleto = encontrado.NombreCompleto;
                UsuarioSesion.IdRol = encontrado.RefRol.IdRol;
                UsuarioSesion.Rol = encontrado.RefRol.Nombre;

                var _formLayout = _serviceProvider.GetRequiredService<Layout>();
                this.Hide();
                txbUsuario.Text = "";
                txbContrasena.Text = "";
                _formLayout.Show();
                _formLayout.FormClosed += (sender, e) =>
                {
                    this.Show();
                    txbUsuario.Select();
                };
            }
        }
    }
}
