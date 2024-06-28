using ClassMate.Domain.Abstractions.Services;
using ClassMate.Domain.Dtos;
using ClassMate.Domain.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ClassMate.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings) 
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendSharedNotification(string email, string from, string to, string link) 
        {
            StringBuilder body = new StringBuilder();
            body.AppendLine($"Hello {to}!");
            body.AppendLine();
            body.AppendLine($"{from} shared a resource with you!");
            body.AppendLine($"Check it out here: {link} ");

            SendEmail(email, "Classmate Notification", body.ToString());
        }

        private void SendEmail(string email, string subject, string body)
        {
            using (var client = new SmtpClient())
            {
                client.Host = _emailSettings.Host!;
                client.Port = _emailSettings.Port;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
                using (var message = new MailMessage(
                    from: new MailAddress(_emailSettings.Email!),
                    to: new MailAddress(email)
                    ))
                {

                    message.Subject = subject;
                    message.Body = body;

                    client.Send(message);
                }
            }
        }
    }
}
