using SVPresentation.Utilidades;
using SVServices.Interfaces;

namespace SVPresentation.Forms
{
    public partial class FrmActualizarClave : Form
    {
        private readonly IUsuarioService _usuarioService;
        public int _idUsuario { get; set; }
        public FrmActualizarClave(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            InitializeComponent();
        }

        private void FrmActualizarClave_Load(object sender, EventArgs e)
        {
            lblValidacion.Visible = false;
            txbClave.Select();

        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txbClave.Text != txbClavex2.Text)
            {
                lblValidacion.Visible = true;
                lblValidacion.Text = "Las contraseñas no coinciden";
            }
            await _usuarioService.ActualizarClave(_idUsuario, Util.ConvertirASha256(txbClave.Text), 0);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
