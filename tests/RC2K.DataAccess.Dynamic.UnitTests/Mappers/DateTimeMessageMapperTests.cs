using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests.Mappers;

public class DateTimeMessageMapperTests
{
    private DateTimeMessageMapper _messageMapper;

    [SetUp]
    public void Setup()
    {
        _messageMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        MessageModel model = AnyMessageModel();

        //Act
        var result = _messageMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.Name, Is.EqualTo(model.Name));
            Assert.That(result.Published, Is.EqualTo(model.Published));
            Assert.That(result.Value, Is.EqualTo(model.Message));
            Assert.That(result.DateTime, Is.EqualTo(new DateTime(2020, 12, 20, 12, 0, 0, 0, 0, DateTimeKind.Utc)));
        }
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        DateTimeMessage message = AnyMessage();

        //Act
        var result = _messageMapper.ToCosmosModel(message);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(message.Id));
            Assert.That(result.Name, Is.EqualTo(message.Name));
            Assert.That(result.Published, Is.EqualTo(message.Published));
            Assert.That(result.DateTime, Is.EqualTo("2020/12/20 12:00:00Z"));
        }
    }

    private static MessageModel AnyMessageModel() => new MessageModel()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        DateTime = "2020/12/20 12:00:00Z",
        Message = "MSG",
        Published = false,
        Name = "name",
    };

    private static DateTimeMessage AnyMessage() => new DateTimeMessage()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        DateTime = new DateTime(2020, 12, 20, 12, 0, 0, 0, DateTimeKind.Utc),
        Published = false,
        Value = "MSG",
        Name = "name"
    };
}
