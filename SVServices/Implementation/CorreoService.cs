
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    /// <summary>
    /// Implementación del servicio para el envío de correos electrónicos.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz ICorreoService y proporciona la funcionalidad
    /// para enviar correos electrónicos utilizando el protocolo SMTP. Utiliza la biblioteca
    /// MailKit para el envío seguro de correos con soporte para TLS/SSL.
    /// </remarks>
    public class CorreoService : ICorreoService
    {
        /// <summary>
        /// Instancia de configuración para acceder a los parámetros de SMTP.
        /// </summary>
        private readonly IConfiguration _configuracion;

        /// <summary>
        /// Cliente SMTP para el envío de correos.
        /// </summary>
        private SmtpClient _smtp;

        /// <summary>
        /// Host del servidor SMTP.
        /// </summary>
        private readonly string _host;

        /// <summary>
        /// Puerto del servidor SMTP.
        /// </summary>
        private readonly int _port;

        /// <summary>
        /// Usuario para la autenticación SMTP.
        /// </summary>
        private readonly string _user;

        /// <summary>
        /// Contraseña para la autenticación SMTP.
        /// </summary>
        private readonly string _pass;

        /// <summary>
        /// Inicializa una nueva instancia de la clase CorreoService.
        /// </summary>
        /// <param name="configuracion">Instancia de configuración que contiene los parámetros SMTP.</param>
        /// <remarks>
        /// El constructor lee la configuración SMTP desde el archivo de configuración
        /// y prepara el cliente SMTP para el envío de correos.
        /// </remarks>
        public CorreoService(IConfiguration configuracion)
        {
            _configuracion = configuracion ?? throw new ArgumentNullException(nameof(configuracion));
            _smtp = new SmtpClient();

            // Leer configuración SMTP desde el archivo de configuración
            _host = _configuracion["Smtp:Host"] ?? throw new InvalidOperationException("La configuración Smtp:Host no está definida.");
            _port = Convert.ToInt32(_configuracion["Smtp:Port"] ?? throw new InvalidOperationException("La configuración Smtp:Port no está definida."));
            _user = _configuracion["Smtp:User"] ?? throw new InvalidOperationException("La configuración Smtp:User no está definida.");
            _pass = _configuracion["Smtp:Pass"] ?? throw new InvalidOperationException("La configuración Smtp:Pass no está definida.");
        }

        /// <summary>
        /// Envía un correo electrónico a un destinatario específico.
        /// </summary>
        /// <param name="para">Dirección de correo electrónico del destinatario.</param>
        /// <param name="asunto">Asunto del correo electrónico.</param>
        /// <param name="mensajeHTML">Contenido del correo en formato HTML.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <remarks>
        /// Este método establece una conexión SMTP segura, autentica con el servidor,
        /// crea y envía el mensaje de correo, y finalmente cierra la conexión.
        /// </remarks>
        public async Task Enviar(string para, string asunto, string mensajeHTML)
        {
            if (string.IsNullOrWhiteSpace(para))
                throw new ArgumentException("La dirección de correo del destinatario no puede estar vacía.", nameof(para));

            if (string.IsNullOrWhiteSpace(asunto))
                throw new ArgumentException("El asunto del correo no puede estar vacío.", nameof(asunto));

            if (string.IsNullOrWhiteSpace(mensajeHTML))
                throw new ArgumentException("El contenido del correo no puede estar vacío.", nameof(mensajeHTML));

            try
            {
                _smtp = new SmtpClient();
                await _smtp.ConnectAsync(_host, _port, SecureSocketOptions.StartTls);
                await _smtp.AuthenticateAsync(_user, _pass);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_user));
                email.To.Add(MailboxAddress.Parse(para));
                email.Subject = asunto;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = mensajeHTML
                };

                await _smtp.SendAsync(email);
                await _smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al enviar el correo electrónico: {ex.Message}", ex);
            }
            finally
            {
                if (_smtp != null && _smtp.IsConnected)
                {
                    await _smtp.DisconnectAsync(true);
                }
                _smtp?.Dispose();
            }
        }
    }
}
