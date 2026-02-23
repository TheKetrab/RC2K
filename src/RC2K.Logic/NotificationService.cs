using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        INotificationRepository notificationRepository, 
        IUserRepository userRepository, 
        ILogger<NotificationService> logger)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _logger = logger;
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
        if (theKetrab is null)
        {
            _logger.LogError("Master admin user 'TheKetrab' not found. Cannot send notification. {Message}", message);
            return;
        }
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
}