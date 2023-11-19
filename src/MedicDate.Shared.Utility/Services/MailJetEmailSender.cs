using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace MedicDate.Infrastructure.Services;

public class MailJetEmailSender(IOptions<MailJetOptions> mailJetOptions) : IEmailSender
{
  private readonly MailJetOptions _mailJetOptions = mailJetOptions.Value;

  public async Task SendEmailAsync(string email, string subject, string htmlMessage)
  {
    try
    {
      var client = new MailjetClient(_mailJetOptions.ApiKey, _mailJetOptions.SecretKey);

      // construct your email with builder
      var emailBuilder = new TransactionalEmailBuilder()
        .WithFrom(new SendContact("nelsondevmaster@medic-datepro.com", "Medic Date"))
        .WithSubject(subject)
        .WithHtmlPart(htmlMessage)
        .WithTo(new SendContact(email, "Medic Date - CIDEMAX"))
        .Build();

      var result = await client.SendTransactionalEmailAsync(emailBuilder);
      if (result.Messages is not null)
      {
        foreach (var item in result.Messages)
        {
          Console.WriteLine(item.Status);
          Console.WriteLine(item.Errors);
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }
}
