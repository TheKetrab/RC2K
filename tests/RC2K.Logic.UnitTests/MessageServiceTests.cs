using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.Logic.UnitTests;

public class MessageServiceTests
{
    private MessageService _sut;
    private Mock<IMessageRepository> _cronMessageRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _cronMessageRepositoryMock = new Mock<IMessageRepository>();
        _sut = new(_cronMessageRepositoryMock.Object);
    }

    [Test]
    public async Task GetAll_CallsRepositoryAndReturnsItsResult()
    {
        //Arrange
        List<DateTimeMessage> cronMessages = [AnyDateTimeMessage(), AnyDateTimeMessage()];

        _cronMessageRepositoryMock.Setup(x => x.GetAll())
            .Returns(Task.FromResult(cronMessages));

        //Act
        var result = await _sut.GetAll();

        //Assert
        _cronMessageRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        Assert.That(result, Is.EqualTo(cronMessages));
    }

    private DateTimeMessage AnyDateTimeMessage() =>
        new() 
        {
            Id = Guid.NewGuid(),
            Value = "Test Message",
            Published = false,
            DateTime = new DateTime(2026,10,10)
        };
}
