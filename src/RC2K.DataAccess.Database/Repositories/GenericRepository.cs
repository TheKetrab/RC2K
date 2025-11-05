using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DataAccess.Interfaces.Cache;

namespace RC2K.DataAccess.Database.Repositories;

public abstract class GenericRepository<TEntity, TCache> : IGenericRepository<TEntity>
    where TEntity : class
    where TCache : IGenericCache<TEntity>
{
    protected RallyDbContext _dbContext;
    protected DbSet<TEntity> _dbSet;
    protected TCache _cache;

    protected GenericRepository(RallyDbContext dbContext, TCache cache)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
        _cache = cache;
    }

    protected virtual IQueryable<TEntity> Full(IQueryable<TEntity> query) => query;

    public virtual async Task<List<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        bool full = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (full)
        {
            query = Full(query);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public virtual async Task<TEntity?> GetById(int id)
    {
        var cacheValue = _cache.Get(id);
        if (cacheValue is not null)
        {
            return cacheValue;
        }

        var dbValue = await _dbSet.FindAsync(id);
        if (dbValue is not null)
        {
            _cache.Set(id, dbValue);
        }

        return dbValue;
    }

    public virtual async Task Insert(TEntity entity) => await _dbSet.AddAsync(entity);

    public virtual async Task Delete(int id)
    {
        TEntity? entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            Delete(entity);
        }
    }

    public virtual void Delete(TEntity entity)
    {
        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbSet.Attach(entity);
        }
        _dbSet.Remove(entity);
    }

    public virtual void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }
}
