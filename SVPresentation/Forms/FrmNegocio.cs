using SVRepository.Entities;
using SVServices.Interfaces;
using SVServices.Recursos.Cloudinary;


namespace SVPresentation.Forms
{
    public partial class FrmNegocio : Form
    {
        private readonly INegocioService _negocioService;
        private readonly ICloudinaryService _cloudinaryService;
        OpenFileDialog _openFileDialog = new OpenFileDialog();
        Negocio _negocio = new Negocio();
        public FrmNegocio(INegocioService negocioService, ICloudinaryService cloudinaryService)
        {
            InitializeComponent();
            _negocioService = negocioService;
            _cloudinaryService = cloudinaryService;

        }

        private async void FrmNegocio_Load(object sender, EventArgs e)
        {
            _openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            pbLogo.SizeMode = PictureBoxSizeMode.StretchImage;

            _negocio = await _negocioService.Obtener();

            txbRazonSocial.Text = _negocio.RazonSocial;
            txbRFC.Text = _negocio.RFC;
            txbDireccion.Text = _negocio.Direccion;
            txbCelular.Text = _negocio.Celular;
            txbCorreo.Text = _negocio.Correo;
            txbSimboloMoneda.Text = _negocio.SimboloMoneda;

            pbLogo.ImageLocation = _negocio.URL;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txbNombreNuevo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                _openFileDialog.OpenFile();
                pbLogo.Image = Image.FromFile(_openFileDialog.FileName);

                txbRutaImagen.Text = _openFileDialog.FileName;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private async void btnGuardarConfiguracion_Click(object sender, EventArgs e)
        {
            CloudinaryResponse cloudinaryResponse = new CloudinaryResponse();
            Negocio negocio = new Negocio();

            if (_openFileDialog.FileName != "")
            {
                cloudinaryResponse = await _cloudinaryService.SubirImagen(_openFileDialog.SafeFileName,_openFileDialog.OpenFile());

                if(cloudinaryResponse.PublicId != "")
                {
                    if (_negocio.NombreLogo != "")
                    {
                        await _cloudinaryService.EliminarImagen(_negocio.NombreLogo);
                    }
                    negocio.NombreLogo = cloudinaryResponse.PublicId;
                    negocio.URL = cloudinaryResponse.SecureUrl;

                    _negocio.NombreLogo = cloudinaryResponse.PublicId;
                    _negocio.URL = cloudinaryResponse.SecureUrl;
                }
            }
            else
            {
                negocio.NombreLogo = _negocio.NombreLogo;
                negocio.URL = _negocio.URL;
            }
            negocio.RazonSocial = txbRazonSocial.Text;
            negocio.RFC = txbRFC.Text;
            negocio.Direccion = txbDireccion.Text;
            negocio.Celular = txbCelular.Text;
            negocio.Correo = txbCorreo.Text;
            negocio.SimboloMoneda = txbSimboloMoneda.Text;

            await _negocioService.Editar(negocio);

            MessageBox.Show("Los datos se han guardado correctamente", "Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);

            txbRutaImagen.Text = "";
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

        }
    }
}
