using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public abstract class AbstractRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly IDataContext _context;

    protected AbstractRepository(IDataContext context)
    {
        _context = context;
    }

    protected abstract IQueryable<TEntity> DataSet { get; }

    public Task<TEntity> GetById(int id) => DataSet.FirstAsync(x => x.Id == id);

    public Task<List<TEntity>> GetAll() => DataSet.ToListAsync();
}
