using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Clinica;
using MedicDate.Shared.Models.Common;
using MedicDate.Shared.Models.Especialidad;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClinicaController : BaseController<Clinica>
{
    public ClinicaController(IClinicaRepository repository, IMapper mapper)
        : base(repository, mapper)
    {
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
          EspecialidadResponseDto>(clinicaReq, "GetClinica");
    }

    [HttpPut("editar/{id}")]
    public async Task<ActionResult> UpdateClinicaAsync(string id,
      ClinicaRequestDto clinicaReq)
    {
        var resp =
          await .UpdateEspecialidadAsync(id,
            clinicaReq);

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
