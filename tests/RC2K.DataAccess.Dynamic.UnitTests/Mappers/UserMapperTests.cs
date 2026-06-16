using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests.Mappers;

public class UserMapperTests
{
    private UserMapper _userMapper;

    [SetUp]
    public void Setup()
    {
        _userMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        UserModel model = AnyUserModel();

        //Act
        var result = _userMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.Name, Is.EqualTo(model.Name));
            Assert.That(result.DriverId, Is.EqualTo(model.DriverId));
            Assert.That(result.PasswordHash, Is.EqualTo(model.PasswordHash));
            Assert.That(result.Email, Is.EqualTo(model.Email));
            Assert.That(result.Roles, Has.Length.EqualTo(2));
            Assert.That(result.Roles[0], Is.EqualTo("admin"));
            Assert.That(result.Roles[1], Is.EqualTo("user"));
        }
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        User user = AnyUser();

        //Act
        var result = _userMapper.ToCosmosModel(user);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Name, Is.EqualTo(user.Name));
            Assert.That(result.DriverId, Is.EqualTo(user.DriverId));
            Assert.That(result.PasswordHash, Is.EqualTo(user.PasswordHash));
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Roles, Has.Count.EqualTo(2));
            Assert.That(result.Roles[0], Is.EqualTo("admin"));
            Assert.That(result.Roles[1], Is.EqualTo("user"));
        }
    }

    [Test]
    public void ToDomainModel_WithNullPasswordHash_MapsProperly()
    {
        //Arrange
        UserModel model = new UserModel()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            Name = "John Doe",
            DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            PasswordHash = null,
            Email = "john@example.com",
            Roles = new List<string> { "user" }
        };

        //Act
        var result = _userMapper.ToDomainModel(model);

        //Assert
        Assert.That(result.PasswordHash, Is.Null);
    }

    [Test]
    public void ToCosmosModel_WithMultipleRoles_MapsProperly()
    {
        //Arrange
        User user = new User()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            Name = "John Doe",
            DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            PasswordHash = "hashed_password_123",
            Email = "john@example.com",
            Roles = new[] { "admin", "moderator", "user" }
        };

        //Act
        var result = _userMapper.ToCosmosModel(user);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Roles, Has.Count.EqualTo(3));
            Assert.That(result.Roles[0], Is.EqualTo("admin"));
            Assert.That(result.Roles[1], Is.EqualTo("moderator"));
            Assert.That(result.Roles[2], Is.EqualTo("user"));
        }
    }

    [Test]
    public void RoundTrip_FromDomainToCosmosAndBack_PreservesData()
    {
        //Arrange
        User original = AnyUser();

        //Act
        UserModel model = _userMapper.ToCosmosModel(original);
        User restored = _userMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(restored.Id, Is.EqualTo(original.Id));
            Assert.That(restored.Name, Is.EqualTo(original.Name));
            Assert.That(restored.DriverId, Is.EqualTo(original.DriverId));
            Assert.That(restored.PasswordHash, Is.EqualTo(original.PasswordHash));
            Assert.That(restored.Email, Is.EqualTo(original.Email));
            Assert.That(restored.Roles, Is.EqualTo(original.Roles));
        }
    }

    private static UserModel AnyUserModel() => new UserModel()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        Name = "John Doe",
        DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        PasswordHash = "hashed_password_123",
        Email = "john@example.com",
        Roles = new List<string> { "admin", "user" }
    };

    private static User AnyUser() => new User()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        Name = "John Doe",
        DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        PasswordHash = "hashed_password_123",
        Email = "john@example.com",
        Roles = new[] { "admin", "user" }
    };
}
