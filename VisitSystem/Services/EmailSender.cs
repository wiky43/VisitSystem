using MimeKit;
using MailKit.Net.Smtp;

namespace VisitSystem.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarReporteAsync(string contenido)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sistema Reportes", _config["Email:Usuario"]));
            message.To.Add(new MailboxAddress("", _config["Email:Destino"]));
            message.Subject = "Reporte Diario";

            message.Body = new TextPart("html") { Text = contenido };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);
            await smtp.AuthenticateAsync(_config["Email:Usuario"], _config["Email:Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

        public async Task EnviarReporteConAdjuntosAsync(string subject, string body, Dictionary<string, byte[]> adjuntos)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Sistema Reportes", _config["Email:Usuario"]));
            message.To.Add(new MailboxAddress("", _config["Email:Destino"]));
            message.Subject = subject;

            var multipart = new Multipart("mixed");

            
            var bodyPart = new TextPart("html") { Text = body };
            multipart.Add(bodyPart);

            
            foreach (var adj in adjuntos)
            {
                var attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(new MemoryStream(adj.Value)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = adj.Key
                };
                multipart.Add(attachment);
            }

            message.Body = multipart;

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["Email:Usuario"], _config["Email:Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }

    }
}
