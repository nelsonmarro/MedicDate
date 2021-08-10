using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Paciente;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : BaseController<Paciente>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PacienteController(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<PacienteResponse>>> GetAllPacientesWithPaging(
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerGrupos = false,
            [FromQuery] string filtrarGrupoId = null)
        {
            if (!string.IsNullOrEmpty(filtrarGrupoId))
            {
                return await GetAllWithPagingAsync<PacienteResponse>
                (
                    pageIndex,
                    pageSize,
                    traerGrupos,
                    "GruposPacientes.Grupo",
                    p => p.GruposPacientes
                        .Any(gp => gp.GrupoId == filtrarGrupoId),
                    x => x.OrderBy(p => p.Nombres)
                );
            }

            return await GetAllWithPagingAsync<PacienteResponse>
            (
                pageIndex,
                pageSize,
                traerGrupos,
                "GruposPacientes.Grupo",
                null,
                x => x.OrderBy(p => p.Nombres)
            );
        }

        [HttpGet("{id}", Name = "GetPaciente")]
        public async Task<ActionResult<PacienteResponse>> GetPacienteAsync(string id)
        {
            return await GetByIdAsync<PacienteResponse>(id, true, "GruposPacientes.Grupo");
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<PacienteRequest>> GetPutPacienteAsync(string id)
        {
            return await GetByIdAsync<PacienteRequest>(id, true, "GruposPacientes");
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostPacienteAsync(PacienteRequest pacienteRequest)
        {
            if (await _unitOfWork.PacienteRepo
                .CheckNumHistoriaExistsAsync(pacienteRequest.NumHistoria))
            {
                return BadRequest("Ya existe otro paciente registrado con el número de historia que ingresó");
            }

            var response = await ControllerEntityValidator
                .ValidateAsync(_unitOfWork.PacienteRepo, cedula: pacienteRequest.Cedula,
                    entityIds: pacienteRequest.GruposId);

            if (!response.IsSuccess) return response.ErrorActionResult;

            return await AddResourceAsync<PacienteRequest, PacienteResponse>(pacienteRequest, "GetPaciente");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutPacienteAsync(string id, PacienteRequest pacienteRequest)
        {
            var resp = await _unitOfWork.PacienteRepo.UpdatePacienteAsync(id, pacienteRequest);

            return resp.IsSuccess
                ? Ok("Paciente actualizado con éxito")
                : resp.ErrorActionResult;
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeletePacienteAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}