using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;
using Microsoft.Extensions.Logging;
using static RC2K.Utils.Utils;
using System.Runtime.CompilerServices;

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
        _rankingFillerMock
            .Setup(x => x.FillRecursive(
                It.IsAny<RankingSnapshot>(),
                It.IsAny<FillingContext>(),
                _fillersBagMock.Object,
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.GetLatest();

        //Assert
        _rankingRepositoryMock.Verify(x => x.GetCurrent(), Times.Once());
        _rankingFillerMock.Verify(x => x.FillRecursive(ranking, It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()), Times.Once());
        Assert.That(result, Is.EqualTo(ranking));
    }

    private TimeEntry AnyTe(Guid driverId, TimeOnly time, int carClass = 8)
    {
        return new TimeEntry()
        {
            Id = Guid.NewGuid(),
            CarId = 1,
            Car = new Car() { Id = 1, Name = "", Class = carClass },
            DriverId = driverId,
            StageId = 1,
            Time = time,
            UploadTime = DateTime.UtcNow,
        };
    }

    [Test]
    public async Task CalculateCurrentRankingSnapshot_GeneralPoints_AreCalculatedProperly()
    {
        _sut = new(_rankingRepositoryMock.Object, _stageServiceMock.Object,
           _timeEntryServiceMock.Object, new PointsProvider(),
           _bonusPointsServiceMock.Object, _fillersBagMock.Object, _loggerMock.Object);

        Stage stage = new Stage() { Code = 1, Id = 1, Direction = Direction.Simulation };
        _stageServiceMock.Setup(x => x.GetAll()).ReturnsAsync([stage]);

        Guid driver1 = Guid.NewGuid();
        Guid driver2 = Guid.NewGuid();
        Guid driver3 = Guid.NewGuid();
        Guid driver4 = Guid.NewGuid();
        Guid driver5 = Guid.NewGuid();
        Guid driver6 = Guid.NewGuid();
        Guid driver7 = Guid.NewGuid();
        Guid driver8 = Guid.NewGuid();
        Guid driver9 = Guid.NewGuid();
        Guid driver10 = Guid.NewGuid();
        Guid driver11 = Guid.NewGuid();

        _timeEntryServiceMock
            .Setup(x => x.Get(
                It.Is<int>(x => x == stage.Id),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                AnyTe(driver1, CentisecondsToTimeOnly(1)), // top1 ; +100
                AnyTe(driver2, CentisecondsToTimeOnly(2)),
                AnyTe(driver3, CentisecondsToTimeOnly(3)), // top3 ; +60
                AnyTe(driver4, CentisecondsToTimeOnly(4)), // top10 ; +50
                AnyTe(driver5, CentisecondsToTimeOnly(5)),
                AnyTe(driver6, CentisecondsToTimeOnly(6)),
                AnyTe(driver7, CentisecondsToTimeOnly(7)),
                AnyTe(driver8, CentisecondsToTimeOnly(8)),
                AnyTe(driver9, CentisecondsToTimeOnly(9)),
                AnyTe(driver10, CentisecondsToTimeOnly(10)),
                AnyTe(driver11, CentisecondsToTimeOnly(11)), // top30 ; +24
            ]);

        _bonusPointsServiceMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        //Act
        var result = await _sut.CalculateCurrentRankingSnapshot();

        //Assert
        var driver1Entry = result.Entries.First(x => x.DriverId == driver1);
        var driver3Entry = result.Entries.First(x => x.DriverId == driver3);
        var driver4Entry = result.Entries.First(x => x.DriverId == driver4);
        var driver11Entry = result.Entries.First(x => x.DriverId == driver11);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(driver1Entry.GeneralTop1Count, Is.EqualTo(1));
            Assert.That(driver1Entry.GeneralTop3Count, Is.EqualTo(1));
            Assert.That(driver1Entry.GeneralTop10Count, Is.EqualTo(1));
            Assert.That(driver1Entry.GeneralTop30Count, Is.EqualTo(1));
            Assert.That(driver1Entry.GeneralPoints, Is.EqualTo(100));

            Assert.That(driver3Entry.GeneralTop1Count, Is.Zero);
            Assert.That(driver3Entry.GeneralTop3Count, Is.EqualTo(1));
            Assert.That(driver3Entry.GeneralTop10Count, Is.EqualTo(1));
            Assert.That(driver3Entry.GeneralTop30Count, Is.EqualTo(1));
            Assert.That(driver3Entry.GeneralPoints, Is.EqualTo(60));

            Assert.That(driver4Entry.GeneralTop1Count, Is.Zero);
            Assert.That(driver4Entry.GeneralTop3Count, Is.Zero);
            Assert.That(driver4Entry.GeneralTop10Count, Is.EqualTo(1));
            Assert.That(driver4Entry.GeneralTop30Count, Is.EqualTo(1));
            Assert.That(driver4Entry.GeneralPoints, Is.EqualTo(50));

            Assert.That(driver11Entry.GeneralTop1Count, Is.Zero);
            Assert.That(driver11Entry.GeneralTop3Count, Is.Zero);
            Assert.That(driver11Entry.GeneralTop10Count, Is.Zero);
            Assert.That(driver11Entry.GeneralTop30Count, Is.EqualTo(1));
            Assert.That(driver11Entry.GeneralPoints, Is.EqualTo(24));
        }
    }

    [TestCase(5)]
    [TestCase(6)]
    [TestCase(7)]
    [TestCase(8)]
    public async Task CalculateCurrentRankingSnapshot_CarPoints_AreCalculatedProperly(int @class)
    {
        _sut = new(_rankingRepositoryMock.Object, _stageServiceMock.Object,
           _timeEntryServiceMock.Object, new PointsProvider(),
           _bonusPointsServiceMock.Object, _fillersBagMock.Object, _loggerMock.Object);

        Stage stage = new Stage() { Code = 1, Id = 1, Direction = Direction.Simulation };
        _stageServiceMock.Setup(x => x.GetAll()).ReturnsAsync([stage]);

        Guid driver1 = Guid.NewGuid();
        Guid driver2 = Guid.NewGuid();
        Guid driver3 = Guid.NewGuid();
        Guid driver4 = Guid.NewGuid();
        Guid driver5 = Guid.NewGuid();
        Guid driver6 = Guid.NewGuid();

        _timeEntryServiceMock
            .Setup(x => x.Get(
                It.Is<int>(x => x == stage.Id),
                It.IsAny<int?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([
                AnyTe(driver1, CentisecondsToTimeOnly(1), @class), // top1
                AnyTe(driver2, CentisecondsToTimeOnly(2), @class),
                AnyTe(driver3, CentisecondsToTimeOnly(3), @class), // top5
                AnyTe(driver4, CentisecondsToTimeOnly(4), @class),
                AnyTe(driver5, CentisecondsToTimeOnly(5), @class),
                AnyTe(driver6, CentisecondsToTimeOnly(6), @class), // not top5
            ]);

        _bonusPointsServiceMock.Setup(x => x.GetAll()).ReturnsAsync([]);

        //Act
        var result = await _sut.CalculateCurrentRankingSnapshot();

        //Assert
        var driver1Entry = result.Entries.First(x => x.DriverId == driver1);
        var driver3Entry = result.Entries.First(x => x.DriverId == driver3);
        var driver6Entry = result.Entries.First(x => x.DriverId == driver6);

        (Func<RankingEntry, int> top1CountSelector,
         Func<RankingEntry, int> top5CountSelector,
         Func<RankingEntry, int> pointsSelector,
         Func<int,int,int> class2points) = @class switch
        {
            5 => ((Func<RankingEntry, int>)(re => re.CarA5PointsTop1Count),
                  (Func<RankingEntry, int>)(re => re.CarA5PointsTop5Count),
                  (Func<RankingEntry, int>)(re => re.CarA5Points),
                  (Func<int,int,int>)((int @class, int p) => p > 5 ? 0 : PointsProvider._a5carPoints[p-1])),
            6 => ((Func<RankingEntry, int>)(re => re.CarA6PointsTop1Count),
                  (Func<RankingEntry, int>)(re => re.CarA6PointsTop5Count),
                  (Func<RankingEntry, int>)(re => re.CarA6Points),
                  (Func<int, int, int>)((int @class, int p) => p > 5 ? 0 : PointsProvider._a6carPoints[p-1])),
            7 => ((Func<RankingEntry, int>)(re => re.CarA7PointsTop1Count),
                  (Func<RankingEntry, int>)(re => re.CarA7PointsTop5Count),
                  (Func<RankingEntry, int>)(re => re.CarA7Points),
                  (Func<int, int, int>)((int @class, int p) => p > 5 ? 0 : PointsProvider._a7carPoints[p-1])),
            8 => ((Func<RankingEntry, int>)(re => re.CarA8PointsTop1Count),
                  (Func<RankingEntry, int>)(re => re.CarA8PointsTop5Count),
                  (Func<RankingEntry, int>)(re => re.CarA8Points),
                  (Func<int, int, int>)((int @class, int p) => p > 5 ? 0 : PointsProvider._a8carPoints[p-1])),

            _ => throw new SwitchExpressionException($"Unsupported class: {@class}")
        };

        using (Assert.EnterMultipleScope())
        {
            Assert.That(top1CountSelector(driver1Entry), Is.EqualTo(1));
            Assert.That(top5CountSelector(driver1Entry), Is.EqualTo(1));
            Assert.That(pointsSelector(driver1Entry), Is.EqualTo(class2points(@class, 1)));

            Assert.That(top1CountSelector(driver3Entry), Is.Zero);
            Assert.That(top5CountSelector(driver3Entry), Is.EqualTo(1));
            Assert.That(pointsSelector(driver3Entry), Is.EqualTo(class2points(@class, 3)));

            Assert.That(top1CountSelector(driver6Entry), Is.Zero);
            Assert.That(top5CountSelector(driver6Entry), Is.Zero);
            Assert.That(pointsSelector(driver6Entry), Is.Zero);
        }
    }

}
