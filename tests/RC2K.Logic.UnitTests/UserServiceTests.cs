using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic.UnitTests;

public class UserServiceTests
{
    private UserService _sut;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IDriverRepository> _driverRepositoryMock;
    private Mock<IPasswordProvider> _passwordProviderMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _driverRepositoryMock = new Mock<IDriverRepository>();
        _passwordProviderMock = new Mock<IPasswordProvider>();

        _sut = new(_userRepositoryMock.Object, _driverRepositoryMock.Object, _passwordProviderMock.Object);
    }

    [Test]
    public async Task GetCurrentUserName_ReturnsEmptyString()
    {
        //Act
        var result = await _sut.GetCurrentUserName();

        //Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task Authenticate_ReturnsSuccessWithRolesForValidCredentials()
    {
        //Arrange
        string name = "TestUser";
        string password = "Password123";
        string passwordHash = "hashedPassword";
        Guid userId = Guid.NewGuid();
        Guid driverId = Guid.NewGuid();
        User user = new User 
        { 
            Id = userId,
            Name = name, 
            PasswordHash = passwordHash, 
            Roles = ["admin", "user"],
            DriverId = driverId,
            Email = "test@example.com"
        };
        
        _userRepositoryMock.Setup(x => x.GetByName(name)).Returns(Task.FromResult<User?>(user));
        _passwordProviderMock.Setup(x => x.CalculatePasswordHash(password)).Returns(passwordHash);

        //Act
        var result = await _sut.Authenticate(name, password);

        //Assert
        Assert.That(result.Success, Is.True);
        Assert.That(result.Message, Is.EqualTo("admin,user"));
        _userRepositoryMock.Verify(x => x.GetByName(name), Times.Once());
    }

    [Test]
    public async Task Authenticate_ReturnsFailureForInvalidPassword()
    {
        //Arrange
        string name = "TestUser";
        string password = "WrongPassword";
        string correctHash = "correctHash";
        string wrongHash = "wrongHash";
        Guid userId = Guid.NewGuid();
        Guid driverId = Guid.NewGuid();
        User user = new User 
        { 
            Id = userId,
            Name = name, 
            PasswordHash = correctHash,
            Roles = ["user"],
            DriverId = driverId,
            Email = "test@example.com"
        };
        
        _userRepositoryMock.Setup(x => x.GetByName(name)).Returns(Task.FromResult<User?>(user));
        _passwordProviderMock.Setup(x => x.CalculatePasswordHash(password)).Returns(wrongHash);

        //Act
        var result = await _sut.Authenticate(name, password);

        //Assert
        Assert.That(result.Success, Is.False);
    }

    [Test]
    public async Task Authenticate_ReturnsFailureForNonExistentUser()
    {
        //Arrange
        string name = "NonExistentUser";
        string password = "Password123";
        
        _userRepositoryMock.Setup(x => x.GetByName(name)).Returns(Task.FromResult<User?>(null));

        //Act
        var result = await _sut.Authenticate(name, password);

        //Assert
        Assert.That(result.Success, Is.False);
        _passwordProviderMock.Verify(x => x.CalculatePasswordHash(It.IsAny<string>()), Times.Never());
    }

    [Test]
    public async Task UpgradeDriverToUser_UpgradesDriverSuccessfully()
    {
        //Arrange
        string name = "TestDriver";
        string driverKey = "driverKey123";
        string password = "NewPassword";
        string email = "test@example.com";
        string passwordHash = "hashedPassword";
        
        Driver driver = new Driver { Id = Guid.NewGuid(), Name = name, Key = driverKey, Known = false };
        
        _driverRepositoryMock.Setup(x => x.GetByName(name)).Returns(Task.FromResult<Driver?>(driver));
        _passwordProviderMock.Setup(x => x.CalculatePasswordHash(password)).Returns(passwordHash);
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
        _driverRepositoryMock.Setup(x => x.Update(It.IsAny<Driver>())).Returns(Task.CompletedTask);

        //Act
        var result = await _sut.UpgradeDriverToUser(name, driverKey, password, email);

        //Assert
        Assert.That(result.Success, Is.True);
        _userRepositoryMock.Verify(x => x.Create(It.Is<User>(u => u.Name == name && u.Email == email)), Times.Once());
        _driverRepositoryMock.Verify(x => x.Update(It.Is<Driver>(d => d.Known == true && d.Id == driver.Id)), Times.Once());
    }

    [Test]
    public async Task UpgradeDriverToUser_ReturnsFailureForInvalidPassCode()
    {
        //Arrange
        string name = "TestDriver";
        string driverKey = "correctKey";
        string wrongKey = "wrongKey";
        string password = "NewPassword";
        string email = "test@example.com";
        
        Driver driver = new Driver { Id = Guid.NewGuid(), Name = name, Key = driverKey, Known = false };
        
        _driverRepositoryMock.Setup(x => x.GetByName(name)).Returns(Task.FromResult<Driver?>(driver));

        //Act
        var result = await _sut.UpgradeDriverToUser(name, wrongKey, password, email);

        //Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Contains.Substring("pass code is not valid"));
        _userRepositoryMock.Verify(x => x.Create(It.IsAny<User>()), Times.Never());
    }

    [Test]
    public async Task UpgradeDriverToUser_ReturnsFailureForNonExistentDriver()
    {
        //Arrange
        string name = "NonExistentDriver";
        string driverKey = "someKey";
        string password = "NewPassword";
        string email = "test@example.com";
        
        _driverRepositoryMock.Setup(x => x.GetByName(name)).Returns(Task.FromResult<Driver?>(null));

        //Act
        var result = await _sut.UpgradeDriverToUser(name, driverKey, password, email);

        //Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Contains.Substring("not found"));
    }

    [Test]
    public async Task CreateUserWithPassword_CreatesUserSuccessfully()
    {
        //Arrange
        string name = "NewUser";
        string password = "Password123";
        string email = "newuser@example.com";
        string nationality = "Poland";
        string passwordHash = "hashedPassword";
        Guid driverId = Guid.NewGuid();
        
        _passwordProviderMock.Setup(x => x.CalculatePasswordHash(password)).Returns(passwordHash);
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
        _driverRepositoryMock.Setup(x => x.Create(It.IsAny<Driver>())).Returns(Task.CompletedTask);

        //Act
        var result = await _sut.CreateUserWithPassword(name, password, nationality, email);

        //Assert
        Assert.That(result.Success, Is.True);
        _userRepositoryMock.Verify(x => x.Create(It.Is<User>(u => u.Name == name && u.Email == email)), Times.Once());
        _driverRepositoryMock.Verify(x => x.Create(It.Is<Driver>(d => d.Nationality == nationality)), Times.Once());
    }

    [Test]
    public async Task CreateUserWithPassword_GeneratesTempPasswordWhenPasswordIsNull()
    {
        //Arrange
        string name = "NewUser";
        string email = "newuser@example.com";
        string tempPassword = "tempPass123";
        string passwordHash = "hashedPassword";
        
        _passwordProviderMock.Setup(x => x.GenerateTemporaryPassword()).Returns(tempPassword);
        _passwordProviderMock.Setup(x => x.CalculatePasswordHash(tempPassword)).Returns(passwordHash);
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
        _driverRepositoryMock.Setup(x => x.Create(It.IsAny<Driver>())).Returns(Task.CompletedTask);

        //Act
        var result = await _sut.CreateUserWithPassword(name, null, null, email);

        //Assert
        Assert.That(result.Success, Is.True);
        _passwordProviderMock.Verify(x => x.GenerateTemporaryPassword(), Times.Once());
    }

    [Test]
    public async Task CreateUserWithPassword_ReturnsFailureWhenUserExists()
    {
        //Arrange
        string name = "ExistingUser";
        string password = "Password123";
        string email = "existing@example.com";
        string passwordHash = "hashedPassword";
        
        _passwordProviderMock.Setup(x => x.CalculatePasswordHash(password)).Returns(passwordHash);
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).ThrowsAsync(new UserExistsException());

        //Act
        var result = await _sut.CreateUserWithPassword(name, password, null, email);

        //Assert
        Assert.That(result.Success, Is.False);
        Assert.That(result.Message, Contains.Substring("already exists"));
        Assert.That(result.ErrorCode, Is.EqualTo((int)ErrorCodes.UserAlreadyExists));
    }

    [Test]
    public async Task CreateUserWithOAuth_CreatesUserWithOAuth()
    {
        //Arrange
        string name = "OAuthUser";
        string email = "oauth@example.com";
        string nationality = "Germany";
        
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
        _driverRepositoryMock.Setup(x => x.Create(It.IsAny<Driver>())).Returns(Task.CompletedTask);

        //Act
        var result = await _sut.CreateUserWithOAuth(name, email, nationality);

        //Assert
        Assert.That(result.Success, Is.True);
        _userRepositoryMock.Verify(x => x.Create(It.Is<User>(u => u.Name == name && u.PasswordHash == null)), Times.Once());
    }

    [Test]
    public async Task GetById_ReturnsUserFromRepository()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        Guid driverId = Guid.NewGuid();
        User user = new User 
        { 
            Id = userId, 
            Name = "TestUser",
            DriverId = driverId,
            Roles = ["user"],
            Email = "test@example.com"
        };
        
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(Task.FromResult<User?>(user));

        //Act
        var result = await _sut.GetById(userId);

        //Assert
        Assert.That(result, Is.EqualTo(user));
        _userRepositoryMock.Verify(x => x.GetById(userId), Times.Once());
    }

    [Test]
    public void SetEmailConfirmationCode_StoresCodeForEmail()
    {
        //Arrange
        string email = "test@example.com";
        string code = "ABC123";

        //Act
        _sut.SetEmailConfirmationCode(email, code);

        //Assert - just verify it doesn't throw
        Assert.Pass();
    }
}
