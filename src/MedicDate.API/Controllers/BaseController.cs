using System.Linq.Expressions;
using AutoMapper;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Common;
using MedicDate.Utility.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.API.Controllers;

public class BaseController<TEntity> : ControllerBase
  where TEntity : class, IId, new()
{
  private readonly IMapper _mapper;
  private readonly IRepository<TEntity> _repository;

  protected BaseController(IRepository<TEntity> repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  protected async Task<
    ActionResult<PaginatedResourceListDto<TResponse>>
  > GetAllWithPagingAsync<TResponse>(
    int pageIndex = 0,
    int pageSize = 10,
    string includeProperties = "",
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null
  )
    where TResponse : class
  {
    var entityList = await _repository.GetAllWithPagingAsync(
      filter,
      orderBy,
      includeProperties,
      false,
      pageIndex,
      pageSize
    );

    var result = new PaginatedResourceListDto<TResponse>
    {
      PageIndex = pageIndex,
      PageSize = pageSize,
      DataResult = _mapper.Map<List<TResponse>>(entityList),
      TotalCount = await _repository.CountResourcesAsync()
    };
    return result;
  }

  protected async Task<ActionResult<List<TResponse>>> GetAllAsync<TResponse>(
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    Expression<Func<TEntity, bool>>? filter = null
  )
  {
    var resp = await _repository.GetAllAsync(isTracking: false, orderBy: orderBy, filter: filter);

    return _mapper.Map<List<TResponse>>(resp);
  }

  protected async Task<ActionResult<TResponse>> GetByIdAsync<TResponse>(
    string id,
    string includeProperties = ""
  )
  {
    var existeEntity = await _repository.ResourceExists(id);

    if (!existeEntity)
      return NotFound($"No existe el registro con id : {id}");

    TEntity? responseEntity;

    if (!string.IsNullOrEmpty(includeProperties))
      responseEntity = await _repository.FirstOrDefaultAsync(
        x => x.Id == id,
        includeProperties,
        false
      );
    else
      responseEntity = (await _repository.FindAsync(id))!;

    return _mapper.Map<TResponse>(responseEntity);
  }

  protected async Task<ActionResult> AddResourceAsync<TRequest, TResponse>(
    TRequest entityRequest,
    string routeResultName,
    string? includeProperties = null
  )
    where TResponse : IId
  {
    var entityDb = _mapper.Map<TEntity>(entityRequest);
    await _repository.AddAsync(entityDb);
    await _repository.SaveAsync();

    TResponse responseEntity;

    if (!string.IsNullOrEmpty(includeProperties))
    {
      var entityWithRelatedData = await _repository.FirstOrDefaultAsync(
        x => x.Id == entityDb.Id,
        includeProperties,
        false
      );

      responseEntity = _mapper.Map<TResponse>(entityWithRelatedData);
    }
    else
    {
      responseEntity = _mapper.Map<TResponse>(entityDb);
    }

    return CreatedAtRoute(routeResultName, new { id = responseEntity.Id }, responseEntity);
  }

  protected async Task<ActionResult> DeleteResourceAsync(string id)
  {
    var response = await _repository.RemoveAsync(id);
    await _repository.SaveAsync();

    if (response == 0)
      return NotFound("No se encontró el registro a eliminar");

    return Ok("Registro Eliminado con éxito");
  }
}
