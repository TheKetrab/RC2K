using Microsoft.AspNetCore.Components.Authorization;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;

namespace RC2K.Presentation.Blazor.AuthProxy;

public class AuthNotificationServiceProxy : INotificationService
{
    private AuthenticationStateProvider _asp;
    private NotificationService _service;
    private INotificationRepository _notificationRepository;
    private IUserService _userService;

    public event EventHandler<Guid>? NotificationsUpdated
    {
        add => _service.NotificationsUpdated += value;
        remove => _service.NotificationsUpdated -= value;
    }
    public AuthNotificationServiceProxy(
        AuthenticationStateProvider asp,
        NotificationService service,
        INotificationRepository notificationRepository,
        IUserService userService)
    {
        _asp = asp;
        _service = service;
        _notificationRepository = notificationRepository;
        _userService = userService;
    }

    public Task Create(Guid userId, string message) =>
        _service.Create(userId, message);

    public async Task Delete(Guid userId, Guid id)
    {
        await AuthorizeSelf(id);

        await _service.Delete(userId, id);
    }

    public async Task DeleteMany(List<Guid> userIds, List<Guid> ids)
    {
        foreach (var id in ids)
            await AuthorizeSelf(id);

        await _service.DeleteMany(userIds, ids);
    }

    public async Task<List<Notification>> GetUserNotifications(string userName)
    {
        var auth = await _asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, userName);

        return await _service.GetUserNotifications(userName);
    }

    public async Task<int> GetUserNotificationsCount(string userName)
    {
        var auth = await _asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, userName);

        return await _service.GetUserNotificationsCount(userName);
    }

    public Task NotifyMasterAdmin(string message) =>
        _service.NotifyMasterAdmin(message);

    private async Task AuthorizeSelf(Guid notificationId)
    {
        Notification? notification = await _notificationRepository.GetById(notificationId, CancellationToken.None);
        if (notification == null) 
        {
            return;
        }
        User? user = await _userService.GetById(notification.UserId);
        if (user == null)
        {
            return;
        }

        var auth = await _asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, user.Name);
    }
}
