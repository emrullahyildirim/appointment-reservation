using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Notification.Mail.SendGrid
{
    public class SendGridOptions
    {
        public string UserName { get; set; }
        public string SenderEmail { get; set; }
        public string ApiKey { get; set; }
    }
}
