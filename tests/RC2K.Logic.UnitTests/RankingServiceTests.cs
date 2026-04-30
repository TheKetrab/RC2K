using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;
using Microsoft.Extensions.Logging;

namespace RC2K.Logic.UnitTests;

public class RankingServiceTests
{
    private RankingService _sut;
    private Mock<IRankingsRepository> _rankingRepositoryMock;
    private Mock<IStageService> _stageServiceMock;
    private Mock<ITimeEntryService> _timeEntryServiceMock;
    private Mock<IPointsProvider> _pointsProviderMock;
    private Mock<IBonusPointsService> _bonusPointsServiceMock;
    private Mock<IFillersBag> _fillersBagMock;
    private Mock<ILogger<RankingService>> _loggerMock;
    private Mock<IRankingFiller> _rankingFillerMock;

    [SetUp]
    public void Setup()
    {
        _rankingRepositoryMock = new Mock<IRankingsRepository>();
        _stageServiceMock = new Mock<IStageService>();
        _timeEntryServiceMock = new Mock<ITimeEntryService>();
        _pointsProviderMock = new Mock<IPointsProvider>();
        _bonusPointsServiceMock = new Mock<IBonusPointsService>();
        _fillersBagMock = new Mock<IFillersBag>();
        _loggerMock = new Mock<ILogger<RankingService>>();
        _rankingFillerMock = new Mock<IRankingFiller>();

        _fillersBagMock.Setup(x => x.RankingFiller).Returns(_rankingFillerMock.Object);

        _sut = new(_rankingRepositoryMock.Object, _stageServiceMock.Object, 
                   _timeEntryServiceMock.Object, _pointsProviderMock.Object, 
                   _bonusPointsServiceMock.Object, _fillersBagMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task GetLatest_CallsRepositoryAndFillsData()
    {
        //Arrange
        RankingSnapshot ranking = new RankingSnapshot 
        { 
            Id = Guid.NewGuid(),
            Date = DateTime.UtcNow
        };
        
        _rankingRepositoryMock.Setup(x => x.GetCurrent()).Returns(Task.FromResult(ranking));
        _rankingFillerMock.Setup(x => x.FillRecursive(It.IsAny<RankingSnapshot>(), It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.GetLatest();

        //Assert
        _rankingRepositoryMock.Verify(x => x.GetCurrent(), Times.Once());
        _rankingFillerMock.Verify(x => x.FillRecursive(ranking, It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()), Times.Once());
        Assert.That(result, Is.EqualTo(ranking));
    }

}
