using Microsoft.EntityFrameworkCore;
using Moq;
using RC2K.DataAccess.Database.Repositories;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.UnitTests;

public class StageRepositoryTests
{
    private StageRepository _sut;
    private Mock<IStageCache> _stageCache;

    [SetUp]
    public void SetUp()
    {
        var opt = new DbContextOptionsBuilder().Options;
        var dbContext = new Mock<RallyDbContext>(opt);
        _stageCache = new Mock<IStageCache>();

        _sut = new StageRepository(dbContext.Object, _stageCache.Object); 
    }

    [Test]
    public async Task GetByCode_CacheNotEmpty_ReturnsFromCache()
    {
        //Arrange
        var cachedValue = AnyStage();
        _stageCache.Setup(x => x.Get<Stage>(It.IsAny<string>())).Returns(cachedValue);

        //Act
        var result = await _sut.GetByCode(1, Direction.Arcade);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(cachedValue.Id));
            Assert.That(result.Direction, Is.EqualTo(Direction.Simulation));
            Assert.That(result.Code, Is.EqualTo(cachedValue.Code));
        });
    }

    [Test]
    public async Task GetAll_CacheNotEmpty_ReturnsFromCache()
    {
        //Arrange
        List<Stage> cachedValue = [AnyStage(), AnyStage()];
        _stageCache.Setup(x => x.Get<List<Stage>>(It.IsAny<string>())).Returns(cachedValue);

        //Act
        var result = await _sut.GetAll();

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(cachedValue));
    }

    [Test]
    public async Task GetAllByMinMax_CacheNotEmpty_ReturnsFromCache()
    {
        //Arrange
        List<Stage> cachedValue = [AnyStage(), AnyStage()];
        _stageCache.Setup(x => x.Get<List<Stage>>(It.IsAny<string>())).Returns(cachedValue);

        //Act
        var result = await _sut.GetAllByStageCodeBetween(21,26);

        //Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(cachedValue));
    }

    private Stage AnyStage() =>
        new() { Code = 1, Direction = Direction.Simulation, Id = 1 };
}
