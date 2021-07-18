using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Helpers;
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

        protected async Task<ActionResult<ApiResult<TResponse>>> GetAllWithPagingAsync<TResponse>
        (
            int pageIndex = 0,
            int pageSize = 10,
            bool loadRealtedData = false,
            string includeProperties = "",
            Expression<Func<TEntity, bool>> filter = null,
            string sortColumn = null,
            string sortOrder = null
        )
            where TResponse : class
        {
            try
            {
                if (!loadRealtedData)
                {
                    includeProperties = "";
                }

                var entityList = await _repository.GetAllAsync<TResponse>(filter, null, includeProperties);

                var result = ApiResult<TResponse>.Create
                (
                    entityList,
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder
                );

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los Datos");
            }
        }

        protected async Task<ActionResult<List<TResponse>>> GetAllAsync<TResponse>()
        {
            try
            {
                return await _repository.GetAllAsync<TResponse>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los datos");
            }
        }

        protected async Task<ActionResult<TResponse>> GetByIdAsync<TResponse>
        (
            int id,
            bool loadRealtedData = false,
            string includeProperties = ""
        )
        {
            try
            {
                var existeEntity = await _repository.ResourceExists(id);

                if (!existeEntity)
                {
                    return NotFound($"No existe el registro con id : {id}");
                }

                TResponse responseEntity;

                if (loadRealtedData)
                {
                    responseEntity =
                        await _repository.FirstOrDefaultAsync<TResponse>(x => x.Id == id, includeProperties);
                }
                else
                {
                    responseEntity = await _repository.FindAsync<TResponse>(id);
                }

                if (responseEntity is null)
                {
                    return NotFound($"No se encontró el registro con Id: {id}");
                }

                return responseEntity;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los datos");
            }
        }

        protected async Task<ActionResult> AddResourceAsync<TRequest, TResponse>(TRequest entityRequest,
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

        protected async Task<ActionResult> DeleteResourceAsync(int id)
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