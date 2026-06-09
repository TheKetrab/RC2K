using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.UnitTests;

public class DriverServiceTests
{
    private DriverService _sut;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IDriverRepository> _driverRepositoryMock;
    private Mock<IFillersBag> _fillersBagMock;
    private Mock<IPasswordProvider> _passwordProviderMock;
    private Mock<IDriverFiller> _driverFillerMock;
    private Mock<IUserFiller> _userFillerMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _driverRepositoryMock = new Mock<IDriverRepository>();
        _fillersBagMock = new Mock<IFillersBag>();
        _passwordProviderMock = new Mock<IPasswordProvider>();
        _driverFillerMock = new Mock<IDriverFiller>();
        _userFillerMock = new Mock<IUserFiller>();

        _fillersBagMock.Setup(x => x.DriverFiller).Returns(_driverFillerMock.Object);
        _fillersBagMock.Setup(x => x.UserFiller).Returns(_userFillerMock.Object);

        _sut = new(_userRepositoryMock.Object, _driverRepositoryMock.Object, _fillersBagMock.Object, _passwordProviderMock.Object);
    }

    [Test]
    public async Task CreateAnonymous_CreatesDriverWithGeneratedPassword()
    {
        //Arrange
        string name = "TestDriver";
        string nationality = "Poland";
        string tempPassword = "tempPass123";
        _passwordProviderMock.Setup(x => x.GenerateTemporaryPassword()).Returns(tempPassword);
        _driverRepositoryMock.Setup(x => x.Create(It.IsAny<Driver>())).Returns(Task.CompletedTask);

        //Act
        var result = await _sut.CreateAnonymous(name, nationality);

        //Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Payload.Name, Is.EqualTo(name));
        Assert.That(result.Payload.Nationality, Is.EqualTo(nationality));
        Assert.That(result.Payload.Known, Is.False);
        Assert.That(result.Payload.Key, Is.EqualTo(tempPassword));
        _driverRepositoryMock.Verify(x => x.Create(It.IsAny<Driver>()), Times.Once());
    }

    [Test]
    public async Task CreateAnonymous_ReturnsFailureWhenDriverExists()
    {
        //Arrange
        string name = "TestDriver";
        string nationality = "Poland";
        _passwordProviderMock.Setup(x => x.GenerateTemporaryPassword()).Returns("tempPass123");
        _driverRepositoryMock.Setup(x => x.Create(It.IsAny<Driver>())).ThrowsAsync(new DriverExistsException());

        //Act
        var result = await _sut.CreateAnonymous(name, nationality);

        //Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Is.EqualTo("Driver exists"));
    }

    [Test]
    public async Task GetAllNames_CallsRepositoryAndFillsDrivers()
    {
        //Arrange
        Driver driver = new() { Id = Guid.NewGuid(), Known = false, Name = "TestDriver" };
        List<Driver> drivers = [driver];
        _driverRepositoryMock.Setup(x => x.GetAll()).Returns(Task.FromResult(drivers));
        _driverFillerMock.Setup(x => x.FillRecursive(It.IsAny<Driver>(), It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.GetAllNames();

        //Assert
        _driverRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        _driverFillerMock.Verify(x => x.FillRecursive(driver, It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()), Times.Once());
        Assert.That(result[driver.Id], Is.EqualTo("TestDriver"));
    }

    [Test]
    public async Task GetAllNames_ReturnsKnownDriverNameFromUser()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        Guid driverId = Guid.NewGuid();
        Driver driver = new() { Id = driverId, Known = true, UserId = userId };
        User user = new()
        { 
            Id = userId,
            Name = "KnownUser",
            DriverId = driverId,
            Roles = ["user"],
            Email = "test@example.com",
            Driver = driver 
        };
        driver.User = user;
        
        List<Driver> drivers = [driver];
        _driverRepositoryMock.Setup(x => x.GetAll()).Returns(Task.FromResult(drivers));
        _driverFillerMock.Setup(x => x.FillRecursive(It.IsAny<Driver>(), It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.GetAllNames();

        //Assert
        Assert.That(result[driverId], Is.EqualTo("KnownUser"));
    }

    [Test]
    public async Task GetByName_ReturnsKnownDriverByUserName()
    {
        //Arrange
        string userName = "TestUser";
        Guid driverId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        User user = new()
        { 
            Id = userId,
            Name = userName,
            DriverId = driverId,
            Roles = ["user"],
            Email = "test@example.com"
        };
        Driver driver = new() { Id = driverId, Known = true, User = user, UserId = userId };
        user.Driver = driver;
        
        _userRepositoryMock.Setup(x => x.GetByName(userName)).Returns(Task.FromResult<User?>(user));
        _userFillerMock.Setup(x => x.FillRecursive(It.IsAny<User>(), It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.GetByName(userName);

        //Assert
        Assert.That(result, Is.EqualTo(driver));
        _userRepositoryMock.Verify(x => x.GetByName(userName), Times.Once());
    }

    [Test]
    public async Task GetByName_ReturnsAnonymousDriverByName()
    {
        //Arrange
        string driverName = "AnonymousDriver";
        Driver driver = new() { Id = Guid.NewGuid(), Known = false, Name = driverName };
        
        _userRepositoryMock.Setup(x => x.GetByName(driverName)).Returns(Task.FromResult<User?>(null));
        _driverRepositoryMock.Setup(x => x.GetByName(driverName)).Returns(Task.FromResult<Driver?>(driver));
        _driverFillerMock.Setup(x => x.FillRecursive(It.IsAny<Driver>(), It.IsAny<FillingContext>(), _fillersBagMock.Object, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.GetByName(driverName);

        //Assert
        Assert.That(result, Is.EqualTo(driver));
        _userRepositoryMock.Verify(x => x.GetByName(driverName), Times.Once());
        _driverRepositoryMock.Verify(x => x.GetByName(driverName), Times.Once());
    }

    [Test]
    public async Task GetByName_ReturnsNullWhenDriverNotFound()
    {
        //Arrange
        string driverName = "NonExistentDriver";
        _userRepositoryMock.Setup(x => x.GetByName(driverName)).Returns(Task.FromResult<User?>(null));
        _driverRepositoryMock.Setup(x => x.GetByName(driverName)).Returns(Task.FromResult<Driver?>(null));

        //Act
        var result = await _sut.GetByName(driverName);

        //Assert
        Assert.That(result, Is.Null);
    }
}
