using AutoMapper;
using MedicDate.API.DTOs.Common;
using MedicDate.API.DTOs.Paciente;
using MedicDate.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.DataAccess.Repository.IRepository;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : BaseController<Paciente>
    {
        private readonly IPacienteRepository _pacienteRepo;

        public PacienteController(IPacienteRepository pacienteRepo, IMapper mapper)
            : base(pacienteRepo, mapper)
        {
            _pacienteRepo = pacienteRepo;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<PaginatedResourceListDto<PacienteResponseDto>>> GetAllPacientesWithPaging(
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerGrupos = false,
            [FromQuery] string filtrarGrupoId = null)
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

        [HttpGet("{id}", Name = "GetPaciente")]
        public async Task<ActionResult<PacienteResponseDto>> GetPacienteAsync(string id)
        {
            return await GetByIdAsync<PacienteResponseDto>(id, "GruposPacientes.Grupo");
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<PacienteRequestDto>> GetPutPacienteAsync(string id)
        {
            return await GetByIdAsync<PacienteRequestDto>(id, "GruposPacientes");
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostPacienteAsync(PacienteRequestDto pacienteRequestDto)
        {
            if (await _pacienteRepo
                .CheckNumHistoriaExistsAsync(pacienteRequestDto.NumHistoria))
                return BadRequest("Ya existe otro paciente registrado con el número de historia que ingresó");


            if (await _pacienteRepo
                .CheckCedulaExistsForCreateAsync(pacienteRequestDto.Cedula))
                return BadRequest("Ya existe otro paciente registrado con el número de cédula que ingresó");

            if (!await _pacienteRepo
                .CheckRelatedEntityIdsExistsAsync(pacienteRequestDto.GruposId))
                return BadRequest("No existe uno de los grupos asignados");


            return await AddResourceAsync<PacienteRequestDto, PacienteResponseDto>(pacienteRequestDto, "GetPaciente");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutPacienteAsync(string id, PacienteRequestDto pacienteRequestDto)
        {
            var resp = await _pacienteRepo.UpdatePacienteAsync(id, pacienteRequestDto);

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
}