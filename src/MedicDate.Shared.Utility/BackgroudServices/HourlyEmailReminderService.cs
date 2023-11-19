using System.Globalization;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Domain.Services.IDomainServices;
using MedicDate.Shared.Models.Cita;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MedicDate.Infrastructure.BackgroundServices;

public class HourlyEmailReminderService(IServiceProvider serviceProvider) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      using var scope = serviceProvider.CreateScope();
      var citaRepository = scope.ServiceProvider.GetRequiredService<ICitaRepository>();
      var citaService = scope.ServiceProvider.GetRequiredService<ICitaService>();

      while (!stoppingToken.IsCancellationRequested)
      {
        // Para enviar un recordatorio 2 horas antes
        var twoHoursBeforeCitas = await citaRepository.GetAllAsync(
          c => c.FechaInicio <= DateTime.Now.AddHours(2) 
          && c.FechaInicio > DateTime.Now
          && !c.EmailHoursBeforeConfirm,
          includeProperties: "Medico,Paciente,ActividadesCita.Actividad"
        );

        if (twoHoursBeforeCitas.Count != 0)
        {
          foreach (var cita in twoHoursBeforeCitas)
          {
            await citaService.SendReminderEmailAsync(new CitaReminderDto
            {
              AppointmentDate = cita.FechaInicio.ToString("dddd, d 'de' MMMM 'de' yyyy. h:mm tt",
              new CultureInfo("es-ES")),
              DoctorEmail = cita.Medico.Email,
              DoctorInfo = $"{cita.Medico.Nombre} {cita.Medico.Apellidos}. Contacto: {cita.Medico.PhoneNumber}",
              PatientEmail = cita.Paciente.Email,
              PatientInfo = $"{cita.Paciente.Nombres} {cita.Paciente.Apellidos}. CI: {cita.Paciente.Cedula}",
              Treatments = cita.ActividadesCita.Select(a => a.Actividad.Nombre).ToList()
            });
            await citaRepository.ConfirmHoursBeforeEmailSendedAsync(cita.Id);
          }
        }
        await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Intervalo de verificación
      }
    }
    catch (Exception e)
    {
      //UnWrap aggregate exceptions
      if (e is AggregateException aggregateException)
        foreach (var innerException in aggregateException.Flatten().InnerExceptions)
          Console.WriteLine(innerException + "\nOne or many tasks failed.");
      else
        Console.WriteLine(e + "\nException executing tasks.");
    }
  }
}
