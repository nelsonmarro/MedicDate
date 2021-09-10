using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.API.DTOs.Cita;
using MedicDate.API.DTOs.Common;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : BaseController<Cita>
    {
        private readonly ICitaRepository _citaRepo;

        public CitaController(ICitaRepository citaRepo, IMapper mapper) : base(citaRepo, mapper)
        {
            _citaRepo = citaRepo;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<PaginatedResourceListDto<CitaCalendarDto>>> GetAllCitasWithPagingAsync(
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] string medicoId = null,
            [FromQuery] string pacienteId = null
        )
        {
            Expression<Func<Cita, bool>> filter = null;

            if (medicoId is not null && pacienteId is null)
                filter = c => c.MedicoId == medicoId;

            if (pacienteId is not null && medicoId is null)
                filter = c => c.PacienteId == pacienteId;

            if (pacienteId is not null && medicoId is not null)
                filter = c => c.PacienteId == pacienteId && c.MedicoId == medicoId;

            return await GetAllWithPagingAsync<CitaCalendarDto>
            (
                pageIndex,
                pageSize,
                "Medico,Paciente",
                filter,
                x => x.OrderBy(c => c.FechaInicio)
            );
        }

        [HttpGet("{id}", Name = "GetCitaDetails")]
        public async Task<ActionResult<CitaDetailsDto>> GetCitaDetailsAsync(string id)
        {
            return await GetByIdAsync<CitaDetailsDto>(id,
                "Medico,Paciente,Archivos,ActividadesCita.Actividad");
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<CitaRequestDto>> GetPutCitaAsyc(string id)
        {
            return await GetByIdAsync<CitaRequestDto>(id, "Medico,Paciente,Archivos,ActividadesCita.Actividad");
        }

        [HttpPost("crear")]
        public async Task<ActionResult> CreateCitaAsync(CitaRequestDto citaRequestDto)
        {
            return await AddResourceAsync<CitaRequestDto, CitaDetailsDto>(citaRequestDto, "GetCitaDetails");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> UpdateCitaAsync(string id, CitaRequestDto citaRequestDto)
        {
            var result = await _citaRepo.UpdateCitaAsync(id, citaRequestDto);

            return result.Succeeded
                ? result.SuccessResult
                : result.ErrorResult;
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteCitaAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}