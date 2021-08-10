using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : BaseController<Medico>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MedicoController(
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<MedicoResponse>>> GetAllWithPagingAsync(
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerEspecialidades = false,
            [FromQuery] string filtrarEspecialidadId = null
        )
        {
            if (!string.IsNullOrEmpty(filtrarEspecialidadId))
            {
                return await GetAllWithPagingAsync<MedicoResponse>
                (
                    pageIndex,
                    pageSize,
                    traerEspecialidades,
                    "MedicosEspecialidades.Especialidad",
                    m => m.MedicosEspecialidades
                        .Any(me => me.EspecialidadId == filtrarEspecialidadId),
                    x => x.OrderBy(m => m.Nombre)
                );
            }

            return await GetAllWithPagingAsync<MedicoResponse>
            (
                pageIndex,
                pageSize,
                traerEspecialidades,
                "MedicosEspecialidades.Especialidad",
                null,
                x => x.OrderBy(m => m.Nombre)
            );
        }

        [HttpGet("{id}", Name = "GetMedico")]
        public async Task<ActionResult<MedicoResponse>> GetMedicoByIdAsync(string id,
            [FromQuery] bool traerEspecialidades = false)
        {
            var existeMedico = await _unitOfWork.MedicoRepo.ResourceExists(id);

            if (!existeMedico)
            {
                return NotFound($"No existe un médico con el id : {id}");
            }

            return await GetByIdAsync<MedicoResponse>(id, traerEspecialidades, "MedicosEspecialidades.Especialidad");
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<MedicoRequest>> GetPutMedicoAsync(string id)
        {
            return await GetByIdAsync<MedicoRequest>(id, true, "MedicosEspecialidades");
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostAsync(MedicoRequest medicoRequest)
        {
            var response = await ControllerEntityValidator
                .ValidateAsync(_unitOfWork.MedicoRepo, cedula: medicoRequest.Cedula,
                    entityIds: medicoRequest.EspecialidadesId);

            if (!response.IsSuccess) return response.ErrorActionResult;

            return await AddResourceAsync<MedicoRequest, MedicoResponse>(medicoRequest, "GetMedico");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id, MedicoRequest medicoRequest)
        {
            var resp = await _unitOfWork.MedicoRepo.UpdateMedicoAsync(id, medicoRequest);

            return resp.IsSuccess
                ? Ok("Doctor actualizado con éxito")
                : resp.ErrorActionResult;
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}