using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers.UnitTests;

public class TimeEntryFillerTests
{
    private TimeEntryFiller _sut;
    private Mock<ICarRepository> _carRepositoryMock;
    private Mock<IDriverRepository> _driverRepositoryMock;
    private Mock<IStageRepository> _stageRepositoryMock;
    private Mock<IVerifyInfoRepository> _verifyInfoRepositoryMock;
    private Mock<IFillersBag> _fillersBagMock;

    [SetUp]
    public void Setup()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _driverRepositoryMock = new Mock<IDriverRepository>();
        _stageRepositoryMock = new Mock<IStageRepository>();
        _verifyInfoRepositoryMock = new Mock<IVerifyInfoRepository>();
        _fillersBagMock = new Mock<IFillersBag>();
        _sut = new(_carRepositoryMock.Object, _driverRepositoryMock.Object, _stageRepositoryMock.Object, _verifyInfoRepositoryMock.Object);
    }

    [Test]
    public async Task FillRecursive_TimeEntryAlreadyInContext_TimeEntryNotFilled()
    {
        //Arrange
        TimeEntry timeEntry = AnyTimeEntry();
        FillingContext context = new();
        context.TimeEntries.Add(timeEntry.Id, timeEntry);

        //Act
        await _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object);

        //Assert
        Assert.That(timeEntry.Driver, Is.Null);
        Assert.That(timeEntry.VerifyInfo, Is.Null);
        _driverRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
        _verifyInfoRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_DriverAlreadyInContext_UseDriverFromContext()
    {
        //Arrange
        Driver driver = AnyDriver();
        Stage stage = AnyStage();
        Car car = AnyCar();
        TimeEntry timeEntry = AnyTimeEntry(driver.Id, stage.Id, car.Id);
        FillingContext context = new();
        context.Drivers.Add(driver.Id, driver);

        _stageRepositoryMock.Setup(x => x.GetById(stage.Id)).ReturnsAsync(stage);
        _carRepositoryMock.Setup(x => x.GetById(car.Id)).ReturnsAsync(car);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act
        await _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object);

        //Assert
        Assert.That(timeEntry.Driver, Is.EqualTo(driver));
        _driverRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
        driverFillerMock.Verify(x => x.FillRecursive(It.IsAny<Driver>(), It.IsAny<FillingContext>(), It.IsAny<IFillersBag>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_DriverNotInContext_GetsDriverAndFillsIt()
    {
        //Arrange
        Driver driver = AnyDriver();
        Stage stage = AnyStage();
        Car car = AnyCar();
        TimeEntry timeEntry = AnyTimeEntry(driver.Id, stage.Id, car.Id);
        FillingContext context = new();

        _driverRepositoryMock.Setup(x => x.GetById(driver.Id)).ReturnsAsync(driver);
        _stageRepositoryMock.Setup(x => x.GetById(stage.Id)).ReturnsAsync(stage);
        _carRepositoryMock.Setup(x => x.GetById(car.Id)).ReturnsAsync(car);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act
        await _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object);

        //Assert
        Assert.That(timeEntry.Driver, Is.EqualTo(driver));
        _driverRepositoryMock.Verify(x => x.GetById(driver.Id), Times.Once());
        driverFillerMock.Verify(x => x.FillRecursive(driver, context, _fillersBagMock.Object), Times.Once());
    }

    [Test]
    public async Task FillRecursive_VerifyInfoWithoutId_VerifyInfoNotFilled()
    {
        //Arrange
        Driver driver = AnyDriver();
        Stage stage = AnyStage();
        Car car = AnyCar();
        TimeEntry timeEntry = AnyTimeEntry(driver.Id, stage.Id, car.Id, verifyInfoId: null);
        FillingContext context = new();

        _driverRepositoryMock.Setup(x => x.GetById(driver.Id)).ReturnsAsync(driver);
        _stageRepositoryMock.Setup(x => x.GetById(stage.Id)).ReturnsAsync(stage);
        _carRepositoryMock.Setup(x => x.GetById(car.Id)).ReturnsAsync(car);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act
        await _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object);

        //Assert
        Assert.That(timeEntry.VerifyInfo, Is.Null);
        _verifyInfoRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_VerifyInfoAlreadyInContext_UseVerifyInfoFromContext()
    {
        //Arrange
        Driver driver = AnyDriver();
        VerifyInfo verifyInfo = AnyVerifyInfo();
        Stage stage = AnyStage();
        Car car = AnyCar();
        TimeEntry timeEntry = AnyTimeEntry(driver.Id, stage.Id, car.Id, verifyInfo.Id);
        FillingContext context = new();
        context.VerifyInfos.Add(verifyInfo.Id, verifyInfo);

        _driverRepositoryMock.Setup(x => x.GetById(driver.Id)).ReturnsAsync(driver);
        _stageRepositoryMock.Setup(x => x.GetById(stage.Id)).ReturnsAsync(stage);
        _carRepositoryMock.Setup(x => x.GetById(car.Id)).ReturnsAsync(car);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act
        await _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object);

        //Assert
        Assert.That(timeEntry.VerifyInfo, Is.EqualTo(verifyInfo));
        _verifyInfoRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_VerifyInfoNotInContext_GetsVerifyInfoAndFillsIt()
    {
        //Arrange
        Driver driver = AnyDriver();
        VerifyInfo verifyInfo = AnyVerifyInfo();
        Stage stage = AnyStage();
        Car car = AnyCar();
        TimeEntry timeEntry = AnyTimeEntry(driver.Id, stage.Id, car.Id, verifyInfo.Id);
        FillingContext context = new();

        _driverRepositoryMock.Setup(x => x.GetById(driver.Id)).ReturnsAsync(driver);
        _stageRepositoryMock.Setup(x => x.GetById(stage.Id)).ReturnsAsync(stage);
        _carRepositoryMock.Setup(x => x.GetById(car.Id)).ReturnsAsync(car);
        _verifyInfoRepositoryMock.Setup(x => x.GetById(verifyInfo.Id)).ReturnsAsync(verifyInfo);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        Mock<IVerifyInfoFiller> verifyInfoFillerMock = new Mock<IVerifyInfoFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);
        _fillersBagMock.Setup(x => x.VerifyInfoFiller).Returns(verifyInfoFillerMock.Object);

        //Act
        await _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object);

        //Assert
        Assert.That(timeEntry.VerifyInfo, Is.EqualTo(verifyInfo));
        _verifyInfoRepositoryMock.Verify(x => x.GetById(verifyInfo.Id), Times.Once());
        verifyInfoFillerMock.Verify(x => x.FillRecursive(verifyInfo, context, _fillersBagMock.Object), Times.Once());
    }

    [Test]
    public void FillRecursive_DriverNotInContextAndUnknownId_ThrowsException()
    {
        //Arrange
        Stage stage = AnyStage();
        Car car = AnyCar();
        TimeEntry timeEntry = AnyTimeEntry(Guid.NewGuid(), stage.Id, car.Id);
        FillingContext context = new();

        _stageRepositoryMock.Setup(x => x.GetById(stage.Id)).ReturnsAsync(stage);
        _carRepositoryMock.Setup(x => x.GetById(car.Id)).ReturnsAsync(car);

        //Act-Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object));
    }

    [Test]
    public void FillRecursive_VerifyInfoNotInContextAndUnknownId_ThrowsException()
    {
        //Arrange
        Driver driver = AnyDriver();
        Stage stage = AnyStage();
        Car car = AnyCar();
        TimeEntry timeEntry = AnyTimeEntry(driver.Id, stage.Id, car.Id, Guid.NewGuid());
        FillingContext context = new();

        _driverRepositoryMock.Setup(x => x.GetById(driver.Id)).ReturnsAsync(driver);
        _stageRepositoryMock.Setup(x => x.GetById(stage.Id)).ReturnsAsync(stage);
        _carRepositoryMock.Setup(x => x.GetById(car.Id)).ReturnsAsync(car);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act-Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.FillRecursive(timeEntry, context, _fillersBagMock.Object));
    }

    private static TimeEntry AnyTimeEntry(Guid? driverId = null, int? stageId = null, int? carId = null, Guid? verifyInfoId = null) => new TimeEntry()
    {
        Id = Guid.NewGuid(),
        StageId = stageId.HasValue ? stageId.Value : 1,
        CarId = carId.HasValue ? carId.Value : 1,
        DriverId = driverId.HasValue ? driverId.Value : Guid.NewGuid(),
        Time = TimeOnly.FromTimeSpan(new TimeSpan(1, 2, 3)),
        UploadTime = DateTime.Now,
        VerifyInfoId = verifyInfoId,
        Labels = ""
    };

    private static Driver AnyDriver() => new Driver()
    {
        Id = Guid.NewGuid(),
        Known = false
    };

    private static Stage AnyStage() => new Stage()
    {
        Id = 1,
        Code = 1,
        Direction = Direction.Arcade
    };

    private static Car AnyCar() => new Car()
    {
        Id = 1,
        Name = "Car 1",
        Class = 8
    };

    private static VerifyInfo AnyVerifyInfo() => new VerifyInfo()
    {
        Id = Guid.NewGuid(),
        VerifierId = Guid.NewGuid(),
        Comment = "",
        VerifyDate = DateTime.Now
    };
}
