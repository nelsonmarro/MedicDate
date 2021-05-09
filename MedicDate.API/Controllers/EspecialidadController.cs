using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Utility;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadController : BaseController<Especialidad>
    {
        private readonly IEspecialidadRepository _especialidadRepo;
        private readonly IMapper _mapper;

        public EspecialidadController(IEspecialidadRepository especialidadRepo, IMapper mapper) : base(especialidadRepo,
            mapper)
        {
            _especialidadRepo = especialidadRepo;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<EspecialidadResponse>>> GetAllWithPaging(
            int pageIndex = 0,
            int pageSize = 10,
            string includeProperties = null)
        {
            return await ListarConPaginacionAsync<EspecialidadResponse>(pageIndex, pageSize, includeProperties);
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<EspecialidadResponse>>> GetAll()
        {
            return await ListarAsync<EspecialidadResponse>();
        }

        [HttpGet("{id:int}", Name = "ObtenerEspecialidad")]
        public async Task<ActionResult<EspecialidadRequest>> GetOne(int id)
        {
            return await ObtenerRegistroPorIdAsync<EspecialidadRequest>(id);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostAsync(EspecialidadRequest especialidadResponse)
        {
            return await InsertarRegistroAsync<EspecialidadRequest, EspecialidadResponse>(especialidadResponse,
                "ObtenerEspecialidad");
        }

        [HttpPut("editar/{id:int}")]
        public async Task<ActionResult> PutAsync(int id, EspecialidadRequest especialidadRequest)
        {
            var response = await _especialidadRepo.UpdateEspecialidad(id, especialidadRequest);

            if (!response.Sussces)
            {
                return NotFound(response.Message);
            }

            return Ok(response.Message);
        }

        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return await EliminarRegistroAsync(id);
        }
    }
}