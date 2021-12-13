using AutoMapper;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Common;
using MedicDate.Shared.Models.Paciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PacienteController : BaseController<Paciente>
{
   private readonly IPacienteRepository _pacienteRepo;

   public PacienteController(IPacienteRepository pacienteRepo
     , IMapper mapper)
     : base(pacienteRepo, mapper)
   {
      _pacienteRepo = pacienteRepo;
   }

   [HttpGet("listarConPaginacion")]
   public async
     Task<ActionResult<PaginatedResourceListDto<PacienteResponseDto>>>
     GetAllPacientesWithPaging(
       int pageIndex = 0,
       int pageSize = 10,
       [FromQuery] bool traerGrupos = false,
       [FromQuery] string? filtrarGrupoId = null)
   {
      var includeProperties = traerGrupos
        ? "GruposPacientes.Grupo"
        : "";

      if (!string.IsNullOrEmpty(filtrarGrupoId))
         return await GetAllWithPagingAsync<PacienteResponseDto>
         (
           pageIndex,
           pageSize,
           includeProperties,
           p => p.GruposPacientes
             .Any(gp => gp.GrupoId == filtrarGrupoId),
           x => x.OrderBy(p => p.Nombres)
         );

      return await GetAllWithPagingAsync<PacienteResponseDto>
      (
        pageIndex,
        pageSize,
        includeProperties,
        null,
        x => x.OrderBy(p => p.Nombres)
      );
   }

   [HttpGet("listar")]
   public async Task<ActionResult<List<PacienteCitaResponseDto>>>
     GetAllPacienteAsync()
   {
      return await GetAllAsync<PacienteCitaResponseDto>
        (x => x.OrderBy(p => p.Nombres));
   }

   [HttpGet("getAnualMonthReview/{requestedYear}")]
   public async Task<ActionResult<List<PacienteMonthReviewDto>>> GetPacienteAnualMonthReviewAsync([FromServices] IPacienteService pacienteService, int requestedYear)
   {
      return await pacienteService.GetPacientesMonthRegisterationReview(requestedYear);
   }

   [HttpGet("{id}", Name = "GetPaciente")]
   public async Task<ActionResult<PacienteResponseDto>> GetPacienteAsync(
     string id)
   {
      return await GetByIdAsync<PacienteResponseDto>(id
        , "GruposPacientes.Grupo");
   }

   [HttpGet("obtenerParaEditar/{id}")]
   public async Task<ActionResult<PacienteRequestDto>> GetPutPacienteAsync(
     string id)
   {
      return await GetByIdAsync<PacienteRequestDto>(id
        , "GruposPacientes");
   }

   [HttpPost("crear")]
   public async Task<ActionResult> PostPacienteAsync(
     PacienteRequestDto pacienteRequestDto
     , [FromServices] IPacienteService pacienteService)
   {
      var result = await pacienteService.ValidatePacienteForCreate(
        pacienteRequestDto.NumHistoria, pacienteRequestDto.Cedula);

      if (!result.Succeeded) return result.ErrorResult;

      return await
        AddResourceAsync<PacienteRequestDto, PacienteResponseDto>(
          pacienteRequestDto, "GetPaciente");
   }

   [HttpPut("editar/{id}")]
   public async Task<ActionResult> PutPacienteAsync(string id
     , PacienteRequestDto pacienteRequestDto
     , [FromServices] IPacienteService pacienteService)
   {
      var result = await pacienteService.ValidatePacienteForEdit(
        pacienteRequestDto.NumHistoria, pacienteRequestDto.Cedula, id);

      if (!result.Succeeded) return result.ErrorResult;

      var resp =
        await _pacienteRepo.UpdatePacienteAsync(id, pacienteRequestDto);

      return resp.Succeeded
        ? resp.SuccessResult
        : resp.ErrorResult;
   }

   [HttpDelete("eliminar/{id}")]
   public async Task<ActionResult> DeletePacienteAsync(string id)
   {
      return await DeleteResourceAsync(id);
   }
}