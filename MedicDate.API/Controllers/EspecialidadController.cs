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
using Microsoft.AspNetCore.Authorization;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            int pageSize = 10)
        {
            return await ListarConPaginacionAsync<EspecialidadResponse>(pageIndex, pageSize);
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<EspecialidadResponse>>> GetAll()
        {
            return await ListarAsync<EspecialidadResponse>();
        }

        [HttpGet("{id:int}", Name = "ObtenerEspecialidad")]
        public async Task<ActionResult<EspecialidadRequest>> GetPutEspecialidad(int id)
        {
            return await GetPutAsync<EspecialidadRequest>(id);
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

            return response.ActionResult;
        }

        [HttpDelete("eliminar/{id:int}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            return await EliminarRegistroAsync(id);
        }
    }
}