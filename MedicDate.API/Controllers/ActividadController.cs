using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Actividad;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : BaseController<Actividad>
    {
        private readonly IUnitOfWork _unitOfWork;

        protected ActividadController(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<ActividadResponse>>>
            GetAllActividadesWithPagingAsync(
                int pageIndex = 0,
                int pageSize = 10
            )
        {
            return await GetAllWithPagingAsync<ActividadResponse>
            (
                pageIndex,
                pageSize,
                orderBy: x => x.OrderBy(ac => ac.Nombre)
            );
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<ActividadResponse>>> GetAllActividadesAsync()
        {
            return await GetAllAsync<ActividadResponse>(
                orderBy: x => x.OrderBy(ac => ac.Nombre));
        }

        [HttpGet("{id}", Name = "GetActividad")]
        public async Task<ActionResult<ActividadResponse>> GetActividadAsync(string id)
        {
            return await GetByIdAsync<ActividadResponse>(id);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostActividadAsync(ActividadRequest actividadReq)
        {
            return await AddResourceAsync<ActividadRequest, ActividadResponse>(actividadReq, "GetActividad");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutActividadAsync(string id, ActividadRequest actividadReq)
        {
            var resp = await _unitOfWork.ActividadRepo.UpdateActividadAsync(id, actividadReq);

            return resp.IsSuccess
                ? Ok("Actividad actualizada con éxito")
                : resp.ErrorActionResult;

        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteActividadAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}