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
    [Authorize]
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
        public async Task<ActionResult<ApiResponseDto<MedicoResponse>>> GetAllWithPagingAsync
        (
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerEspecialidades = false,
            [FromQuery] int? filtrarEspecialidadId = null
        )
        {
            if (filtrarEspecialidadId is not null)
            {
                return await GetAllWithPagingAsync<MedicoResponse>(pageIndex, pageSize, traerEspecialidades,
                    "MedicosEspecialidades.Especialidad",
                    filter: x => x.MedicosEspecialidades.Any(y => y.EspecialidadId == filtrarEspecialidadId));
            }

            return await GetAllWithPagingAsync<MedicoResponse>(pageIndex, pageSize, traerEspecialidades,
                "MedicosEspecialidades.Especialidad");
        }

        [HttpGet("{id:int}", Name = "ObtenerMedico")]
        public async Task<ActionResult<MedicoResponse>> GetMedicoByIdAsync(int id,
            [FromQuery] bool traerEspecialidades = false)
        {
            var existeMedico = await _medicoRepo.ResourceExists(id);

            if (!existeMedico)
            {
                return NotFound($"No existe un médico con el id : {id}");
            }

            return await GetByIdAsync<MedicoResponse>(id, traerEspecialidades, "MedicosEspecialidades.Especialidad");
        }

        [HttpGet("obtenerParaEditar/{id:int}")]
        public async Task<ActionResult<MedicoRequest>> GetPutMedicoAsync(int id)
        {
            return await GetPutAsync<MedicoRequest>(id, "MedicosEspecialidades");
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostAsync(MedicoRequest medicoRequest)
        {
            try
            {
                if (!await _medicoRepo.EspecialidadIdExistForMedicoCreation(medicoRequest.EspecialidadesId))
                {
                    return BadRequest("No existe una de las especialidades asignadas");
                }

                var entityDb = _mapper.Map<Medico>(medicoRequest);
                await _medicoRepo.AddAsync(entityDb);
                await _medicoRepo.SaveAsync();

                var meidcoDb = await _medicoRepo
                    .FirstOrDefaultAsync(x => x.Id == entityDb.Id,
                        includeProperties: "MedicosEspecialidades.Especialidad");

                var entityResponse = _mapper.Map<MedicoResponse>(meidcoDb);

                return CreatedAtRoute("ObtenerMedico", new {id = entityResponse.Id},
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
        public async Task<ActionResult> PutAsync(int id, MedicoRequest medicoRequest)
        {
            var response = await _medicoRepo.UpdateMedicoAsync(id, medicoRequest);
            return response.ActionResult;
        }

        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}