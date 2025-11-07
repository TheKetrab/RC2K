using System.Linq.Expressions;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> Get(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "",
        bool full = false);

    Task<TEntity?> GetById(int id);

    Task Insert(TEntity entity);

    Task Delete(int id);

    void Delete(TEntity entity);
    
    void Update(TEntity entity);
}
