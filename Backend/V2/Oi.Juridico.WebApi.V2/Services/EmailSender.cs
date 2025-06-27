using System.Net.Mail;
using System.Net;

namespace Oi.Juridico.WebApi.V2.Services
{
    public class EmailSender
    {
        // Our private configuration variables
        private string _host;
        private int _port;
        private bool _enableSSL;
        private bool _useCredentials;
        private string _email;
        private string _name;
        private string _password;

        // Get our parameterized configuration
        public EmailSender(string host, int port, bool enableSSL, string email, string name, string password, bool useCredentials)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSL;
            _email = email;
            _name = name;
            _password = password;
            _useCredentials = useCredentials;
        }

        // Use our configuration to send the email by using SmtpClient
        public Task SendEmailAsync(string email, string nomeUsuario, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port);

            if (_useCredentials)
            {
                client.Credentials = new NetworkCredential(_email, _password);
                client.EnableSsl = _enableSSL;
            }

            var mailMessage = new MailMessage(new MailAddress(_email, _name), new MailAddress(email, nomeUsuario))
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = htmlMessage,
                BodyTransferEncoding = System.Net.Mime.TransferEncoding.QuotedPrintable
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}
