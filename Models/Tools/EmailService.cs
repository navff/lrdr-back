using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Camps.Tools;
using NLog;
using NLog.Fluent;

namespace Models.Tools
{
    public static class EmailService
    {

        public static bool SendEmail(EmailMessage message)
        {
            try
            {
                var msg = new MailMessage();
                msg.From = new MailAddress(message.From);
                msg.To.Add(new MailAddress(message.To));
                msg.Subject = message.EmailSubject;
                //Добавляем текстовое и html представление для разных клиентов
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Plain));
                msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(message.Body, null, MediaTypeNames.Text.Html));
                var smtpClient = new SmtpClient("smtp-pulse.com", Convert.ToInt32(2525));
                var credentials = new NetworkCredential("var@33kita.ru", "J9FnHkWjK8H3br");
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = false;
                smtpClient.Send(msg);
                smtpClient.Dispose();
                return true;
            }
            catch (SmtpException ex)
            {
                ErrorLogger.Log("CANNOT SEND EMAIL", ex);
                throw;
            }
        }
    }

    public class EmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public string EmailSubject { get; set; }
    }

    public class UserMessages
    {
        public static readonly string SubjectConfirmEmail = "Пожалуйста подтвердите Ваш E-mail";
        public static readonly string SubjectConfirmLogin = "Подтвердите вход на сайт «Light Order»";
    }
}
