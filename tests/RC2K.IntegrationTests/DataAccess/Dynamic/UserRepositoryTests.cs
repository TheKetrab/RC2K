using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;

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
    }

    [Test]
    public async Task GetByName_UserDoesNotExists_ReturnsNull()
    {
        var result = await _sut.GetByName("NOT_EXISTING_#&%@");
        Assert.That(result, Is.Null);
    }

}
