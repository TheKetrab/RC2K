using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework.Internal;
using RC2K.DataAccess.Database.Cache;

namespace RC2K.DataAccess.Database.UnitTests;

public class A;
public class B;
public class ACache(IMemoryCache memoryCache) : GenericCache<A>(memoryCache);

public class GenericCacheTests
{
    private ACache _sut;
    private Mock<IMemoryCache> _memoryCacheMock;
    delegate bool DoSomethingCallback(object? key, out A? value);

    [SetUp]
    public void Setup()
    {
        _memoryCacheMock = new Mock<IMemoryCache>();

        _memoryCacheMock
            .Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
            .Returns(false);

        _sut = new ACache(_memoryCacheMock.Object);
    }

    [Test]
    public void Get_ValueExists_ReturnsValue()
    {
        //Arrange
        const int cachedId = 42;
        A cachedObject = new();
        SetupCachedObject(cachedId, cachedObject);

        //Act
        A? result = _sut.Get(cachedId);
        
        //Assert
        Assert.That(result, Is.EqualTo(cachedObject));
    }

    [Test]
    public void Get_ValueDoesNotExist_ReturnsNull()
    {
        //Arrange
        const int cachedId = 42;
        A cachedObject = new();
        SetupCachedObject(cachedId, cachedObject);

        //Act
        A? result = _sut.Get(17);

        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Get_ValueExistsButIsOfOtherType_ReturnsNull()
    {
        //Arrange
        const int cachedId = 42;
        B cachedObject = new();
        SetupCachedObject(cachedId, cachedObject);

        //Act
        A? result = _sut.Get(cachedId);

        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetTyped_ValueExists_ReturnsValue()
    {
        //Arrange
        const string cachedKey = "42";
        B cachedObject = new();
        SetupCachedObject(cachedKey, cachedObject);

        //Act
        B? result = _sut.Get<B>(cachedKey);

        //Assert
        Assert.That(result, Is.EqualTo(cachedObject));
    }

    [Test]
    public void GetTyped_ValueDoesNotExist_ReturnsNull()
    {
        //Arrange
        const string cachedKey = "42";
        B cachedObject = new();
        SetupCachedObject(cachedKey, cachedObject);

        //Act
        B? result = _sut.Get<B>("17");

        //Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public void GetTyped_ValueExistsButIsOfOtherType_ReturnsNull()
    {
        //Arrange
        const string cachedKey = "42";
        A cachedObject = new();
        SetupCachedObject(cachedKey, cachedObject);

        //Act
        B? result = _sut.Get<B>("42");

        //Assert
        Assert.That(result, Is.Null);
    }

    private void SetupCachedObject(int id, object value)
    {
        _memoryCacheMock
            .Setup(x => x.TryGetValue(It.Is<object>(y => (int)y == id), out It.Ref<object?>.IsAny))
            .Callback((object key, out object? cacheResult) =>
            {
                cacheResult = value;
            })
            .Returns(true);
    }

    private void SetupCachedObject(string key, object value)
    {
        _memoryCacheMock
            .Setup(x => x.TryGetValue(It.Is<object>(y => (string)y == key), out It.Ref<object?>.IsAny))
            .Callback((object key, out object? cacheResult) =>
            {
                cacheResult = value;
            })
            .Returns(true);
    }
}
