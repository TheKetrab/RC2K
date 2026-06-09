using Microsoft.AspNetCore.Components.Authorization;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;

namespace RC2K.Presentation.Blazor.AuthProxy;

public class AuthNotificationServiceProxy(
    AuthenticationStateProvider asp,
    NotificationService service,
    INotificationRepository notificationRepository,
    IUserService userService)
    : INotificationService
{
    public event EventHandler<Guid>? NotificationsUpdated
    {
        add => service.NotificationsUpdated += value;
        remove => service.NotificationsUpdated -= value;
    }
    
    public Task Create(Guid userId, string message) =>
        service.Create(userId, message);

    public async Task Delete(Guid userId, Guid id)
    {
        await AuthorizeSelf(id);
        await service.Delete(userId, id);
    }

    public async Task DeleteMany(List<Guid> userIds, List<Guid> ids)
    {
        foreach (var id in ids)
        {
            await AuthorizeSelf(id);
        }

        await service.DeleteMany(userIds, ids);
    }

    public async Task<List<Notification>> GetUserNotifications(string userName)
    {
        var auth = await asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, userName);

        return await service.GetUserNotifications(userName);
    }

    public async Task<int> GetUserNotificationsCount(string userName)
    {
        var auth = await asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, userName);

        return await service.GetUserNotificationsCount(userName);
    }

    public Task NotifyMasterAdmin(string message) =>
        service.NotifyMasterAdmin(message);

    private async Task AuthorizeSelf(Guid notificationId)
    {
        Notification? notification = await notificationRepository.GetById(notificationId, CancellationToken.None);
        if (notification == null) 
        {
            return;
        }
        User? user = await userService.GetById(notification.UserId);
        if (user == null)
        {
            return;
        }

        var auth = await asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, user.Name);
    }
}
