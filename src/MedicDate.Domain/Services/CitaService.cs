using System.Globalization;
using Fluid;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Domain.Services.IDomainServices;
using MedicDate.Shared.Models.Cita;
using Microsoft.AspNetCore.Identity.UI.Services;
using AutoMapper;
using MedicDate.Shared.Models.Common;

namespace MedicDate.Domain.Services;

public class CitaService(
  ICitaRepository citaRepo,
  IEntityValidator entityValidator,
  IEmailSender emailSender,
  IMapper mapper
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

  public async Task SendRegistrationEmailAsync(string AppointmentId)
  {
    // Get email content
    var cita = await citaRepo.FirstOrDefaultAsync(
      x => x.Id == AppointmentId,
      "Medico,Paciente,ActividadesCita.Actividad"
    );

    if (cita is null)
      throw new InvalidOperationException("No se encontró la cita");

    var emailContent = mapper.Map<CitaEmailContent>(cita);

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

  public async Task SendReminderEmailAsync(CitaReminderDto cita)
  {
    var emailContent = new CitaEmailContent
    {
      AppointmentDate = cita.AppointmentDate,
      DoctorInfo = cita.DoctorInfo,
      PatientInfo = cita.PatientInfo,
      Treatments = cita.Treatments
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
      cita.PatientEmail,
      "Cita registrada. CIDEMAX",
      await template.RenderAsync(context)
    );

    await emailSender.SendEmailAsync(
      cita.DoctorEmail,  
      "Cita registrada. CIDEMAX",
      await template.RenderAsync(context)
    );
  }
}
