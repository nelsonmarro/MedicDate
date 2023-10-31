using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using MedicDate.DataAccess.Helpers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace MedicDate.Domain.ApplicationServices;

public class MailJetEmailSender(IOptions<MailJetOptions> mailJetOptions) : IEmailSender
{
  private readonly MailJetOptions _mailJetOptions = mailJetOptions.Value;

  public async Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    var client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey);

    // construct your email with builder
    var emailBuilder = new TransactionalEmailBuilder()
      .WithFrom(new SendContact("nelsondevmaster@medic-datepro.com", "Medic Date"))
      .WithSubject(subject)
      .WithHtmlPart(htmlMessage)
      .WithTo(new SendContact(email, "Medic Date - CIDEMAX"))
      .Build();

    await client.SendTransactionalEmailAsync(emailBuilder);
  }
}
