﻿using System.Linq.Expressions;

namespace MedicDate.Domain.Interfaces.DataAccess;

public interface IRepository<TEntity> where TEntity : class
{
  Task<TEntity?> FindAsync(string id);

  Task<List<TEntity>> GetAllAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    string? includeProperties = null,
    bool isTracking = true
  );

  Task<List<TEntity>> GetAllWithPagingAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    string? includeProperties = null,
    bool isTracking = true,
    int pageIndex = 0,
    int pageSize = 10
  );

  Task<TEntity?> FirstOrDefaultAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    string? includeProperties = null,
    bool isTracking = true
  );

  Task<bool> ResourceExists(string resourceId);

  Task AddAsync(TEntity entity);

  Task AddRangeAsync(List<TEntity> entities);

  Task<int> RemoveAsync(string id);

  void Remove(TEntity entity);

  void RemoveRange(List<TEntity> entities);

  Task<int> CountResourcesAsync(Expression<Func<TEntity, bool>>? filter = null);

  Task<int> SaveAsync();
}