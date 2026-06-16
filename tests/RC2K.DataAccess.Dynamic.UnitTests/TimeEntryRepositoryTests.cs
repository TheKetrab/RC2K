using Microsoft.Azure.Cosmos;
using Moq;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Dynamic.Repositories;

namespace RC2K.DataAccess.Dynamic.UnitTests;

public class TimeEntryRepositoryTests
{
    private Mock<Container> _container;
    private TimeEntryRepository _sut;

    [SetUp]
    public void SetUp()
    {
        (var db, _container, var mapper, var envProvider) = 
            ContainerMockHelper.SetUpMocks<TimeEntryMapper>("TimeEntries");

        _sut = new TimeEntryRepository(db.Object, mapper.Object, envProvider.Object);

        _container.
            Setup(x => x.ReadItemAsync<TimeEntryModel>(
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
            .Verify(x => x.ReadItemAsync<TimeEntryModel>(
                It.Is<string>(y => y == id.ToString()),
                It.Is<PartitionKey>(y => y.ToString() == "[\"TimeEntries\"]"),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()));

    }
}
