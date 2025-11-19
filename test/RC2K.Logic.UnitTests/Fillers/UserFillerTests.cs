using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers.UnitTests;

public class UserFillerTests
{
    private UserFiller _sut;
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
    public async Task FillRecursive_UserAlreadyInContext_UserDriverIsNotFilled()
    {
        //Arrange
        User user = AnyUser();
        FillingContext context = new();
        context.Users.Add(user.Id, user);

        //Act
        await _sut.FillRecursive(user, context, _fillersBagMock.Object);

        //Assert
        Assert.That(user.Driver, Is.Null);
    }

    [Test]
    public async Task FillRecursive_DriverAlreadyInContext_UseDriverFromContext()
    {
        //Arrange
        Driver driver = AnyDriver();
        User user = AnyUser(driver.Id);
        FillingContext context = new();
        context.Drivers.Add(driver.Id, driver);

        //Act
        await _sut.FillRecursive(user, context, _fillersBagMock.Object);

        //Assert
        Assert.That(user.Driver, Is.EqualTo(driver));
        _driverRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_DriverNotInContext_GetsDriverAndFillsIt()
    {
        //Arrange
        Driver driver = AnyDriver();
        User user = AnyUser(driver.Id);
        FillingContext context = new();
        _driverRepositoryMock.Setup(x => x.GetById(driver.Id)).ReturnsAsync(driver);

        Mock<IDriverFiller> driverFillerMock = new Mock<IDriverFiller>();
        _fillersBagMock.Setup(x => x.DriverFiller).Returns(driverFillerMock.Object);

        //Act
        await _sut.FillRecursive(user, context, _fillersBagMock.Object);

        //Assert
        Assert.That(user.Driver, Is.EqualTo(driver));
        _driverRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once());
        driverFillerMock.Verify(x => x.FillRecursive(driver, context, _fillersBagMock.Object), Times.Once());
    }

    [Test]
    public void FillRecursive_DriverNotInContextAndUnknownId_ThrowsException()
    {
        //Arrange
        User user = AnyUser();
        FillingContext context = new();

        //Act-Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.FillRecursive(user, context, _fillersBagMock.Object));
    }

    private User AnyUser(Guid? driverId = null) => new User()
    {
        Id = Guid.NewGuid(),
        DriverId = driverId.HasValue ? driverId.Value : Guid.NewGuid(),
        Name = "",
        PasswordHash = "",
        Roles = [],
        Email = ""
    };

    private Driver AnyDriver() => new Driver()
    {
        Id = Guid.NewGuid(),
        Known = false,
    };

}