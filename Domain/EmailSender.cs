using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Domain
{
    public class EmailSender : IEmailSender
    {

        MailMessage mailmessage = new MailMessage();
        private readonly string _apikey;
        private readonly SmtpClient client;
        public EmailSender(IConfiguration config, SmtpClient client)
        {
            this.client = client;
            _apikey = config["SendGridApiKey"];
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //using send grid
            mailmessage.From = new MailAddress("edwinkamaumuraya0@gmail.com");
            mailmessage.To.Add(new MailAddress(email));
            mailmessage.Subject = subject;
            mailmessage.Body = htmlMessage;
            mailmessage.IsBodyHtml = true;
            client.Send(mailmessage);
            return Task.CompletedTask;
        }
    }
}
