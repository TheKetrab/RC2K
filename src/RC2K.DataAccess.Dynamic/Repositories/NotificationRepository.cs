using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class NotificationRepository(
    Database database, NotificationMapper mapper, IEnvironmentProvider envProvider)
    : CosmosRepository<Notification, NotificationModel, NotificationMapper>(database, mapper, envProvider)
    , INotificationRepository
{
    public override string EntityName => "Notifications";

    public async Task<List<Notification>> GetNotifications(Guid userId)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.userId = @userId")
            .WithParameter("@userId", userId);

        return await FetchAll(query, CancellationToken.None);
    }

    public async Task<int> GetNotificationsCount(Guid userId)
    {
        var query = new QueryDefinition(@"
            SELECT VALUE COUNT(1) FROM c WHERE c.userId = @userId")
            .WithParameter("@userId", userId);

        var it = Container.GetItemQueryIterator<int>(query);
        int count = 0;
        while (it.HasMoreResults)
        {
            count = (await it.ReadNextAsync()).SingleOrDefault();
        }

        return count;
    }
}
