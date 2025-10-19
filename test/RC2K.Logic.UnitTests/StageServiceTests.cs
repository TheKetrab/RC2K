using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.Logic.UnitTests;

public class StageServiceTests
{
    private StageService _sut;
    private Mock<IStageRepository> _stageRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _stageRepositoryMock = new Mock<IStageRepository>();
        _sut = new(_stageRepositoryMock.Object);
    }

    [Test]
    public async Task GetAllFilled_CallsStageRepositoryAndReturnsItsResult()
    {
        //Arrange
        List<Stage> stages = [];
        _stageRepositoryMock.Setup(x => x.GetAll()).Returns(Task.FromResult(stages));

        //Act
        List<Stage> result = await _sut.GetAllFilled();

        //Assert
        _stageRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        Assert.That(result, Is.EqualTo(stages));
    }

    [TestCase(RallyCode.Sony, 21, 26)]
    [TestCase(RallyCode.Vauxhall, 41, 46)]
    [TestCase(RallyCode.Pirelli, 61, 66)]
    [TestCase(RallyCode.Scottish, 31, 36)]
    [TestCase(RallyCode.Seat, 71, 76)]
    [TestCase(RallyCode.Stena, 51, 56)]
    public async Task GetAllFilledByRallyCode_CallsStageRepositoryWithProperBoundaries(RallyCode rallyCode, int expectedFrom, int expectedTo)
    {
        //Arrange
        List<Stage> stages = [];
        _stageRepositoryMock.Setup(x => x.GetAllByRallyCodeBetween(expectedFrom, expectedTo))
            .Returns(Task.FromResult(stages));

        //Act
        List<Stage> result = await _sut.GetAllFilledByRallyCode(rallyCode);

        //Assert
        _stageRepositoryMock.Verify(x => x.GetAllByRallyCodeBetween(expectedFrom, expectedTo), Times.Once());
        Assert.That(result, Is.EqualTo(stages));
    }

    [Test]
    public void GetAllFilledByRallyCode_UnknownRallyCode_ThrowsException() =>
        //Arrange-Act-Assert
        Assert.Throws<Exception>(() => _sut.GetAllFilledByRallyCode((RallyCode)(-1)));

}