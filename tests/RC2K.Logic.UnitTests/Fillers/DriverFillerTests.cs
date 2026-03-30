using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers.UnitTests;

public class DriverFillerTests
{
    private DriverFiller _sut;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IFillersBag> _fillersBagMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _fillersBagMock = new Mock<IFillersBag>();
        _sut = new(_userRepositoryMock.Object);
    }

    [Test]
    public async Task FillRecursive_DriverAlreadyInContext_DriverNotFilled()
    {
        //Arrange
        CancellationToken ct = new();
        Driver driver = AnyDriver();
        FillingContext context = new();
        context.Drivers.Add(driver.Id, driver);

        //Act
        await _sut.FillRecursive(driver, context, _fillersBagMock.Object, ct);

        //Assert
        Assert.That(driver.User, Is.Null);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_DriverWithoutUserId_UserNotFilled()
    {
        //Arrange
        CancellationToken ct = new();
        Driver driver = AnyDriver(userId: null);
        FillingContext context = new();

        //Act
        await _sut.FillRecursive(driver, context, _fillersBagMock.Object, ct);

        //Assert
        Assert.That(driver.User, Is.Null);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_UserAlreadyInContext_UseUserFromContext()
    {
        //Arrange
        CancellationToken ct = new();
        User user = AnyUser();
        Driver driver = AnyDriver(user.Id);
        FillingContext context = new();
        context.Users.Add(user.Id, user);

        //Act
        await _sut.FillRecursive(driver, context, _fillersBagMock.Object, ct);

        //Assert
        Assert.That(driver.User, Is.EqualTo(user));
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_UserNotInContext_GetsUserAndFillsIt()
    {
        //Arrange
        CancellationToken ct = new();
        User user = AnyUser();
        Driver driver = AnyDriver(user.Id);
        FillingContext context = new();
        _userRepositoryMock.Setup(x => x.GetById(user.Id, It.IsAny<CancellationToken>())).ReturnsAsync(user);

        Mock<IUserFiller> userFillerMock = new Mock<IUserFiller>();
        _fillersBagMock.Setup(x => x.UserFiller).Returns(userFillerMock.Object);

        //Act
        await _sut.FillRecursive(driver, context, _fillersBagMock.Object, ct);

        //Assert
        Assert.That(driver.User, Is.EqualTo(user));
        _userRepositoryMock.Verify(x => x.GetById(user.Id, It.IsAny<CancellationToken>()), Times.Once());
        userFillerMock.Verify(x => x.FillRecursive(user, context, _fillersBagMock.Object, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void FillRecursive_UserNotInContextAndUnknownId_ThrowsException()
    {
        //Arrange
        CancellationToken ct = new();
        Driver driver = AnyDriver(Guid.NewGuid());
        FillingContext context = new();

        //Act-Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.FillRecursive(driver, context, _fillersBagMock.Object, ct));
    }

    private static Driver AnyDriver(Guid? userId = null) => new Driver()
    {
        Id = Guid.NewGuid(),
        Known = false,
        UserId = userId
    };

    private static User AnyUser() => new User()
    {
        Id = Guid.NewGuid(),
        Name = "",
        DriverId = Guid.NewGuid(),
        PasswordHash = "",
        Roles = [],
        Email = ""
    };
}
