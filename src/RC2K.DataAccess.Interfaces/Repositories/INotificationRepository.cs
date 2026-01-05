using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<Notification?> GetById(Guid id);
    Task<List<Notification>> GetNotifications(Guid userId);
    Task Create(Notification notification);
    Task Delete(string id);
}
