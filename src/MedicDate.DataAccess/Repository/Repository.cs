using System.Linq.Expressions;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.DataAccess.Repository;

public class Repository<TEntity> : IRepository<TEntity>
  where TEntity : class
{
  private readonly ApplicationDbContext _context;
  private readonly DbSet<TEntity> _dBSet;

  public Repository(ApplicationDbContext context)
  {
    _context = context;
    _dBSet = context.Set<TEntity>();
  }

  public virtual async Task<TEntity?> FindAsync(string id)
  {
    return await _dBSet.FindAsync(id);
  }

  public virtual async Task<List<TEntity>> GetAllAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    string? includeProperties = null,
    bool isTracking = true
  )
  {
    var query = EntityFetchQueryBuilder(filter, orderBy, includeProperties, isTracking);

    return await query.ToListAsync();
  }

  public virtual async Task<List<TEntity>> GetAllWithPagingAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    string? includeProperties = null,
    bool isTracking = true,
    int pageIndex = 0,
    int pageSize = 10
  )
  {
    var query = EntityFetchQueryBuilder(filter, orderBy, includeProperties, isTracking);

    query = query.Paginate(pageIndex, pageSize);

    return await query.ToListAsync();
  }

  public virtual async Task<TEntity?> FirstOrDefaultAsync(
    Expression<Func<TEntity, bool>>? filter = null,
    string? includeProperties = null,
    bool isTracking = true
  )
  {
    var query = EntityFetchQueryBuilder(filter, null, includeProperties, isTracking);

    var entityDb = await query.FirstOrDefaultAsync();

    return entityDb;
  }

  public virtual async Task AddAsync(TEntity entity)
  {
    await _dBSet.AddAsync(entity);
  }

  public virtual async Task<int> RemoveAsync(string id)
  {
    var resp = 1;

    var entity = await _dBSet.FindAsync(id);

    if (entity is null)
      resp = 0;
    else
      Remove(entity);

    return resp;
  }

  public virtual void Remove(TEntity entity)
  {
    _dBSet.Remove(entity);
  }

  public virtual void RemoveRange(List<TEntity> entities)
  {
    _dBSet.RemoveRange(entities);
  }

  public virtual async Task<int> CountResourcesAsync(Expression<Func<TEntity, bool>>? filter = null)
  {
    var query = _dBSet.AsQueryable();

    if (filter is not null)
    {
      query = query.Where(filter);
    }

    return await query.CountAsync();
  }

  public virtual async Task<int> SaveAsync()
  {
    return await _context.SaveChangesAsync();
  }

  public virtual async Task<bool> ResourceExists(string resourceId)
  {
    var resourse = await FindAsync(resourceId);

    return resourse != null;
  }

  private IQueryable<TEntity> EntityFetchQueryBuilder(
    Expression<Func<TEntity, bool>>? filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    string? includeProperties = null,
    bool isTracking = true
  )
  {
    IQueryable<TEntity> query = _dBSet;

    if (!isTracking)
      query = query.AsNoTracking();

    if (filter != null)
      query = query.Where(filter);

    if (includeProperties != null)
      query = includeProperties
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Aggregate(query, (current, s) => current.Include(s));

    if (orderBy != null)
      query = orderBy(query);

    return query;
  }

  public virtual async Task AddRangeAsync(List<TEntity> entities)
  {
    await _dBSet.AddRangeAsync(entities);
  }
}
