using MedicDate.Bussines.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Data;

namespace MedicDate.Bussines.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _dBSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dBSet = context.Set<TEntity>();
        }

        public async Task<TEntity> FindAsync(string id)
        {
            return await _dBSet.FindAsync(id);
        }


        public async Task<List<TEntity>> GetAllAsync
        (
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
        )
        {
            IQueryable<TEntity> query = _dBSet;

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }


            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, s) => current.Include(s));
            }


            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllWithPagingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true, int pageIndex = 0, int pageSize = 10)
        {
            IQueryable<TEntity> query = _dBSet;

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            var count = await query.CountAsync();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, s) => current.Include(s));
            }

            query = query.Paginate(pageIndex, pageSize);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync
        (
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
        )
        {
            IQueryable<TEntity> query = _dBSet;

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                query = includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Aggregate(query, (current, s) => current.Include(s));
            }


            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dBSet.AddAsync(entity);
        }


        public async Task<int> Remove(string id)
        {
            var resp = 1;

            var entity = await _dBSet.FindAsync(id);

            if (entity is null)
            {
                resp = 0;
            }
            else
            {
                Remove(entity);
            }

            return resp;
        }

        public void Remove(TEntity entity)
        {
            _dBSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dBSet.RemoveRange(entities);
        }

        public async Task<int> CountResourcesAsync()
        {
            return await _dBSet.CountAsync();
        }

        public async Task<bool> ResourceExists(string resourceId)
        {
            var resourse = await FindAsync(resourceId);

            return resourse != null;
        }
    }
}