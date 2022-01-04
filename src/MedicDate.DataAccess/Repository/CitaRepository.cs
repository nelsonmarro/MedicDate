using System.Linq.Expressions;
using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Cita;
using MedicDate.Utility;
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

    public async Task<bool> CheckIfCitaHoursAreValidAsync(CitaRequestDto citaReq)
    {
        if (await _context.Cita.CountAsync() == 0)
        {
            return true;
        }

        return await _context.Cita
           .AsNoTracking()
           .AnyAsync(c =>
        (c.FechaFin != citaReq.FechaFin
        && c.FechaFin != citaReq.FechaInicio
        && c.FechaInicio != citaReq.FechaInicio
        && c.FechaInicio != citaReq.FechaFin)
        ||
        (c.Estado == Sd.ESTADO_CITA_ANULADA
        || c.Estado == Sd.ESTADO_CITA_CANCELADA
        || c.Estado == Sd.ESTADO_CITA_NOASISTIOPACIENTE));
    }

    public async Task<bool> CheckIfCitaHoursAreValidAsync(CitaRequestDto citaReq, string citaId)
    {
        if (await _context.Cita
           .AsNoTracking()
           .AnyAsync(x => x.FechaFin == citaReq.FechaFin
        && x.FechaInicio == citaReq.FechaInicio && citaId == x.Id))
        {
            return true;
        }

        return await CheckIfCitaHoursAreValidAsync(citaReq);
    }

    public async Task<List<CitaCalendarDto>> GetCitasByDateRange(
      DateTime startDate, DateTime endDate, Expression<Func<Cita, bool>>? filter = null)
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
            return OperationResult.
               Error(NotFound, "No se encotró la cita para actualiar");

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

        if (newEstado == Sd.ESTADO_CITA_COMPLETADA && !citaDb.ActividadesCita.TrueForAll(x => x.ActividadTerminada))
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
                   FechaInicio = citaDb.FechaInicio,
                   FechaFin = citaDb.FechaFin,
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
}