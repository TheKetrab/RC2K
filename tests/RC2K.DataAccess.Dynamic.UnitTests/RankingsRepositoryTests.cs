using Microsoft.Azure.Cosmos;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Dynamic.Repositories;

namespace RC2K.DataAccess.Dynamic.UnitTests;

public class RankingsRepositoryTests
{
    private Mock<Container> _container;
    private RankingsRepository _sut;

    [SetUp]
    public void SetUp()
    {
        (var db, _container, var mapper, var envProvider) = 
            ContainerMockHelper.SetUpMocks<RankingsMapper>("Statistics");

        var cache = new Mock<IMemoryCache>();

        _sut = new RankingsRepository(db.Object, mapper.Object, envProvider.Object, cache.Object);

        _container.
            Setup(x => x.ReadItemAsync<RankingSnapshotModel>(
                It.IsAny<string>(),
                It.IsAny<PartitionKey>(),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new CosmosException("", System.Net.HttpStatusCode.NotFound, 0, "", 0));
    }

    [Test]
    public async Task Container_WhenUsePartitionKey_ShouldEqualEntityName()
    {
        //Arrange
        var id = Guid.NewGuid();

        //Act
        _ = await _sut.GetById(id);

        //Assert
        _container
            .Verify(x => x.ReadItemAsync<RankingSnapshotModel>(
                It.Is<string>(y => y == id.ToString()),
                It.Is<PartitionKey>(y => y.ToString() == "[\"Statistics\"]"),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()));

    }
}
