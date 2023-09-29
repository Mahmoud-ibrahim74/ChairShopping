using System.Net.Mail;
using System.Net;
using ChairShopping.Interfaces;

namespace ChairShopping.Repositories
{
    public class GmailEmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;


        public GmailEmailSender()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com", 587);
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.EnableSsl = true;
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.Credentials = new NetworkCredential("hooda01018904042@gmail.com", "");
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var message = new MailMessage("hooda01018904042@gmail.com", email, subject, body);
            await _smtpClient.SendMailAsync(message);
        }
    }
}
