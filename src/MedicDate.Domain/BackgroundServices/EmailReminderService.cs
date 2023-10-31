using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Domain.DomainServices.IDomainServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MedicDate.Domain.BackgroundServices;

public class EmailReminderService(IServiceProvider serviceProvider) : BackgroundService
{
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    using var scope = serviceProvider.CreateScope();
    var citaRepository = scope.ServiceProvider.GetRequiredService<ICitaRepository>();
    var citaService = scope.ServiceProvider.GetRequiredService<ICitaService>();

    while (!stoppingToken.IsCancellationRequested)
    {
      // Para enviar un recordatorio 1 día antes
      var oneDayBeforeCitas = await citaRepository.GetAllAsync(
        c => c.FechaInicio <= DateTime.Now.AddHours(24) && c.FechaInicio > DateTime.Now,
        includeProperties: "Medico,Paciente,ActividadesCita.Actividad"
      );

      if (oneDayBeforeCitas.Count != 0)
      {
        foreach (var cita in oneDayBeforeCitas)
        {
          await citaService.SendReminderEmailAsync(new() { Cita = cita });
        }
      }

      // Para enviar un recordatorio 2 horas antes
      var twoHoursBeforeCitas = await citaRepository.GetAllAsync(
        c => c.FechaInicio <= DateTime.Now.AddHours(2) && c.FechaInicio > DateTime.Now,
        includeProperties: "Medico,Paciente,ActividadesCita.Actividad"
      );

      if (twoHoursBeforeCitas.Count != 0)
      {
        foreach (var cita in twoHoursBeforeCitas)
        {
          await citaService.SendReminderEmailAsync(new() { Cita = cita });
        }
      }

      await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Intervalo de verificación
    }
  }
}
