using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Models.DTOs;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Utility.Interfaces;

namespace MedicDate.API.Controllers
{
    public class BaseController<TEntity> : ControllerBase where TEntity : class, IId
    {
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        protected BaseController(IRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        protected async Task<ActionResult<ApiResponseDto<TResponse>>> ListarConPaginacionAsync<TResponse>
        (
            int pageIndex = 0,
            int pageSize = 10,
            bool traerEspecialidades = false,
            string includeProperties = "")
        {
            ApiResponseDto<TResponse> apiResponse = new();
            try
            {
                if (!traerEspecialidades)
                {
                    includeProperties = "";
                }

                var entityList = await _repository.GetAllAsync(null, null, includeProperties);

                var result = ApiResult<TEntity, TResponse>.Create
                (
                    entityList,
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los Datos");
            }
        }

        protected async Task<ActionResult<List<TResponse>>> ListarAsync<TResponse>()
        {
            try
            {
                var entityList = await _repository.GetAllAsync();

                return _mapper.Map<List<TResponse>>(entityList);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los datos");
            }
        }

        protected async Task<ActionResult<TResponse>> ObtenerRegistroPorIdAsync<TResponse>(int id, bool traerEspecialidades = false, string includeProperties = "")
        {
            try
            {
                TEntity entity;

                if (traerEspecialidades)
                {
                    entity = await _repository.FirstOrDefaultAsync(x => x.Id == id, includeProperties: includeProperties);
                }
                else
                {
                    entity = await _repository.FindAsync(id);
                }

                if (entity is null)
                {
                    return NotFound($"No se encontró el registro con Id: {id}");
                }

                return _mapper.Map<TResponse>(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los datos");
            }
        }

        protected async Task<ActionResult> InsertarRegistroAsync<TRequest, TResponse>(TRequest entityRequest,
            string routeResultName)
            where TResponse : IId
        {
            try
            {
                var entityDb = _mapper.Map<TEntity>(entityRequest);
                await _repository.AddAsync(entityDb);
                await _repository.SaveAsync();

                var entityResponse = _mapper.Map<TResponse>(entityDb);

                return CreatedAtRoute(routeResultName, new { id = entityResponse.Id },
                    entityResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                return BadRequest("Error al crear el registro");
            }
        }

        protected async Task<ActionResult> EliminarRegistroAsync(int id)
        {
            var response = await _repository.Remove(id);

            if (response == 0)
            {
                return BadRequest("Error al eliminar");
            }

            await _repository.SaveAsync();

            return Ok("Registro Eliminado con éxito");
        }
    }
}