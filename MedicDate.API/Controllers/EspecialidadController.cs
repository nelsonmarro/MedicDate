using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadController : BaseController<Especialidad>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EspecialidadController(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<EspecialidadResponse>>> GetAllWithPagingAsync(
            int pageIndex = 0,
            int pageSize = 10)
        {
            return await GetAllWithPagingAsync<EspecialidadResponse>
            (
                pageIndex,
                pageSize,
                orderBy: x => x.OrderBy(e => e.NombreEspecialidad)
            );
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<EspecialidadResponse>>> GetAllAsync()
        {
            return await GetAllAsync<EspecialidadResponse>(
                x => x.OrderBy(e => e.NombreEspecialidad));
        }

        [HttpGet("{id}", Name = "ObtenerEspecialidad")]
        public async Task<ActionResult<EspecialidadRequest>> GetPutEspecialidadAsync(string id)
        {
            return await GetByIdAsync<EspecialidadRequest>(id);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostAsync(EspecialidadRequest especialidadResponse)
        {
            return await AddResourceAsync<EspecialidadRequest, EspecialidadResponse>(especialidadResponse,
                "ObtenerEspecialidad");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id, EspecialidadRequest especialidadRequest)
        {
            try
            {
                var response = await _unitOfWork.EspecialidadRepo.UpdateEspecialidad(id, especialidadRequest);

                return response.ActionResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                return BadRequest("Error al editar el registro");
            }
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}