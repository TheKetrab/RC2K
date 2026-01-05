using Microsoft.AspNetCore.Components.Authorization;
using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;

namespace RC2K.Presentation.Blazor.AuthProxy;

public class AuthNotificationServiceProxy : INotificationService
{
    private AuthenticationStateProvider _asp;
    private NotificationService _service;
    private IUserService _userService;

    public AuthNotificationServiceProxy(
        AuthenticationStateProvider asp,
        NotificationService service,
        IUserService userService)
    {
        _asp = asp;
        _service = service;
        _userService = userService;
    }

    public Task Create(Guid userId, string message) =>
        _service.Create(userId, message);

    public async Task Delete(Guid id)
    {
        await AuthorizeSelf(id);

        await _service.Delete(id);
    }

    public async Task<Notification?> GetById(Guid id)
    {
        return await _service.GetById(id);
    }

    public Task<List<Notification>> GetUserNotifications(string userName) =>
        _service.GetUserNotifications(userName);

    public Task NotifyMasterAdmin(string message) =>
        _service.NotifyMasterAdmin(message);

    private async Task AuthorizeSelf(Guid notificationId)
    {
        Notification? notification = await _service.GetById(notificationId);
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
