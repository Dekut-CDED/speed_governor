using System.Threading.Tasks;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.EmailHelper
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<SendGridSettings> _settings;
        public EmailSender(IOptions<SendGridSettings> settings)
        {
            this._settings = settings;
        }
        public async Task SendEmailAsync(string userEmail, string emailSubject, string message)
        {
            var client = new SendGridClient(_settings.Value.Key);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("edwinkamaumuraya0@outlook.com", _settings.Value.User),
                Subject = emailSubject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(userEmail));
            msg.SetClickTracking(false, false);
            await client.SendEmailAsync(msg);
        }
    }
}