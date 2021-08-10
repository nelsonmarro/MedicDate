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
using MedicDate.Models.DTOs.Grupo;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrupoController : BaseController<Grupo>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GrupoController(IUnitOfWork unitOfWork, IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<GrupoResponse>>> GetAllGruposWithPagingAsync(
            int pageIndex = 0,
            int pageSize = 10
        )
        {
            return await GetAllWithPagingAsync<GrupoResponse>
            (
                pageIndex,
                pageSize,
                orderBy: x => x.OrderBy(g => g.Nombre)
            );
        }

        [HttpGet("listar")]
        public async Task<ActionResult<List<GrupoResponse>>> GetAllGruposAsync()
        {
            return await GetAllAsync<GrupoResponse>(
                x => x.OrderBy(g => g.Nombre));
        }

        [HttpGet("{id}", Name = "GetGrupo")]
        public async Task<ActionResult<GrupoResponse>> GetGrupoAsync(string id)
        {
            return await GetByIdAsync<GrupoResponse>(id);
        }

        [HttpPost("crear")]
        public async Task<ActionResult> PostGrupoAsync(GrupoRequest grupoRequest)
        {
            return await AddResourceAsync<GrupoRequest, GrupoResponse>(grupoRequest, "GetGrupo");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutGrupoAsync(string id, GrupoRequest grupoRequest)
        {
            try
            {
                var response = await _unitOfWork.GrupoRepo.UpdateGrupoAsync(id, grupoRequest);

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
        public async Task<ActionResult> DeleteGrupoAsync(string id)
        {
            return await DeleteResourceAsync(id);
        }
    }
}
