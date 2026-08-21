using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class MessageRepository(Database database, DateTimeMessageMapper mapper, IEnvironmentProvider envProvider)
    : CosmosRepository<DateTimeMessage, MessageModel, DateTimeMessageMapper>(database, mapper, envProvider)
    , IMessageRepository
{
    public override string EntityName => "Statistics";

    public override async Task<List<DateTimeMessage>> GetAll()
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.type = 'message'");

        return await FetchAll(query, CancellationToken.None);
    }
}
