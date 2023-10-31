using System.Linq.Expressions;
using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Domain.DomainServices.IDomainServices;
using MedicDate.Shared.Models.Cita;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CitaController(ICitaRepository citaRepo, IMapper mapper, ICitaService citaService)
  : BaseController<Cita>(citaRepo, mapper)
{
  [HttpGet("listarPorFechas")]
  public Task<List<CitaCalendarDto>> GetCitasByDates(
    [FromQuery] CitaByDatesParams citaByDatesParams,
    [FromQuery] string? medicoId = null,
    [FromQuery] string? pacienteId = null
  )
  {
    Expression<Func<Cita, bool>>? filter = null;

    if (medicoId is not null && pacienteId is null)
      filter = c => c.MedicoId == medicoId;

    if (pacienteId is not null && medicoId is null)
      filter = c => c.PacienteId == pacienteId;

    if (pacienteId is not null && medicoId is not null)
      filter = c => c.PacienteId == pacienteId && c.MedicoId == medicoId;

    return citaRepo.GetCitasByDateRange(
      citaByDatesParams.StartDate,
      citaByDatesParams.EndDate,
      filter
    );
  }

  [HttpGet("listarConPaginacion")]
  public Task<ActionResult<PaginatedResourceListDto<CitaCalendarDto>>> GetAllCitasWithPagingAsync(
    int pageIndex = 0,
    int pageSize = 10,
    [FromQuery] string? medicoId = null,
    [FromQuery] string? pacienteId = null
  )
  {
    Expression<Func<Cita, bool>>? filter = null;

    if (medicoId is not null && pacienteId is null)
      filter = c => c.MedicoId == medicoId;

    if (pacienteId is not null && medicoId is null)
      filter = c => c.PacienteId == pacienteId;

    if (pacienteId is not null && medicoId is not null)
      filter = c => c.PacienteId == pacienteId && c.MedicoId == medicoId;

    return GetAllWithPagingAsync<CitaCalendarDto>(
      pageIndex,
      pageSize,
      "Medico,Paciente",
      filter,
      x => x.OrderBy(c => c.FechaInicio)
    );
  }

  [HttpGet("{id}", Name = "GetCitaDetails")]
  public Task<ActionResult<CitaDetailsDto>> GetCitaDetailsAsync(string id)
  {
    return GetByIdAsync<CitaDetailsDto>(id, "Medico,Paciente,Archivos,ActividadesCita.Actividad");
  }

  [HttpGet("obtenerParaEditar/{id}")]
  public Task<ActionResult<CitaRequestDto>> GetPutCitaAsyc(string id)
  {
    return GetByIdAsync<CitaRequestDto>(id, "Medico,Paciente,ActividadesCita.Actividad");
  }

  [HttpGet("getQuarterReview/{requestedYear}")]
  public async Task<ActionResult<List<CitaRegisteredQuarterReviewDto>>> GetAnualQuarterReviewAsync(
    int requestedYear
  )
  {
    return await citaService.GetCitasAnualQuarterReview(requestedYear);
  }

  [HttpGet("getEstadoReview/{requestedYear:int}/{estadoName}")]
  public async Task<ActionResult<List<CitaEstadoMonthReviewDto>>> GetCitasEstadoReviewAsync(
    string estadoName,
    int requestedYear
  )
  {
    return await citaService.GetCitasMonthReviewByEstado(estadoName, requestedYear);
  }

  [HttpPut("actualizarEstado/{id}")]
  public async Task<ActionResult> UpdateEstadoCitaAsync(
    [FromRoute] string id,
    [FromBody] string newEstado
  )
  {
    var result = await citaRepo.UpdateEstadoCitaAsync(id, newEstado);

    return result.Succeeded ? result.SuccessResult : result.ErrorResult;
  }

  [HttpPost("crear")]
  public async Task<ActionResult> CreateCitaAsync(CitaRequestDto citaRequestDto)
  {
    var isValidCita = await citaRepo.CheckIfCitaHoursAreValidAsync(citaRequestDto);
    if (!isValidCita)
      return BadRequest("Ya existe registrada una cita en el rango de tiempo ingresado");

    var createdRecord = await CreateRecordAsync<CitaRequestDto, CitaDetailsDto>(citaRequestDto);

    await citaService.SendRegisterationEmailAsync(new() { AppoimentId = createdRecord.Id });
    return CreatedAtRoute("GetCitaDetails", new { id = createdRecord.Id }, createdRecord);
  }

  [HttpPut("editar/{id}")]
  public async Task<ActionResult> UpdateCitaAsync(string id, CitaRequestDto citaRequestDto)
  {
    var result = await citaRepo.UpdateCitaAsync(id, citaRequestDto);

    return result.Succeeded ? result.SuccessResult : result.ErrorResult;
  }

  [HttpDelete("eliminar/{id}")]
  public Task<ActionResult> DeleteCitaAsync(string id)
  {
    return DeleteResourceAsync(id);
  }
}
