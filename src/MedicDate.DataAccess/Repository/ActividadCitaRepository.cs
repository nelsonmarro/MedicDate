using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Actividad;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class ActividadCitaRepository : IActividadCitaRepository
{
  private readonly ApplicationDbContext _context;

  public ActividadCitaRepository(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<OperationResult> UpdateActividadCitaAsync(string citaId,
    ActividadCitaRequestDto actividadCita)
  {
    var actividadCitaDb = await _context.ActividadCita
      .Where(x =>
        x.CitaId == citaId && x.ActividadId == actividadCita.ActividadId)
      .FirstOrDefaultAsync();

    if (actividadCitaDb is null)
      return OperationResult.Error(NotFound,
        "No se pudo actualizar la actividad seleccionada");

    actividadCitaDb.ActividadTerminada = actividadCita.ActividadTerminada;
    actividadCitaDb.Detalles = actividadCita.Detalles;

    await _context.SaveChangesAsync();
    return OperationResult.Success(OK, "Actividad actualizada correctamente");
  }
}