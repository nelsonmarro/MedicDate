using AutoMapper;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Shared.Models.Common;
using MedicDate.Shared.Models.Grupo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class GrupoController : BaseController<Grupo>
{
  private readonly IGrupoRepository _grupoRepo;

  public GrupoController(IGrupoRepository grupoRepo, IMapper mapper)
    : base(grupoRepo, mapper)
  {
    _grupoRepo = grupoRepo;
  }

  [HttpGet("listarConPaginacion")]
  public async Task<
    ActionResult<PaginatedResourceListDto<GrupoResponseDto>>
  > GetAllGruposWithPagingAsync(int pageIndex = 0, int pageSize = 10)
  {
    return await GetAllWithPagingAsync<GrupoResponseDto>(
      pageIndex,
      pageSize,
      orderBy: x => x.OrderBy(g => g.Nombre)
    );
  }

  [HttpGet("listar")]
  public async Task<ActionResult<List<GrupoResponseDto>>> GetAllGruposAsync()
  {
    return await GetAllAsync<GrupoResponseDto>(x => x.OrderBy(g => g.Nombre));
  }

  [HttpGet("{id}", Name = "GetGrupo")]
  public async Task<ActionResult<GrupoResponseDto>> GetGrupoAsync(string id)
  {
    return await GetByIdAsync<GrupoResponseDto>(id);
  }

  [HttpGet("obtenerParaEditar/{id}")]
  public async Task<ActionResult<GrupoRequestDto>> GetPutGrupoAsync(string id)
  {
    return await GetByIdAsync<GrupoRequestDto>(id);
  }

  [HttpPost("crear")]
  public async Task<ActionResult> PostGrupoAsync(GrupoRequestDto grupoRequestDto)
  {
    return await AddResourceAsync<GrupoRequestDto, GrupoResponseDto>(grupoRequestDto, "GetGrupo");
  }

  [HttpPut("editar/{id}")]
  public async Task<ActionResult> PutGrupoAsync(string id, GrupoRequestDto grupoRequestDto)
  {
    var resp = await _grupoRepo.UpdateGrupoAsync(id, grupoRequestDto);

    return resp.Succeeded ? resp.SuccessResult : resp.ErrorResult;
  }

  [HttpDelete("eliminar/{id}")]
  public async Task<ActionResult> DeleteGrupoAsync(string id)
  {
    return await DeleteResourceAsync(id);
  }
}
