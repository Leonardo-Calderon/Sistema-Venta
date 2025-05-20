
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using SVServices.Interfaces;

namespace SVServices.Implementation
{
    public class CorreoService : ICorreoService
    {
        private readonly  IConfiguration _configuracion;
        private SmtpClient _smtp;
        private readonly string _host;
        private readonly int _port;
        private readonly string _user;
        private readonly string _pass;

        public CorreoService(IConfiguration configuracion)
        {
            _smtp = new SmtpClient();
            _configuracion = configuracion;

            _host = _configuracion["Smtp:Host"]!;
            _port = Convert.ToInt32(_configuracion["Smtp:Port"]!);
            _user = _configuracion["Smtp:User"]!;
            _pass = _configuracion["Smtp:Pass"]!;
        }

        public async Task Enviar(string para, string asunto, string mensajeHTML)
        {
            _smtp = new SmtpClient();
            _smtp.Connect(_host, _port, SecureSocketOptions.StartTls);
            _smtp.Authenticate(_user, _pass);

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
    }
}
