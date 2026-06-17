using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;

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
    public async Task GetByName_DriverExists_ReturnsDriver()
    {
        var result = await _sut.GetByName(Constants.TestUserId);
        Assert.That(result, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id.ToString(), Is.EqualTo(Constants.TestDriverId));
            Assert.That(result.Known, Is.True);
            Assert.That(result.UserId.ToString(), Is.EqualTo(Constants.TestUserId));
        }
    }

    [Test]
    public async Task GetByName_DriverDoesNotExists_ReturnsNull()
    {
        var result = await _sut.GetByName("NOT_EXISTING_#&%@");
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Create_DriverWithNameAlreadyExists_ThrowsDriverExistsException()
    {
        Driver driver = new()
        {
            Id = Guid.NewGuid(),
            Name = Constants.TestUserId, // for known drivers driver.name = user.id
            Known = false
        };

        Assert.ThrowsAsync<DriverExistsException>(
            () => _sut.Create(driver));
    }
}
