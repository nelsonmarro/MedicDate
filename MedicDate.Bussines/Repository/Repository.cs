using MedicDate.Bussines.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Data;
using MedicDate.Models.DTOs;

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

        public async Task<TEntity> FindAsync(int id)
        {
            return await _dBSet.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null,
            bool isTracking = true)
        {
            IQueryable<TEntity> query = _dBSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var s in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(s);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null, bool isTracking = true)
        {
            IQueryable<TEntity> query = _dBSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var s in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(s);
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dBSet.AddAsync(entity);
        }

        public async Task<int> Remove(int id)
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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ResourceExists(int resourceId)
        {
            var resourse = await _dBSet.FindAsync(resourceId);

            return resourse != null;
        }
    }
}