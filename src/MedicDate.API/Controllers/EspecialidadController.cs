using AutoMapper;
using MedicDate.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Common;
using MedicDate.Shared.Models.Especialidad;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadController : BaseController<Especialidad>
    {
        private readonly IEspecialidadRepository _especialidadRepo;
        private readonly IMapper _mapper;

        public EspecialidadController(IEspecialidadRepository especialidadRepo, IMapper mapper)
            : base(especialidadRepo, mapper)
        {
            _especialidadRepo = especialidadRepo;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<PaginatedResourceListDto<EspecialidadResponseDto>>>
            GetAllEspecialidadesWithPagingAsync(
                int pageIndex = 0,
                int pageSize = 10)
        {
            return await GetAllWithPagingAsync<EspecialidadResponseDto>
            (
                pageIndex,
                pageSize,
                orderBy: x => x.OrderBy(e => e.NombreEspecialidad)
            );
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<EspecialidadResponseDto>>> GetAllEspecialidadesAsync()
        {
            return await GetAllAsync<EspecialidadResponseDto>(
                x => x.OrderBy(e => e.NombreEspecialidad));
        }

        [HttpGet("{id}", Name = "GetEspecialidad")]
        public async Task<ActionResult<EspecialidadResponseDto>> GetEspecialidadAsync(string id)
        {
            return await GetByIdAsync<EspecialidadResponseDto>(id);
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<EspecialidadRequestDto>> GetPutEspecialidadAsync(string id)
        {
            return await GetByIdAsync<EspecialidadRequestDto>(id);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> CreateEspecialidadAsync(EspecialidadRequestDto especialidadResponse)
        {
            return await AddResourceAsync<EspecialidadRequestDto,
                EspecialidadResponseDto>(especialidadResponse, "GetEspecialidad");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> UpdateEspecialidadAsync(string id,
            EspecialidadRequestDto especialidadRequestDto)
        {
            var resp = await _especialidadRepo.UpdateEspecialidadAsync(id, especialidadRequestDto);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteEspecialidadAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}