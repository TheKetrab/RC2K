using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;

    public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }

    public async Task<List<Notification>> GetUserNotifications(string userName)
    {
        User? user = await _userRepository.GetByName(userName);
        if (user is null)
        {
            return [];
        }

        return await _notificationRepository.GetNotifications(user.Id);
    }

    public async Task NotifyMasterAdmin(string message)
    {
        var theKetrab = await _userRepository.GetByName("TheKetrab");
        await Create(theKetrab.Id, message);
    }

    public async Task Create(Guid userId, string message)
    {
        Notification notification = new Notification()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            Created = DateTime.Now
        };

        await _notificationRepository.Create(notification);
    }

    public async Task Delete(Guid id)
    {
        await _notificationRepository.Delete(id.ToString());
    }

    public async Task<Notification?> GetById(Guid id)
    {
        return await _notificationRepository.GetById(id);
    }
}