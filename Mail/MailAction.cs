using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Mail
{
    public class MailAction
    {
        public bool SendMail(string email, string subject, string body)
        {
            try
            {
                MailMessage mailMessage = new()
                {
                    From = new MailAddress("exchangerate69@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);
                MailConnection mailConnection = new();
                mailConnection.SmtpClient.Send(mailMessage);
                mailConnection.SmtpClient.Dispose();
                return true;
            } catch(Exception)
            {
                return false;
                throw;
            }
        }
    }
}
