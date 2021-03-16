using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.EmailHelper
{
    public class EmailHelper
    {
        MailMessage mailmessage = new MailMessage();
        SmtpClient client = new SmtpClient("smtp.gmail.com");
        private readonly IConfiguration _configuration;
        public EmailHelper(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string sendEmailPasswordReset(string userEmail)
        {
            mailmessage.From = new MailAddress("edwinkamaumuraya0@gmail.com");
            mailmessage.To.Add(new MailAddress(userEmail));
            mailmessage.Subject = "Password Reset";
            mailmessage.IsBodyHtml = true;

            client.Credentials = new NetworkCredential("edwinkamaumuraya0@gmail.com", "edd0715209404k");
            client.Host = "";
            return "";
        }
    }
}