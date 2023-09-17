using System.Linq.Expressions;
using AutoMapper;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Cita;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CitaController : BaseController<Cita>
{
  private readonly ICitaRepository _citaRepo;

  public CitaController(ICitaRepository citaRepo, IMapper mapper)
    : base(citaRepo, mapper)
  {
    _citaRepo = citaRepo;
  }

  [HttpGet("listarPorFechas")]
  public async Task<List<CitaCalendarDto>> GetCitasByDates(
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

    return await _citaRepo.GetCitasByDateRange(
      citaByDatesParams.StartDate,
      citaByDatesParams.EndDate,
      filter
    );
  }

  [HttpGet("listarConPaginacion")]
  public async Task<
    ActionResult<PaginatedResourceListDto<CitaCalendarDto>>
  > GetAllCitasWithPagingAsync(
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

    return await GetAllWithPagingAsync<CitaCalendarDto>(
      pageIndex,
      pageSize,
      "Medico,Paciente",
      filter,
      x => x.OrderBy(c => c.FechaInicio)
    );
  }

  [HttpGet("{id}", Name = "GetCitaDetails")]
  public async Task<ActionResult<CitaDetailsDto>> GetCitaDetailsAsync(string id)
  {
    return await GetByIdAsync<CitaDetailsDto>(
      id,
      "Medico,Paciente,Archivos,ActividadesCita.Actividad"
    );
  }

  [HttpGet("obtenerParaEditar/{id}")]
  public async Task<ActionResult<CitaRequestDto>> GetPutCitaAsyc(string id)
  {
    return await GetByIdAsync<CitaRequestDto>(id, "Medico,Paciente,ActividadesCita.Actividad");
  }

  [HttpGet("getQuarterReview/{requestedYear}")]
  public async Task<ActionResult<List<CitaRegisteredQuarterReviewDto>>> GetAnualQuarterReviewAsync(
    [FromServices] ICitaService citaService,
    int requestedYear
  )
  {
    return await citaService.GetCitasAnualQuarterReview(requestedYear);
  }

  [HttpGet("getEstadoReview/{requestedYear:int}/{estadoName}")]
  public async Task<ActionResult<List<CitaEstadoMonthReviewDto>>> GetCitasEstadoReviewAsync(
    [FromServices] ICitaService citaService,
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
    var result = await _citaRepo.UpdateEstadoCitaAsync(id, newEstado);

    return result.Succeeded ? result.SuccessResult : result.ErrorResult;
  }

  [HttpPost("crear")]
  public async Task<ActionResult> CreateCitaAsync(CitaRequestDto citaRequestDto)
  {
    var isValidCita = await _citaRepo.CheckIfCitaHoursAreValidAsync(citaRequestDto);
    if (!isValidCita)
      return BadRequest("Ya existe registrada una cita en el rango de tiempo ingresado");

    return await AddResourceAsync<CitaRequestDto, CitaDetailsDto>(citaRequestDto, "GetCitaDetails");
  }

  [HttpPut("editar/{id}")]
  public async Task<ActionResult> UpdateCitaAsync(string id, CitaRequestDto citaRequestDto)
  {
    var result = await _citaRepo.UpdateCitaAsync(id, citaRequestDto);

    return result.Succeeded ? result.SuccessResult : result.ErrorResult;
  }

  [HttpDelete("eliminar/{id}")]
  public async Task<ActionResult> DeleteCitaAsync(string id)
  {
    return await DeleteResourceAsync(id);
  }
}
