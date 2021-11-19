using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using MedicDate.DataAccess.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace MedicDate.Bussines.ApplicationServices;

public class MailJetEmailSender : IEmailSender
{
  private readonly MailJetOptions _mailJetOptions;

  public MailJetEmailSender(IOptions<MailJetOptions> mailJetOptions)
  {
    _mailJetOptions = mailJetOptions.Value;
  }

  public async Task SendEmailAsync(string email, string subject,
    string htmlMessage)
  {
    var client =
      new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey);
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