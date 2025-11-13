using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;

namespace RC2K.DataAccess.Dynamic.Repositories;

public abstract class CosmosRepository<TEntity, TModel, TMapper>
    where TEntity : class
    where TModel : class
    where TMapper : class, IModelMapper<TEntity, TModel>
{
    protected Database Database { get; }
    protected Container Container { get; }
    protected TMapper Mapper { get; }

    public abstract string ContainerName { get; }

    protected CosmosRepository(Database database, TMapper mapper)
    {
        Database = database;
        Container = Database.GetContainer(ContainerName);
        Mapper = mapper;
    }

    public virtual async Task<TEntity?> GetById(Guid id)
    {
        string key = id.ToString();
        var response = await Container.ReadItemAsync<TModel>(key, new PartitionKey(ContainerName));
        
        var model = response.Resource;

        return Mapper.ToDomainModel(model);
    }

   

    public virtual async Task<List<TEntity>> GetAll()
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c");

        using var it = Container.GetItemQueryIterator<TModel>(query);

        List<TEntity> result = new();
        while (it.HasMoreResults)
        {
            var response = await it.ReadNextAsync();

            foreach (var v in response)
            {
                result.Add(Mapper.ToDomainModel(v));
            }
        }

        return result;
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
        await Container.DeleteItemAsync<TEntity>(id, new PartitionKey(ContainerName));
    }
}
