using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using AutoMapper;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Domain.Results;
using MedicDate.Shared.Models.Cita;
using MedicDate.Shared.Models.Common;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class CitaRepository : Repository<Cita>, ICitaRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;

  public CitaRepository(ApplicationDbContext context, IMapper mapper) :
      base(context)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<bool> CheckIfCitaHoursAreValidAsync(CitaRequestDto citaReq,
      string citaId = "")
  {
    if (await _context.Cita.CountAsync() == 0)
    {
      return true;
    }

    var citasConflictivas = await _context.Cita
        .AsNoTracking()
        .Where(x => (x.FechaInicio < citaReq.FechaFin && x.FechaFin > citaReq.FechaInicio)
                    &&
                    !(x.Estado == Sd.ESTADO_CITA_ANULADA
                      || x.Estado == Sd.ESTADO_CITA_CANCELADA
                      || x.Estado == Sd.ESTADO_CITA_NOASISTIOPACIENTE)
                    && x.Id != citaId)
        .ToListAsync();

    return !citasConflictivas.Any();
  }

  public async Task<List<CitaCalendarDto>> GetCitasByDateRange(
      DateTimeOffset startDate, DateTimeOffset endDate, Expression<Func<Cita, bool>>? filter = null)
  {
    var query = _context.Cita.AsQueryable();

    if (filter is not null) query = query.Where(filter);

    var citasListDb = await query.AsNoTracking()
        .Include(x => x.Paciente)
        .Include(x => x.Medico)
        .Where(x => x.FechaInicio >= startDate && x.FechaFin <= endDate)
        .ToListAsync();

    return _mapper.Map<List<CitaCalendarDto>>(citasListDb);
  }

  public async Task<OperationResult> UpdateCitaAsync(string citaId,
      CitaRequestDto citaRequestDto)
  {
    var citaDb = await FirstOrDefaultAsync(x => x.Id == citaId,
        "ActividadesCita");

    if (citaDb is null)
      return OperationResult.Error(NotFound, "No se encotró la cita para actualiar");

    var timeIsValid = await CheckIfCitaHoursAreValidAsync(citaRequestDto, citaId);

    if (!timeIsValid)
      return OperationResult
          .Error(BadRequest,
              "Ya existe registrada una cita en el rango de tiempo ingresado");

    _mapper.Map(citaRequestDto, citaDb);
    await SaveAsync();

    return OperationResult.Success(OK, "Cita actualizada con éxito");
  }

  public async Task<OperationResult> UpdateEstadoCitaAsync(string id,
      string newEstado)
  {
    var citaDb = await FirstOrDefaultAsync(x => x.Id == id,
        "ActividadesCita");

    if (citaDb is null)
      return OperationResult.Error(NotFound,
          "No se encontro la cita para actualizar");

    if (newEstado == Sd.ESTADO_CITA_COMPLETADA &&
        !citaDb.ActividadesCita.TrueForAll(x => x.ActividadTerminada))
      return OperationResult.Error(BadRequest,
          "No puede poner esta cita como completada. Debe marcar todas las actividades de la cita como terminada");

    if ((citaDb.Estado == Sd.ESTADO_CITA_ANULADA
         || citaDb.Estado == Sd.ESTADO_CITA_CANCELADA
         || citaDb.Estado == Sd.ESTADO_CITA_NOASISTIOPACIENTE)
        && (newEstado != Sd.ESTADO_CITA_ANULADA
            && newEstado != Sd.ESTADO_CITA_CANCELADA
            && newEstado != Sd.ESTADO_CITA_NOASISTIOPACIENTE))
    {
      var timeIsValid = await CheckIfCitaHoursAreValidAsync(
          new CitaRequestDto
          {
            FechaInicio = citaDb.FechaInicio.ToLocalTime().DateTime,
            FechaFin = citaDb.FechaFin.ToLocalTime().DateTime,
          }
      );

      if (!timeIsValid)
        return OperationResult
            .Error(BadRequest,
                "Ya existe registrada una cita en el rango de tiempo ingresado");
    }

    citaDb.Estado = newEstado;
    await _context.SaveChangesAsync();

    return OperationResult
        .Success(OK, "Cita actualizada correctamente");
  }

  public async Task<OperationResult> ConfirmDayBeforeEmailSendedAsync(string citaId)
  {
    var citaDb = await FirstOrDefaultAsync(x => x.Id == citaId);

    if (citaDb is null)
      return OperationResult.Error(NotFound,
                 "No se encontro la cita para actualizar");

    citaDb.EmailDayBeforeConfirm = true;
    await _context.SaveChangesAsync();

    return OperationResult
        .Success(OK, "Cita actualizada correctamente");
  }

  public async Task<OperationResult> ConfirmHoursBeforeEmailSendedAsync(string citaId)
  {
    var citaDb = await FirstOrDefaultAsync(x => x.Id == citaId);

    if (citaDb is null)
      return OperationResult.Error(NotFound,
                        "No se encontro la cita para actualizar");

    citaDb.EmailHoursBeforeConfirm = true;
    await _context.SaveChangesAsync();

    return OperationResult
        .Success(OK, "Cita actualizada correctamente");
  }
}