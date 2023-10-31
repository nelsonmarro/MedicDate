using System.Globalization;
using Fluid;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Domain.ApplicationServices.IApplicationServices;
using MedicDate.Domain.DomainServices.IDomainServices;
using MedicDate.Domain.Models;
using MedicDate.Shared.Models.Cita;
using MedicDate.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MedicDate.Domain.DomainServices;

public class CitaService(
  ICitaRepository citaRepo,
  IEntityValidator entityValidator,
  IEmailSender emailSender
) : ICitaService
{
  private readonly IEntityValidator _entityValidator = entityValidator;

  public async Task<List<CitaRegisteredQuarterReviewDto>> GetCitasAnualQuarterReview(
    int requestedYear
  )
  {
    return new List<CitaRegisteredQuarterReviewDto>
    {
      new()
      {
        Quarter = "T1",
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.Januay
            && x.FechaInicio < Sd.April
            && x.FechaInicio.Year == requestedYear
        )
      },
      new()
      {
        Quarter = "T2",
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.April
            && x.FechaInicio < Sd.July
            && x.FechaInicio.Year == requestedYear
        )
      },
      new()
      {
        Quarter = "T3",
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.July
            && x.FechaInicio < Sd.October
            && x.FechaInicio.Year == requestedYear
        )
      },
      new()
      {
        Quarter = "T4",
        TotalCitas = await citaRepo.CountResourcesAsync(
          x => x.FechaInicio >= Sd.October && x.FechaInicio.Year == requestedYear
        )
      }
    };
  }

  public async Task<List<CitaEstadoMonthReviewDto>> GetCitasMonthReviewByEstado(
    string estadoName,
    int requestedYear
  )
  {
    return new List<CitaEstadoMonthReviewDto>
    {
      new()
      {
        RegisterationDate = Sd.Januay,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.Januay
            && x.FechaInicio < Sd.February
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.February,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.February
            && x.FechaInicio < Sd.March
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.March,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.March
            && x.FechaInicio < Sd.April
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.April,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.April
            && x.FechaInicio < Sd.May
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.May,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.May
            && x.FechaInicio < Sd.June
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.June,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.June
            && x.FechaInicio < Sd.July
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.July,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.July
            && x.FechaInicio < Sd.August
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.August,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.August
            && x.FechaInicio < Sd.September
            && x.FechaInicio.Year == requestedYear
        )
      },
      new()
      {
        RegisterationDate = Sd.September,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.September
            && x.FechaInicio < Sd.October
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.October,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.October
            && x.FechaInicio < Sd.November
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.November,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.November
            && x.FechaInicio < Sd.December
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      },
      new()
      {
        RegisterationDate = Sd.December,
        NombreEstado = estadoName,
        TotalCitas = await citaRepo.CountResourcesAsync(
          x =>
            x.FechaInicio >= Sd.December
            && x.FechaInicio.Year == requestedYear
            && estadoName == x.Estado
        )
      }
    };
  }

  public async Task SendRegisterationEmailAsync(SendRegisteredAppoimentEmailRequest request)
  {
    // Get email content
    var cita = await citaRepo.FirstOrDefaultAsync(
      x => x.Id == request.AppoimentId,
      "Medico,Paciente,ActividadesCita.Actividad"
    );

    if (cita is null)
      throw new InvalidOperationException("No se encontro la cita");

    var emailContent = new AppoimentEmailContent
    {
      AppointmentDate = cita.FechaInicio.ToString(
        "dddd, d 'de' MMMM 'de' yyyy. h:mm tt",
        new CultureInfo("es-ES")
      ),
      DoctorInfo =
        $"{cita.Medico.Nombre} {cita.Medico.Apellidos}. Contacto: {cita.Medico.PhoneNumber}",
      PacientInfo =
        $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}. CI: {cita.Paciente.Cedula}",
      Treatments = cita.ActividadesCita.Select(x => x.Actividad.Nombre).ToList()
    };

    var parser = new FluidParser();

    // read the html template file from wwwrot/templates
    var templatePath = Path.Combine(
      Directory.GetCurrentDirectory(),
      "wwwroot",
      "templates",
      "appoiment-registered.email.html"
    );

    // read the content
    var source = await File.ReadAllTextAsync(templatePath);

    if (!parser.TryParse(source, out var template, out var error))
      throw new Exception($"Error: {error}");

    var context = new TemplateContext(emailContent);

    await emailSender.SendEmailAsync(
      cita.Paciente.Email,
      "Cita registrada. CIDEMAX",
      await template.RenderAsync(context)
    );

    await emailSender.SendEmailAsync(
      cita.Medico.Email,
      "Cita registrada. CIDEMAX",
      await template.RenderAsync(context)
    );
  }

  public async Task SendReminderEmailAsync(SendAppoimentReminderEmailRequest request)
  {
    var cita = request.Cita;

    var emailContent = new AppoimentEmailContent
    {
      AppointmentDate = cita.FechaInicio.ToString(
        "dddd, d 'de' MMMM 'de' yyyy. h:mm tt",
        new CultureInfo("es-ES")
      ),
      DoctorInfo =
        $"{cita.Medico.Nombre} {cita.Medico.Apellidos}. Contacto: {cita.Medico.PhoneNumber}",
      PacientInfo =
        $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}. CI: {cita.Paciente.Cedula}",
      Treatments = cita.ActividadesCita.Select(x => x.Actividad.Nombre).ToList()
    };

    var parser = new FluidParser();

    // read the html template file from wwwrot/templates
    var templatePath = Path.Combine(
      Directory.GetCurrentDirectory(),
      "wwwroot",
      "templates",
      "appoiment-reminder.email.html"
    );

    // read the content
    var source = await File.ReadAllTextAsync(templatePath);

    if (!parser.TryParse(source, out var template, out var error))
      throw new Exception($"Error: {error}");

    var context = new TemplateContext(emailContent);

    await emailSender.SendEmailAsync(
      cita.Paciente.Email,
      "Cita registrada. CIDEMAX",
      await template.RenderAsync(context)
    );

    await emailSender.SendEmailAsync(
      cita.Medico.Email,
      "Cita registrada. CIDEMAX",
      await template.RenderAsync(context)
    );
  }
}
