using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests;

public class NotificationMapperTests
{
    private NotificationMapper _notificationMapper;

    [SetUp]
    public void Setup()
    {
        _notificationMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        NotificationModel model = AnyNotificationModel();

        //Act
        var result = _notificationMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.UserId, Is.EqualTo(model.UserId));
            Assert.That(result.Message, Is.EqualTo(model.Message));
            Assert.That(result.Created.Year, Is.EqualTo(2024));
            Assert.That(result.Created.Month, Is.EqualTo(6));
            Assert.That(result.Created.Day, Is.EqualTo(15));
        }
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        Notification notification = AnyNotification();

        //Act
        var result = _notificationMapper.ToCosmosModel(notification);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(notification.Id));
            Assert.That(result.UserId, Is.EqualTo(notification.UserId));
            Assert.That(result.Message, Is.EqualTo(notification.Message));
            Assert.That(result.Created, Is.Not.Empty);
        }
    }

    [Test]
    public void RoundTrip_FromDomainToCosmosAndBack_PreservesData()
    {
        //Arrange
        Notification original = AnyNotification();

        //Act
        NotificationModel model = _notificationMapper.ToCosmosModel(original);
        Notification restored = _notificationMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(restored.Id, Is.EqualTo(original.Id));
            Assert.That(restored.UserId, Is.EqualTo(original.UserId));
            Assert.That(restored.Message, Is.EqualTo(original.Message));
            Assert.That(restored.Created.Date, Is.EqualTo(original.Created.Date));
        }
    }

    private static NotificationModel AnyNotificationModel() => new NotificationModel()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        UserId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Message = "Test notification",
        Created = "15/06/2024"
    };

    private static Notification AnyNotification() => new Notification()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        UserId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Message = "Test notification",
        Created = new DateTime(2024, 6, 15, 10, 30, 0)
    };
}
