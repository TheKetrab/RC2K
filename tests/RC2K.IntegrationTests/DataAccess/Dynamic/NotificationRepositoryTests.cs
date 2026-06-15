using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;

using Notification = RC2K.DomainModel.Notification;

namespace RC2K.IntegrationTests.DataAccess.Dynamic;

public class NotificationRepositoryTests
{
    private NotificationRepository _sut;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _sut = (NotificationRepository)
            IntegrationFixture.ServiceProvider.GetRequiredService<INotificationRepository>();
    }

    [Test]
    public async Task GetNotificationsAndGetCount_ReturnsAllNotificationsForUser()
    {
        //Arrange
        var userRepository = IntegrationFixture.ServiceProvider.GetRequiredService<IUserRepository>();
        Guid userId = (await userRepository.GetByName(Constants.TestUserName))!.Id;

        Notification notification = new() { Id = Guid.NewGuid(), Message = "Test", UserId = userId };
        await using var _ = await NotificationScope.CreateTempNotification(_sut, notification);

        //Act-Assert
        var result1 = await _sut.GetNotifications(userId);
        Assert.That(result1, Is.Not.Null);

        //Act-Assert
        var result2 = await _sut.GetNotificationsCount(userId);
        Assert.That(result2, Is.EqualTo(result1.Count));
    }

    private class NotificationScope : IAsyncDisposable
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly Notification _notification;

        private NotificationScope(INotificationRepository notificationRepository, Notification notification)
        {
            _notificationRepository = notificationRepository;
            _notification = notification;
        }

        public static async Task<NotificationScope> CreateTempNotification(
            INotificationRepository notificationRepository,
            Notification notification)
        {
            var scope = new NotificationScope(notificationRepository, notification);
            await notificationRepository.Create(notification);
            return scope;
        }

        public async ValueTask DisposeAsync() 
        { 
            await _notificationRepository.Delete(_notification.Id.ToString());
        }
    }
}
