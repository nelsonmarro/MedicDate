using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Bussines.Repository;
using MedicDate.Bussines.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : BaseController<Medico>
    {
        private readonly IMedicoRepository _medicoRepo;
        private readonly IMapper _mapper;

        public MedicoController(IMedicoRepository medicoRepo, IMapper mapper) : base(medicoRepo, mapper)
        {
            _medicoRepo = medicoRepo;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<MedicoResponse>>> GetAllWithPaging
        (
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerEspecialidades = false,
            [FromQuery] int? filtrarEspecialidadId = null
        )
        {
            if (filtrarEspecialidadId is not null)
            {
                var medicosDb = await _medicoRepo.GetAllAsync(
                   filter: x => x.MedicosEspecialidades
                    .Any(y => y.EspecialidadId == filtrarEspecialidadId.Value),
                    includeProperties: "MedicosEspecialidades.Especialidad"
                    );

                ApiResponseDto<MedicoResponse> apiResponse = new();

                var result = ApiResult<Medico, MedicoResponse>.Create
                            (
                                medicosDb,
                                pageIndex,
                                pageSize,
                                _mapper
                            );

                apiResponse.DataResult = result.DataResult;
                apiResponse.TotalCount = result.TotalCount;
                apiResponse.PageIndex = result.PageIndex;
                apiResponse.PageSize = result.PageSize;
                apiResponse.TotalPages = result.TotalPages;

                return apiResponse;
            }
            else
            {
                return await ListarConPaginacionAsync<MedicoResponse>(pageIndex, pageSize, traerEspecialidades, "MedicosEspecialidades.Especialidad");
            }
        }

        [HttpGet("{id:int}", Name = "ObtenerMedico")]
        public async Task<ActionResult<MedicoResponse>> GetMedicoPorId(int id, [FromQuery] bool traerEspecialidades = false)
        {
            var existeMedico = await _medicoRepo.ResourceExists(id);

            if (!existeMedico)
            {
                return NotFound($"No existe un médico con el id : {id}");
            }

            return await ObtenerRegistroPorIdAsync<MedicoResponse>(id, traerEspecialidades, "MedicosEspecialidades.Especialidad");
        }

        [HttpGet("obtenerParaEditar/{id:int}")]
        public async Task<ActionResult<MedicoRequest>> GetPut(int id)
        {
            var existeMedico = await _medicoRepo.ResourceExists(id);

            if (!existeMedico)
            {
                return NotFound($"No existe un médico con el id : {id}");
            }

            return await _medicoRepo.GetMedicoParaEdicion<MedicoRequest>(id);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> Post(MedicoRequest medicoRequest)
        {
            try
            {
                if (!await _medicoRepo.ExisteEspecialidadIdParaCrearMedico(medicoRequest.EspecialidadesId))
                {
                    return BadRequest("No existe una de las especialidades asignadas");
                }

                var entityDb = _mapper.Map<Medico>(medicoRequest);
                await _medicoRepo.AddAsync(entityDb);
                await _medicoRepo.SaveAsync();

                var entityResponse = await _medicoRepo.GetMedicoConEspecialidades<MedicoResponse>(entityDb.Id);

                return CreatedAtRoute("ObtenerMedico", new { id = entityResponse.Id },
                    entityResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                return BadRequest("Error al crear el registro");
            }
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> Put(int id, MedicoRequest medicoRequest)
        {
            var response = await _medicoRepo.UpdateMedicoAsync(id, medicoRequest);

            if (!response.Sussces)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }
    }
}