using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers.UnitTests;

public class RankingFillerTests
{
    private RankingFiller _sut;
    private Mock<IDriverRepository> _driverRepositoryMock;
    private Mock<IFillersBag> _fillersBagMock;

    [SetUp]
    public void Setup()
    {
        _driverRepositoryMock = new Mock<IDriverRepository>();
        _fillersBagMock = new Mock<IFillersBag>();
        _sut = new(_driverRepositoryMock.Object);
    }

    [Test]
    public async Task FillRecursive_RankingAlreadyInContext_RankingNotFilled()
    {
        //Arrange
        RankingSnapshot ranking = AnyRanking();
        FillingContext context = new();
        context.Rankings.Add(ranking.Id, ranking);

        //Act
        await _sut.FillRecursive(ranking, context, _fillersBagMock.Object);

        //Assert
        _driverRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_EntriesWithoutDrivers_GetsDrivesAndFillsThem()
    {
        //Arrange
        Driver driver1 = AnyDriver();
        Driver driver2 = AnyDriver();
        RankingEntry entry1 = AnyRankingEntry(driver1.Id);
        RankingEntry entry2 = AnyRankingEntry(driver2.Id);
        RankingSnapshot ranking = AnyRanking(entry1, entry2);

        FillingContext context = new();
        _driverRepositoryMock.Setup(x => x.GetById(driver1.Id)).ReturnsAsync(driver1);
        _driverRepositoryMock.Setup(x => x.GetById(driver2.Id)).ReturnsAsync(driver2);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act
        await _sut.FillRecursive(ranking, context, _fillersBagMock.Object);

        //Assert
        Assert.That(entry1.Driver, Is.EqualTo(driver1));
        Assert.That(entry2.Driver, Is.EqualTo(driver2));
        _driverRepositoryMock.Verify(x => x.GetById(driver1.Id), Times.Exactly(2));
        _driverRepositoryMock.Verify(x => x.GetById(driver2.Id), Times.Exactly(2));
        driverFillerMock.Verify(x => x.FillRecursive(driver1, context, _fillersBagMock.Object), Times.Once());
        driverFillerMock.Verify(x => x.FillRecursive(driver2, context, _fillersBagMock.Object), Times.Once());
    }

    [Test]
    public async Task FillRecursive_DriverAlreadyInContext_UseDriverFromContext()
    {
        //Arrange
        Driver driver = AnyDriver();
        RankingEntry entry = AnyRankingEntry(driver.Id);
        RankingSnapshot ranking = AnyRanking(entry);

        FillingContext context = new();
        context.Drivers.Add(driver.Id, driver);

        _driverRepositoryMock.Setup(x => x.GetById(driver.Id)).ReturnsAsync(driver);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act
        await _sut.FillRecursive(ranking, context, _fillersBagMock.Object);

        //Assert
        Assert.That(entry.Driver, Is.EqualTo(driver));
        _driverRepositoryMock.Verify(x => x.GetById(driver.Id), Times.Once());
        driverFillerMock.Verify(x => x.FillRecursive(It.IsAny<Driver>(), It.IsAny<FillingContext>(), It.IsAny<IFillersBag>()), Times.Never());
    }

    [Test]
    public void FillRecursive_DriverNotInContextAndUnknownId_ThrowsException()
    {
        //Arrange
        RankingEntry entry = AnyRankingEntry(Guid.NewGuid());
        RankingSnapshot ranking = AnyRanking(entry);
        FillingContext context = new();

        //Act-Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.FillRecursive(ranking, context, _fillersBagMock.Object));
    }

    private static RankingSnapshot AnyRanking(params RankingEntry[] entries)
    {
        var ranking = new RankingSnapshot()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now
        };
        foreach (var entry in entries)
        {
            ranking.Entries.Add(entry);
        }
        return ranking;
    }

    private static RankingEntry AnyRankingEntry(Guid? driverId = null) => new RankingEntry()
    {
        Place = 1,
        DriverId = driverId.HasValue ? driverId.Value : Guid.NewGuid(),
        GeneralPoints = 10,
        GeneralTop30Count = 1,
        GeneralTop10Count = 1,
        GeneralTop3Count = 1,
        GeneralTop1Count = 0,
        CarA8Points = 0,
        CarA8PointsTop5Count = 0,
        CarA8PointsTop1Count = 0,
        CarA7Points = 0,
        CarA7PointsTop5Count = 0,
        CarA7PointsTop1Count = 0,
        CarA6Points = 0,
        CarA6PointsTop5Count = 0,
        CarA6PointsTop1Count = 0,
        CarA5Points = 0,
        CarA5PointsTop5Count = 0,
        CarA5PointsTop1Count = 0,
        CarBonusPoints = 0,
        CarBonusPointsTop5Count = 0,
        CarBonusPointsTop1Count = 0,
        BonusPoints = 0
    };

    private static Driver AnyDriver() => new Driver()
    {
        Id = Guid.NewGuid(),
        Known = false
    };
}
