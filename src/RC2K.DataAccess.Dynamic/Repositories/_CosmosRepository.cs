using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Interfaces;
using RC2K.Extensions;

namespace RC2K.DataAccess.Dynamic.Repositories;

public abstract class CosmosRepository<TEntity, TModel, TMapper>
    where TEntity : class
    where TModel : class
    where TMapper : class, IModelMapper<TEntity, TModel>
{
    protected Database Database { get; }
    protected Container Container { get; }
    protected TMapper Mapper { get; }

    public abstract string EntityName { get; }

    public event EventHandler<(string,double)>? RequestUnitsHandler;

    private ItemQueryIteratorHelper _iterator = new();

    protected CosmosRepository(
        Database database,
        TMapper mapper,
        IEnvironmentProvider envProvider)
    {
        Database = database;
        Mapper = mapper;

        string containerName = envProvider.ResolveContainerName(EntityName);
        Container = Database.GetContainer(containerName);
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        string key = id.ToString();
        var response = await Container.ReadItemAsync<TModel>(key, new PartitionKey(EntityName));
        
        var model = response.Resource;

        return Mapper.ToDomainModel(model);
    }

    protected async Task<List<TEntity>> FetchAll(QueryDefinition query)
    {
        using var it = Container.GetItemQueryIterator<TModel>(query);
        var (result, ru) = await _iterator.FetchAll(query, it, Mapper.ToDomainModel);
        
        string queryText = query.QueryText.Linearize();
        string parameters = string.Join(" ; ", query.GetQueryParameters().Select(x => $"{x.Name}:{x.Value}"));
        RequestUnitsHandler?.Invoke(this, (queryText + " | " + parameters, ru));
        
        return result;
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c");

        return await FetchAll(query);
    }

    public virtual async Task Create(TEntity entity)
    {
        TModel model = Mapper.ToCosmosModel(entity);
        await Container.CreateItemAsync(model);
    }

    public virtual async Task Update(TEntity entity)
    {
        TModel model = Mapper.ToCosmosModel(entity);
        await Container.UpsertItemAsync(model);
    }

    public virtual async Task Delete(string id)
    {
        await Container.DeleteItemAsync<TEntity>(id, new PartitionKey(EntityName));
    }
}
