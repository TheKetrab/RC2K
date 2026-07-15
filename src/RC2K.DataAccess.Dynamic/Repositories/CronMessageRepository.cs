using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class CronMessageRepository(Database database, CronMessageMapper mapper, IEnvironmentProvider envProvider)
    : CosmosRepository<CronMessage, CronMessageModel, CronMessageMapper>(database, mapper, envProvider)
    , ICronMessageRepository
{
    public override string EntityName => "Statistics";

    public override async Task<List<CronMessage>> GetAll()
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.type = 'cronMessage'");

        return await FetchAll(query, CancellationToken.None);
    }
}
