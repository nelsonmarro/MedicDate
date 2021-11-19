using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActividadController : BaseController<Actividad>
{
  private readonly IActividadRepository _actividadRepo;

  public ActividadController(IActividadRepository actividadRepo, IMapper mapper)
    : base(actividadRepo, mapper)
  {
    _actividadRepo = actividadRepo;
  }

  [HttpGet("listarConPaginacion")]
  public async
    Task<ActionResult<PaginatedResourceListDto<ActividadResponseDto>>>
    GetAllActividadesWithPagingAsync(
      int pageIndex = 0,
      int pageSize = 10
    )
  {
    return await GetAllWithPagingAsync<ActividadResponseDto>
    (
      pageIndex,
      pageSize,
      orderBy: x => x.OrderBy(ac => ac.Nombre)
    );
  }

  [HttpGet("listar")]
  public async Task<ActionResult<List<ActividadResponseDto>>>
    GetAllActividadesAsync()
  {
    return await GetAllAsync<ActividadResponseDto>(
      x => x.OrderBy(ac => ac.Nombre));
  }

  [HttpGet("{id}", Name = "GetActividad")]
  public async Task<ActionResult<ActividadResponseDto>> GetActividadAsync(
    string id)
  {
    return await GetByIdAsync<ActividadResponseDto>(id);
  }

  [HttpGet("obtenerParaEditar/{id}")]
  public async Task<ActionResult<ActividadRequestDto>> GetPutActividadAsync(
    string id)
  {
    return await GetByIdAsync<ActividadRequestDto>(id);
  }

  [HttpPost("crear")]
  public async Task<ActionResult> PostActividadAsync(
    ActividadRequestDto actividadReq)
  {
    return await AddResourceAsync<ActividadRequestDto, ActividadResponseDto>(
      actividadReq, "GetActividad");
  }

  [HttpPut("editar/{id}")]
  public async Task<ActionResult> PutActividadAsync(string id,
    ActividadRequestDto actividadReq)
  {
    var resp = await _actividadRepo.UpdateActividadAsync(id, actividadReq);

    return resp.Succeeded
      ? resp.SuccessResult
      : resp.ErrorResult;
  }

  [HttpDelete("eliminar/{id}")]
  public async Task<ActionResult> DeleteActividadAsync(string id)
  {
    return await DeleteResourceAsync(id);
  }
}