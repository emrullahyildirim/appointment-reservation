namespace Core.Utilities.Notification.Mail
{
    public interface IMailService
    {
        public Task SendMessageAsync(string[] to, string subject, string body);
        public Task SendMessageAsync(string to, string subject, string body);


    }
}
