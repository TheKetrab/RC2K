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

        return await FetchAll(query);
    }
}
