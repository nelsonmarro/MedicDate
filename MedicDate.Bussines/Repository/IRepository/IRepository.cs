using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using MedicDate.Bussines.Helpers;
using MedicDate.Models.DTOs;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedicDate.Bussines.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(string id);

        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
        );

        Task<List<TEntity>> GetAllWithPagingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true,
            int pageIndex = 0,
            int pageSize = 10
        );

        Task<TEntity> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
        );

        Task<bool> ResourceExists(string resourceId);

        Task AddAsync(TEntity entity);

        Task<int> Remove(string id);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task<int> CountResourcesAsync();
    }
}