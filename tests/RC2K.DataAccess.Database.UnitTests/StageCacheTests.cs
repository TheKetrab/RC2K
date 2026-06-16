using Microsoft.Extensions.Caching.Memory;
using Moq;
using RC2K.DataAccess.Database.Cache;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.UnitTests;

public class StageCacheTests
{
    private StageCache? _sut;

    [Test]
    public void StageCache_DerivesFromGenericCache()
    {
        var memCache = new Mock<IMemoryCache>();
        _sut = new StageCache(memCache.Object);
        Assert.That(_sut is GenericCache<Stage>, Is.True);
    }
}
