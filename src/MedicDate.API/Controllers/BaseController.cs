﻿using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Utility.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedicDate.API.DTOs;
using MedicDate.API.DTOs.Common;

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

        protected async Task<ActionResult<PaginatedResourceListDto<TResponse>>> GetAllWithPagingAsync<TResponse>(
            int pageIndex = 0,
            int pageSize = 10,
            string includeProperties = "",
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
            where TResponse : class
        {
            var entityList = await _repository.GetAllWithPagingAsync
            (
                filter,
                orderBy,
                includeProperties,
                false,
                pageIndex,
                pageSize
            );

            return new PaginatedResourceListDto<TResponse>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                DataResult = _mapper.Map<List<TResponse>>(entityList),
                TotalCount = await _repository.CountResourcesAsync()
            };
        }

        protected async Task<ActionResult<List<TResponse>>> GetAllAsync<TResponse>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var resp = await _repository.GetAllAsync(isTracking: false, orderBy: orderBy);

            return _mapper.Map<List<TResponse>>(resp);
        }

        protected async Task<ActionResult<TResponse>> GetByIdAsync<TResponse>(
            string id,
            string includeProperties = ""
        )
        {
            var existeEntity = await _repository.ResourceExists(id);

            if (!existeEntity)
            {
                return NotFound($"No existe el registro con id : {id}");
            }

            TEntity responseEntity;

            if (!string.IsNullOrEmpty(includeProperties))
                responseEntity =
                    await _repository.FirstOrDefaultAsync(x => x.Id == id, includeProperties, false);
            else
                responseEntity = await _repository.FindAsync(id);

            return _mapper.Map<TResponse>(responseEntity);
        }

        protected async Task<ActionResult> AddResourceAsync<TRequest, TResponse>(TRequest entityRequest,
            string routeResultName, string includeProperties = null)
            where TResponse : IId
        {
            var entityDb = _mapper.Map<TEntity>(entityRequest);
            await _repository.AddAsync(entityDb);
            await _repository.SaveAsync();

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

        protected async Task<ActionResult> DeleteResourceAsync(string id)
        {
            var response = await _repository.RemoveAsync(id);

            if (response == 0) return NotFound("No se encontró el registro a eliminar");

            await _repository.SaveAsync();

            return Ok("Registro Eliminado con éxito");
        }
    }
}