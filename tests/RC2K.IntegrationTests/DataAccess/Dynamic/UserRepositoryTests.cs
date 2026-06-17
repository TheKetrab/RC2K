using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;

namespace RC2K.IntegrationTests.DataAccess.Dynamic;

public class UserRepositoryTests
{
    private UserRepository _sut;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _sut = (UserRepository)
            IntegrationFixture.ServiceProvider.GetRequiredService<IUserRepository>();
    }

    [Test]
    public async Task GetByName_UserExists_ReturnsUser()
    {
        var result = await _sut.GetByName(Constants.TestUserName);
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id.ToString(), Is.EqualTo(Constants.TestUserId));
            Assert.That(result.DriverId.ToString(), Is.EqualTo(Constants.TestDriverId));
        }
    }

    [Test]
    public async Task GetByName_UserDoesNotExists_ReturnsNull()
    {
        var result = await _sut.GetByName("NOT_EXISTING_#&%@");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Create_UserWithNameExists_ThrowsUsersExistsException()
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Name = Constants.TestUserName,
            DriverId = Guid.NewGuid(),
            Email = Guid.NewGuid().ToString(),
            Roles = []
        };

        Assert.ThrowsAsync<UserExistsException>(
            () => _sut.Create(user));
    }

    [Test]
    public void Create_UserWithMailExists_ThrowsUsersExistsException()
    {
        User user = new()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            DriverId = Guid.NewGuid(),
            Email = "email@email",
            Roles = []
        };

        Assert.ThrowsAsync<UserExistsException>(
            () => _sut.Create(user));
    }

}
