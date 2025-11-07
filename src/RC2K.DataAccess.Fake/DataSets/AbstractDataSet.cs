using Bogus;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;
using System.Collections;
using System.Linq.Expressions;

namespace RC2K.DataAccess.Fake.Repositories;

public abstract class AbstractDataSet<TEntity, TFaker, TDataSet> : IQueryable<TEntity>
    where TEntity : class
    where TFaker : Faker<TEntity>
    where TDataSet : AbstractDataSet<TEntity, TFaker, TDataSet>

{
    protected readonly Faker<TEntity> _faker;
    protected readonly IDataContext? _context;
    protected readonly List<TEntity> _entities = [];

    protected AbstractDataSet(IDataContext? context = null)
    {

        _context = context;
        _faker = CreateFaker().UseSeed(Constants.FakerSeed);
    }

    public TDataSet Generate(int count)
    {
        _entities.Clear();
        _entities.AddRange(_faker.Generate(count));
        return (TDataSet)this;
    }

    protected abstract TFaker CreateFaker();

    public int Count => _entities.Count;

    public Type ElementType => _entities.AsQueryable().ElementType;

    public Expression Expression => _entities.AsQueryable().Expression;

    public IQueryProvider Provider => _entities.AsQueryable().Provider;

    public IEnumerable<TEntity> GetAll() => _entities;

    public TEntity Get(int id) => _entities.First(x => x.Id == id);

    public IEnumerator<TEntity> GetEnumerator() => _entities.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
