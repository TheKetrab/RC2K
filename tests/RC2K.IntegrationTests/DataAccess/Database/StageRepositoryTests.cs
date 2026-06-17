using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Database.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.IntegrationTests.DataAccess.Database;

public class StageRepositoryTests : BaseRepositoryRevertingObject
{
    private StageRepository _sut = default!;
    private IStageCache _cache = default!;

    protected override void OnOneTimeSetup()
    {
        _sut = (StageRepository)IntegrationFixture.ServiceProvider.GetRequiredService<IStageRepository>();
        _cache = IntegrationFixture.ServiceProvider.GetRequiredService<IStageCache>();
    }

    [Test]
    public async Task GetByCode_CacheEmpty_ReturnsDbValueAndSetCacheValue()
    {
        var cacheKey = "StageByCodeAndDirectionKey_21_Simulation";
        _cache.Set<Stage>(cacheKey, null!);

        var result = await _sut.GetByCode(21, Direction.Simulation);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StageData, Is.Not.Null);
        Assert.That(result.StageData.Name, Is.EqualTo("Port Soderick"));
        Assert.That(_cache.Get<Stage>(cacheKey), Is.EqualTo(result));
    }

    [Test]
    public async Task GetAll_CacheEmpty_ReturnsDbValueAndSetCacheValue()
    {
        var cacheKey = "AllStagesKey";
        _cache.Set<Stage>(cacheKey, null!);

        var result = await _sut.GetAll();

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(72));
        Assert.That(_cache.Get<List<Stage>>(cacheKey), Is.EqualTo(result));
    }

    [Test]
    public async Task GetAllByMinMax_CacheEmpty_ReturnsDbValueAndSetCacheValue()
    {
        var cacheKey = "AllStagesByMinMaxKey_21_26";
        _cache.Set<List<Stage>>(cacheKey, null!);

        var result = await _sut.GetAllByStageCodeBetween(21, 26);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(12));
        Assert.That(_cache.Get<List<Stage>>(cacheKey), Is.EqualTo(result));
    }
}
