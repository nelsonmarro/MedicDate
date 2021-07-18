using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using MedicDate.Bussines.Helpers;
using MedicDate.Models.DTOs;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(int id);
        Task<TResponse> FindAsync<TResponse>(int id);

        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
        );

        Task<List<TResponse>> GetAllAsync<TResponse>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
        );

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
        );

        Task<TResponse> FirstOrDefaultAsync<TResponse>(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
        );

        Task<bool> ResourceExists(int resourceId);

        Task AddAsync(TEntity entity);

        Task<int> Remove(int id);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task SaveAsync();
    }
}