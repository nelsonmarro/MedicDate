using AutoMapper;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Common;
using MedicDate.Shared.Models.Medico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MedicoController : BaseController<Medico>
{
    private readonly IMapper _mapper;
    private readonly IMedicoRepository _medicoRepo;

    public MedicoController(
      IMedicoRepository medicoRepo,
      IMapper mapper
    )
      : base(medicoRepo, mapper)
    {
        _medicoRepo = medicoRepo;
        _mapper = mapper;
    }

    [HttpGet("listarConPaginacion")]
    public async
      Task<ActionResult<PaginatedResourceListDto<MedicoResponseDto>>>
      GetAllWithPagingAsync(
        int pageIndex = 0,
        int pageSize = 10,
        [FromQuery] bool traerEspecialidades = false,
        [FromQuery] string? filtrarEspecialidadId = null
      )
    {
        var includeProperties = traerEspecialidades
          ? "MedicosEspecialidades.Especialidad"
          : "";

        if (!string.IsNullOrEmpty(filtrarEspecialidadId))
            return await GetAllWithPagingAsync<MedicoResponseDto>
            (
              pageIndex,
              pageSize,
              includeProperties,
              m => m.MedicosEspecialidades
                .Any(me => me.EspecialidadId == filtrarEspecialidadId),
              x => x.OrderBy(m => m.Nombre)
            );

        return await GetAllWithPagingAsync<MedicoResponseDto>
        (
          pageIndex,
          pageSize,
          includeProperties,
          null,
          x => x.OrderBy(m => m.Nombre)
        );
    }

    [HttpGet("listar")]
    public async Task<ActionResult<List<MedicoCitaResponseDto>>>
      GetAllMedicosAsync()
    {
        return await GetAllAsync<MedicoCitaResponseDto>
          (x => x.OrderBy(p => p.Nombre));
    }

    [HttpGet("{id}", Name = "GetMedico")]
    public async Task<ActionResult<MedicoResponseDto>> GetMedicoByIdAsync(
      string id,
      [FromQuery] bool traerEspecialidades = false)
    {
        var includeProperties = traerEspecialidades
          ? "MedicosEspecialidades.Especialidad"
          : string.Empty;

        return await GetByIdAsync<MedicoResponseDto>(id, includeProperties);
    }

    [HttpGet("obtenerParaEditar/{id}")]
    public async Task<ActionResult<MedicoRequestDto>> GetPutMedicoAsync(
      string id)
    {
        return await GetByIdAsync<MedicoRequestDto>(id,
          "MedicosEspecialidades");
    }

    [HttpPost("crear")]
    public async Task<ActionResult> PostAsync(
      MedicoRequestDto medicoRequestDto,
      [FromServices] IMedicoService medicoService)
    {
        if (await medicoService.ValidatCedulaForCreateAsync(medicoRequestDto
              .Cedula))
            return BadRequest(
              "Ya existe otro doctor registrado con el número de cédula que ingresó");

        return await AddResourceAsync<MedicoRequestDto, MedicoResponseDto>(
          medicoRequestDto, "GetMedico");
    }

    [HttpPut("editar/{id}")]
    public async Task<ActionResult> PutAsync(string id,
      MedicoRequestDto medicoRequestDto,
      [FromServices] IMedicoService medicoService)
    {
        if (await medicoService
              .ValidateCedulaForEditAsync(medicoRequestDto.Cedula, id))
            return BadRequest(
              "Ya existe otro doctor registrado con el número de cédula que ingresó");

        var resp =
          await _medicoRepo.UpdateMedicoAsync(id, medicoRequestDto);

        return resp.Succeeded
          ? resp.SuccessResult
          : resp.ErrorResult;
    }

    [HttpDelete("eliminar/{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        return await DeleteResourceAsync(id);
    }
}