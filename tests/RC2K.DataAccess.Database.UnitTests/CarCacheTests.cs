using Microsoft.Extensions.Caching.Memory;
using Moq;
using RC2K.DataAccess.Database.Cache;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.UnitTests;

public class CarCacheTests
{
    private CarCache? _sut;

    [Test]
    public void CarCache_DerivesFromGenericCache()
    {
        var memCache = new Mock<IMemoryCache>();
        _sut = new CarCache(memCache.Object);
        Assert.That(_sut is GenericCache<Car>, Is.True);
    }
}
