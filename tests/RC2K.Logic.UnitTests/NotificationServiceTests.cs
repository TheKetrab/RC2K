using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using Microsoft.Extensions.Logging;

namespace RC2K.Logic.UnitTests;

public class NotificationServiceTests
{
    private NotificationService _sut;
    private Mock<INotificationRepository> _notificationRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ILogger<NotificationService>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _notificationRepositoryMock = new Mock<INotificationRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<NotificationService>>();

        _sut = new(_notificationRepositoryMock.Object, _userRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetUserNotifications_ReturnsNotificationsForUser()
    {
        //Arrange
        string userName = "TestUser";
        Guid userId = Guid.NewGuid();
        User user = new User 
        { 
            Id = userId, 
            Name = userName,
            DriverId = Guid.NewGuid(),
            Roles = ["user"],
            Email = "test@example.com"
        };
        List<Notification> notifications = [];
        
        _userRepositoryMock.Setup(x => x.GetByName(userName)).Returns(Task.FromResult<User?>(user));
        _notificationRepositoryMock.Setup(x => x.GetNotifications(userId)).Returns(Task.FromResult(notifications));

        //Act
        var result = await _sut.GetUserNotifications(userName);

        //Assert
        _userRepositoryMock.Verify(x => x.GetByName(userName), Times.Once());
        _notificationRepositoryMock.Verify(x => x.GetNotifications(userId), Times.Once());
        Assert.That(result, Is.EqualTo(notifications));
    }

    [Test]
    public async Task GetUserNotifications_ReturnsEmptyListWhenUserNotFound()
    {
        //Arrange
        string userName = "NonExistentUser";
        
        _userRepositoryMock.Setup(x => x.GetByName(userName)).Returns(Task.FromResult<User?>(null));

        //Act
        var result = await _sut.GetUserNotifications(userName);

        //Assert
        Assert.That(result, Is.Empty);
        _notificationRepositoryMock.Verify(x => x.GetNotifications(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task NotifyMasterAdmin_SendsNotificationToMasterAdmin()
    {
        //Arrange
        string message = "Test notification";
        Guid adminId = Guid.NewGuid();
        User admin = new User 
        { 
            Id = adminId, 
            Name = "TheKetrab",
            DriverId = Guid.NewGuid(),
            Roles = ["admin"],
            Email = "admin@example.com"
        };
        
        _userRepositoryMock.Setup(x => x.GetByName("TheKetrab")).Returns(Task.FromResult<User?>(admin));
        _notificationRepositoryMock.Setup(x => x.Create(It.IsAny<Notification>())).Returns(Task.CompletedTask);

        //Act
        await _sut.NotifyMasterAdmin(message);

        //Assert
        _userRepositoryMock.Verify(x => x.GetByName("TheKetrab"), Times.Once());
        _notificationRepositoryMock.Verify(x => x.Create(It.Is<Notification>(n => 
            n.UserId == adminId && n.Message == message)), Times.Once());
    }

    [Test]
    public async Task NotifyMasterAdmin_LogsErrorWhenAdminNotFound()
    {
        //Arrange
        string message = "Test notification";
        
        _userRepositoryMock.Setup(x => x.GetByName("TheKetrab")).Returns(Task.FromResult<User?>(null));

        //Act
        await _sut.NotifyMasterAdmin(message);

        //Assert
        _userRepositoryMock.Verify(x => x.GetByName("TheKetrab"), Times.Once());
        _notificationRepositoryMock.Verify(x => x.Create(It.IsAny<Notification>()), Times.Never());
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Master admin user 'TheKetrab' not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    [Test]
    public async Task Create_CreatesNotificationWithProvidedData()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        string message = "Test message";
        
        _notificationRepositoryMock.Setup(x => x.Create(It.IsAny<Notification>())).Returns(Task.CompletedTask);

        //Act
        await _sut.Create(userId, message);

        //Assert
        _notificationRepositoryMock.Verify(x => x.Create(It.Is<Notification>(n => 
            n.UserId == userId && n.Message == message)), Times.Once());
    }

    [Test]
    public async Task Delete_CallsRepositoryWithProvidedId()
    {
        //Arrange
        Guid id = Guid.NewGuid();
        
        _notificationRepositoryMock.Setup(x => x.Delete(id.ToString())).Returns(Task.CompletedTask);

        //Act
        await _sut.Delete(id);

        //Assert
        _notificationRepositoryMock.Verify(x => x.Delete(id.ToString()), Times.Once());
    }
}
