using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using TAClassifieds.Models.DAL;

namespace TAClassifieds.HelperClasses
{
    public class SendEmail
    {
        private string fromEmail = System.Configuration.ConfigurationManager.AppSettings["Email_ID"];
        private string password = System.Configuration.ConfigurationManager.AppSettings["Password"];
        public void SendEmailMessage(string toEmail, string body, string subject)
        {

            MailMessage mailMessage = new MailMessage(fromEmail, toEmail);

            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = fromEmail,
                Password = password
            };

            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
        }
    }
}