using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Clinica;
using MedicDate.Shared.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClinicaController : BaseController<Clinica>
{
    private readonly IClinicaRepository _clinicaRepo;

    public ClinicaController(IClinicaRepository clinicaRepo, IMapper mapper)
        : base(clinicaRepo, mapper)
    {
        _clinicaRepo = clinicaRepo;
    }

    [HttpGet("listarConPaginacion")]
    public async
      Task<ActionResult<PaginatedResourceListDto<ClinicaResponseDto>>>
      GetAllClinicasPaginatedAsync(
        int pageIndex = 0,
        int pageSize = 10)
    {
        return await GetAllWithPagingAsync<ClinicaResponseDto>
        (
          pageIndex,
          pageSize,
          orderBy: x => x.OrderBy(e => e.Nombre)
        );
    }

    [HttpGet("listar")]
    public async Task<ActionResult<List<ClinicaResponseDto>>>
      GetAllClinciasAsync()
    {
        return await GetAllAsync<ClinicaResponseDto>(
          x => x.OrderBy(e => e.Nombre));
    }

    [HttpGet("{id}", Name = "GetClinica")]
    public async Task<ActionResult<ClinicaResponseDto>>
      GetClinicaAsync(string id)
    {
        return await GetByIdAsync<ClinicaResponseDto>(id);
    }

    [HttpGet("obtenerParaEditar/{id}")]
    public async Task<ActionResult<ClinicaRequestDto>>
      GetPutClinicaAsync(string id)
    {
        return await GetByIdAsync<ClinicaRequestDto>(id);
    }

    [HttpPost("crear")]
    public async Task<ActionResult> CreateClinicaAsync(
      ClinicaRequestDto clinicaReq)
    {
        return await AddResourceAsync<ClinicaRequestDto,
          ClinicaResponseDto>(clinicaReq, "GetClinica");
    }

    [HttpPut("editar/{id}")]
    public async Task<ActionResult> UpdateClinicaAsync(string id,
      ClinicaRequestDto clinicaReq)
    {
        var resp = await _clinicaRepo.UpdateClinicaAsync(id, clinicaReq);

        return resp.Succeeded
          ? resp.SuccessResult
          : resp.ErrorResult;
    }

    [HttpDelete("eliminar/{id}")]
    public async Task<ActionResult> DeleteClinicaAsync(string id)
    {
        return await DeleteResourceAsync(id);
    }

}
