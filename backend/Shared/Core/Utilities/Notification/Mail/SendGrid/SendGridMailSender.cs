using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Notification.Mail.SmptMail;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Notification.Mail.SendGrid
{
    public class SendGridMailSender:IMailService
    {
        private readonly SendGridOptions _mailOptions;
        private readonly ILoggerServiceBase _logger;
        public SendGridMailSender(IConfiguration configuration, ILoggerServiceBase logger)
        {
            _mailOptions = configuration.GetSection("MailSendGrid").Get<SendGridOptions>();
            _logger = logger;
        }


        public async Task SendMessageAsync(string to, string subject, string body)
        {
            await SendMessageAsync(new[] { to }, subject, body);
        }

        public async Task SendMessageAsync(string[] to, string subject, string body)
        {
            try
            {
                var client = new SendGridClient(_mailOptions.ApiKey);
                var from = new EmailAddress(_mailOptions.SenderEmail, _mailOptions.UserName); 
                var toEmails = to.Select(email => new EmailAddress(email)).ToList();
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toEmails, subject, body, body);
                var response = await client.SendEmailAsync(msg);
                _logger.Info($"Mail gönderildi: {string.Join(", ", to)}");
            }
            catch (Exception e)
            {
                _logger.Error($"Mail gönderilemedi {string.Join(", ", to)}. Hata: {e.ToString()}");
            }
        }
    }
}
