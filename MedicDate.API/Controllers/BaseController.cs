using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Helpers;
using MedicDate.Models.DTOs;
using MedicDate.Utility.Interfaces;

namespace MedicDate.API.Controllers
{
    public class BaseController<TEntity> : ControllerBase where TEntity : class, IId
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TEntity> _repository;
        private readonly IMapper _mapper;

        protected BaseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.GetRepository<TEntity>();
            _mapper = mapper;
        }

        protected async Task<ActionResult<ApiResponseDto<TResponse>>> GetAllWithPagingAsync<TResponse>
        (
            int pageIndex = 0,
            int pageSize = 10,
            bool loadRealtedData = false,
            string includeProperties = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
            where TResponse : class
        {
            try
            {
                if (!loadRealtedData)
                {
                    includeProperties = "";
                }

                var entityList = await _repository.GetAllWithPagingAsync
                (
                    filter,
                    orderBy,
                    includeProperties,
                    false,
                    pageIndex,
                    pageSize
                );

                return new ApiResponseDto<TResponse>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    DataResult = _mapper.Map<List<TResponse>>(entityList),
                    TotalCount = await _repository.CountResourcesAsync()
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los Datos");
            }
        }

        protected async Task<ActionResult<List<TResponse>>> GetAllAsync<TResponse>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            try
            {
                var resp = await _repository.GetAllAsync(isTracking: false, orderBy: orderBy);

                return _mapper.Map<List<TResponse>>(resp);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los datos");
            }
        }

        protected async Task<ActionResult<TResponse>> GetByIdAsync<TResponse>
        (
            string id,
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

                TEntity responseEntity;

                if (loadRealtedData)
                {
                    responseEntity =
                        await _repository.FirstOrDefaultAsync(x => x.Id == id, includeProperties, false);
                }
                else
                {
                    responseEntity = await _repository.FindAsync(id);
                }

                if (responseEntity is null)
                {
                    return NotFound($"No se encontró el registro con Id: {id}");
                }

                return _mapper.Map<TResponse>(responseEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Error al obtener los datos");
            }
        }

        protected async Task<ActionResult> AddResourceAsync<TRequest, TResponse>(TRequest entityRequest,
            string routeResultName, string includeProperties = null)
            where TResponse : IId
        {
            try
            {
                var entityDb = _mapper.Map<TEntity>(entityRequest);
                await _repository.AddAsync(entityDb);
                await _unitOfWork.SaveAsync();

                TResponse responseEntity;

                if (!string.IsNullOrEmpty(includeProperties))
                {
                    var entityWithRelatedData = await _repository
                        .FirstOrDefaultAsync(x => x.Id == entityDb.Id, includeProperties, false);

                    responseEntity = _mapper.Map<TResponse>(entityWithRelatedData);
                }
                else
                {
                    responseEntity = _mapper.Map<TResponse>(entityDb);
                }

                return CreatedAtRoute(routeResultName, new { id = responseEntity.Id },
                    responseEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                return BadRequest("Error al crear el registro");
            }
        }

        protected async Task<ActionResult> DeleteResourceAsync(string id)
        {
            try
            {
                var response = await _repository.Remove(id);

                if (response == 0)
                {
                    return BadRequest("Error al eliminar");
                }

                await _unitOfWork.SaveAsync();

                return Ok("Registro Eliminado con éxito");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                return BadRequest("Error al eliminar el registro");
            }
        }
    }
}