using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.API.DTOs;
using MedicDate.API.DTOs.Common;
using MedicDate.API.DTOs.Medico;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : BaseController<Medico>
    {
        private readonly IMedicoRepository _medicoRepo;
        private readonly IMapper _mapper;

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
        public async Task<ActionResult<PaginatedResourceListDto<MedicoResponseDto>>> GetAllWithPagingAsync(
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerEspecialidades = false,
            [FromQuery] string filtrarEspecialidadId = null
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

        [HttpGet("{id}", Name = "GetMedico")]
        public async Task<ActionResult<MedicoResponseDto>> GetMedicoByIdAsync(string id,
            [FromQuery] bool traerEspecialidades = false)
        {
            var includeProperties = traerEspecialidades
                ? "MedicosEspecialidades.Especialidad"
                : string.Empty;

            return await GetByIdAsync<MedicoResponseDto>(id, includeProperties);
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<MedicoRequestDto>> GetPutMedicoAsync(string id)
        {
            return await GetByIdAsync<MedicoRequestDto>(id, "MedicosEspecialidades");
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostAsync(MedicoRequestDto medicoRequestDto)
        {
            if (await _medicoRepo
                .CheckCedulaExistsForCreateAsync(medicoRequestDto.Cedula))
                return BadRequest("Ya existe otro doctor registrado con el número de cédula que ingresó");

            if (!await _medicoRepo
                .CheckRelatedEntityIdsExistsAsync(medicoRequestDto.EspecialidadesId))
                return BadRequest("No existe uno de los grupos asignados");

            return await AddResourceAsync<MedicoRequestDto, MedicoResponseDto>(medicoRequestDto, "GetMedico");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id, MedicoRequestDto medicoRequestDto)
        {
            var resp = await _medicoRepo.UpdateMedicoAsync(id, medicoRequestDto);

            return resp.IsSuccess
                ? resp.SuccessActionResult
                : resp.ErrorActionResult;
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}