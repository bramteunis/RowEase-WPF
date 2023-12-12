using System.Net.Mail;
using System.Net;

namespace Mail
{
    internal class MailConnection
    {
        internal SmtpClient SmtpClient { get; set; }

        public MailConnection()
        {
            SmtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("exchangerate69@gmail.com", "swdokbrqbiiszmvi"),
                EnableSsl = true
            };
        }
    }
}