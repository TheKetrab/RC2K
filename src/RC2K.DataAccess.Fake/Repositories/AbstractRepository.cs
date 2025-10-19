using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public abstract class AbstractRepository<TEntity> where TEntity : class, IEntity
{
    protected readonly IDataContext _context;

    protected AbstractRepository(IDataContext context)
    {
        _context = context;
    }

    protected abstract IQueryable<TEntity> DataSet { get; }

    public Task<TEntity> GetById(int id) => Task.FromResult(DataSet.First(x => x.Id == id));

    public Task<List<TEntity>> GetAll() => Task.FromResult(DataSet.ToList());
}
