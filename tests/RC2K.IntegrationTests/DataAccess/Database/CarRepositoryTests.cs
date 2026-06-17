using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Database.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.IntegrationTests.DataAccess.Database;

public class CarRepositoryTests : BaseRepositoryRevertingObject
{
    private CarRepository _sut = default!;

    protected override void OnOneTimeSetup()
    {
        _sut = (CarRepository)IntegrationFixture.ServiceProvider.GetRequiredService<ICarRepository>();
    }

    [Test]
    public async Task GetAll()
    {
        var result = await _sut.GetAll();
        Assert.That(result.Count, Is.EqualTo(34));
    }

    [Test]
    public async Task GetAllByClass_GivenClass_ReturnsAllCarsFromClass()
    {
        var result = await _sut.GetAllByClass(6);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.All(x => x.Class == 6), Is.True);
            Assert.That(result, Has.Count.EqualTo(5));
        }
    }

    [Test]
    public async Task GetAllBonusCars()
    {
        var result = await _sut.GetBonusCars();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.All(x => x.Class == Car.BonusClass), Is.True);
            Assert.That(result, Has.Count.EqualTo(12));
        }
    }

}
