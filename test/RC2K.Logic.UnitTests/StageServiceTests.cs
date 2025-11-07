using Moq;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.Logic.UnitTests;

public class StageServiceTests
{
    private StageService _sut;
    private Mock<IRallyUoW> _rallyUoWMock;
    private Mock<IStageRepository> _stageRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _rallyUoWMock = new Mock<IRallyUoW>();
        _stageRepositoryMock = new Mock<IStageRepository>();
        
        _rallyUoWMock
            .Setup(x => x.Stages)
            .Returns(_stageRepositoryMock.Object);
        
        _sut = new(_rallyUoWMock.Object);
    }

    [Test]
    public async Task GetAll_CallsStageRepositoryAndReturnsItsResult()
    {
        //Arrange
        List<Stage> stages = [];
        _stageRepositoryMock.Setup(x => x.GetAll()).Returns(Task.FromResult(stages));

        //Act
        List<Stage> result = await _sut.GetAll();

        //Assert
        _stageRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        Assert.That(result, Is.EqualTo(stages));
    }

    [Test]
    public async Task GetPath_CallsStageRepositoryAndReturnsItsResult()
    {
        //Arrange
        int stageCode = 42;
        string path = "path";
        _stageRepositoryMock.Setup(x => x.GetPathByStageCode(stageCode)).Returns(Task.FromResult<string?>(path));

        //Act
        var result = await _sut.GetPath(stageCode);

        //Assert
        Assert.That(result, Is.EqualTo(path));
        _stageRepositoryMock.Verify(x => x.GetPathByStageCode(stageCode), Times.Once());
    }

    [Test]
    public async Task SetPath_CallsStageRepositoryToUpdatePath()
    {
        //Arrange-Act
        await _sut.SetPath(10, "path");

        //Assert
        _stageRepositoryMock.Verify(x => x.UpdatePath(10, "path"), Times.Once());
    }

}