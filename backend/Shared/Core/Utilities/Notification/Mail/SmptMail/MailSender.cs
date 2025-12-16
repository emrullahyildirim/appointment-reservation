using Core.CrossCuttingConcerns.Logging;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Core.Utilities.Notification.Mail.SmptMail
{
    public class MailSender : IMailService
    {
        private readonly MailOptions _mailOptions;
        private readonly ILoggerServiceBase _logger;
        public MailSender(IConfiguration configuration, ILoggerServiceBase logger)
        {
            _mailOptions = configuration.GetSection("Mail").Get<MailOptions>();
            _logger = logger;
        }

        private MimeMessage CreateMessage(string[] to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("İyself", _mailOptions.UserName));

            foreach (var recipient in to)
                message.To.Add(MailboxAddress.Parse(recipient));

            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            return message;
        }

        public async Task SendMessageAsync(string to, string subject, string body)
        {
            await SendMessageAsync(new[] { to }, subject, body);
        }

        public async Task SendMessageAsync(string[] to, string subject, string body)
        {
            var message = CreateMessage(to, subject, body);
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_mailOptions.Host, _mailOptions.Port, MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_mailOptions.UserName, _mailOptions.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                _logger.Info($"Mail gönderildi: {string.Join(", ", to)}");
            }
            catch (Exception e)
            {
                _logger.Error($"Mail gönderilemedi {string.Join(", ", to)}. Hata: {e.ToString()}");
            }
        }
    }
}
