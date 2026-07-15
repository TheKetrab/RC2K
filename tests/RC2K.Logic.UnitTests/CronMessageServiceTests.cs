using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.Logic.UnitTests;

public class CronMessageServiceTests
{
    private CronMessageService _sut;
    private Mock<ICronMessageRepository> _cronMessageRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _cronMessageRepositoryMock = new Mock<ICronMessageRepository>();
        _sut = new(_cronMessageRepositoryMock.Object);
    }

    [Test]
    public async Task GetAll_CallsRepositoryAndReturnsItsResult()
    {
        //Arrange
        List<CronMessage> cronMessages = [AnyCronMessage(), AnyCronMessage()];

        _cronMessageRepositoryMock.Setup(x => x.GetAll())
            .Returns(Task.FromResult(cronMessages));

        //Act
        var result = await _sut.GetAll();

        //Assert
        _cronMessageRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        Assert.That(result, Is.EqualTo(cronMessages));
    }

    private CronMessage AnyCronMessage() =>
        new() 
        {
            Id = Guid.NewGuid(),
            Message = "Test Message",
            Published = false,
            Cron = "* * * * *"
        };
}
