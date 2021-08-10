using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using MedicDate.API.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace MedicDate.Bussines.Services
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private MailJetOptions _mailJetOptions;

        public MailJetEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _mailJetOptions = _configuration.GetSection("MailJet").Get<MailJetOptions>();

            var client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey);
            var request = new MailjetRequest
            {
                Resource = Send.Resource
            };

            // construct your email with builder
            var emailBuilder = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("nelsondev99@protonmail.com", "Nelson Marro"))
                .WithSubject(subject)
                .WithHtmlPart(htmlMessage)
                .WithTo(new SendContact(email, "X-Development"))
                .Build();

            var response = await client.SendTransactionalEmailAsync(emailBuilder);
        }
    }
}