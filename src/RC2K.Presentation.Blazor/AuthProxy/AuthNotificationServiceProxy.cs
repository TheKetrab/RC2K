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

    public async Task Delete(Guid id)
    {
        await AuthorizeSelf(id);

        await _service.Delete(id);
    }

    public async Task<List<Notification>> GetUserNotifications(string userName)
    {
        var auth = await _asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, userName);

        return await _service.GetUserNotifications(userName);
    }

    public Task NotifyMasterAdmin(string message) =>
        _service.NotifyMasterAdmin(message);

    private async Task AuthorizeSelf(Guid notificationId)
    {
        Notification? notification = await _notificationRepository.GetById(notificationId);
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
