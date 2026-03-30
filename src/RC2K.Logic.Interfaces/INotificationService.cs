using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface INotificationService
{
    Task NotifyMasterAdmin(string message);
    Task<List<Notification>> GetUserNotifications(string userName);
    Task<int> GetUserNotificationsCount(string userName);
    Task Create(Guid userId, string message);
    Task Delete(Guid userId, Guid id);
    Task DeleteMany(List<Guid> userIds, List<Guid> ids);

    event EventHandler<Guid>? NotificationsUpdated;
}
