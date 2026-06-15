using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;

namespace RC2K.IntegrationTests.DataAccess.Dynamic;

public class DriverRepositoryTests
{
    private DriverRepository _sut;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _sut = (DriverRepository)
            IntegrationFixture.ServiceProvider.GetRequiredService<IDriverRepository>();
    }

    [Test]
    public async Task GetByName_UserDoesNotExists_ReturnsNull()
    {
        var result = await _sut.GetByName("NOT_EXISTING_#&%@");
        Assert.That(result, Is.Null);
    }

}
