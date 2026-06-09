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

    public virtual async Task<TEntity?> GetById(Guid id, CancellationToken ct = default)
    {
        string key = id.ToString();
        var response = await Container.ReadItemAsync<TModel>(
            key, new PartitionKey(EntityName), cancellationToken: ct);
        
        var model = response.Resource;

        return Mapper.ToDomainModel(model);
    }

    protected async Task<List<TEntity>> FetchAll(QueryDefinition query, CancellationToken ct)
    {
        using var it = Container.GetItemQueryIterator<TModel>(query);
        var (result, ru) = await ItemQueryIteratorHelper.FetchAll(it, Mapper.ToDomainModel, ct);

        RequestUnitsHandlerInvoke(query, ru);
        
        return result;
    }

    public virtual async Task<List<TEntity>> GetAll()
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c");

        return await FetchAll(query, CancellationToken.None);
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

    public virtual Task Delete(string id) =>
        Container.DeleteItemAsync<TEntity>(id, new PartitionKey(EntityName));

    protected void RequestUnitsHandlerInvoke(QueryDefinition query, double ru)
    {
        string queryText = query.QueryText.Linearize();
        string parameters = string.Join(" ; ", query.GetQueryParameters().Select(x => $"{x.Name}:{x.Value}"));
        RequestUnitsHandler?.Invoke(this, (queryText + " | " + parameters, ru));
    }
}
