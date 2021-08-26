﻿using AutoMapper;
using MedicDate.API.DTOs.Actividad;
using MedicDate.API.DTOs.Common;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Entities;
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
		private readonly IActividadRepository _actividadRepo;

		public ActividadController(IActividadRepository actividadRepo, IMapper mapper)
			: base(actividadRepo, mapper)
		{
			_actividadRepo = actividadRepo;
		}

		[HttpGet("listarConPaginacion")]
		public async Task<ActionResult<PaginatedResourceListDto<ActividadResponseDto>>>
			GetAllActividadesWithPagingAsync(
				int pageIndex = 0,
				int pageSize = 10
			)
		{
			return await GetAllWithPagingAsync<ActividadResponseDto>
			(
				pageIndex,
				pageSize,
				orderBy: x => x.OrderBy(ac => ac.Nombre)
			);
		}

		[HttpGet("listar")]
		public async Task<ActionResult<List<ActividadResponseDto>>> GetAllActividadesAsync()
		{
			return await GetAllAsync<ActividadResponseDto>(
				x => x.OrderBy(ac => ac.Nombre));
		}

		[HttpGet("{id}", Name = "GetActividad")]
		public async Task<ActionResult<ActividadResponseDto>> GetActividadAsync(string id)
		{
			return await GetByIdAsync<ActividadResponseDto>(id);
		}

		[HttpGet("obtenerParaEditar/{id}")]
		public async Task<ActionResult<ActividadRequestDto>> GetPutActividadAsync(string id)
		{
			return await GetByIdAsync<ActividadRequestDto>(id);
		}

		[HttpPost("crear")]
		public async Task<ActionResult> PostActividadAsync(ActividadRequestDto actividadReq)
		{
			return await AddResourceAsync<ActividadRequestDto, ActividadResponseDto>(actividadReq, "GetActividad");
		}

		[HttpPut("editar/{id}")]
		public async Task<ActionResult> PutActividadAsync(string id, ActividadRequestDto actividadReq)
		{
			var resp = await _actividadRepo.UpdateActividadAsync(id, actividadReq);

			return resp.IsSuccess
				? resp.SuccessActionResult
				: resp.ErrorActionResult;

		}

		[HttpDelete("eliminar/{id}")]
		public async Task<ActionResult> DeleteActividadAsync(string id)
		{
			return await DeleteResourceAsync(id);
		}
	}
}