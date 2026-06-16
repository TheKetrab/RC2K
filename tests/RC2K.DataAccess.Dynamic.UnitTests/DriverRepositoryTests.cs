using Microsoft.Azure.Cosmos;
using Moq;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Dynamic.Repositories;

namespace RC2K.DataAccess.Dynamic.UnitTests;

public class DriverRepositoryTests
{
    private Mock<Container> _container;
    private DriverRepository _sut;

    [SetUp]
    public void SetUp()
    {
        (var db, _container, var mapper, var envProvider) = 
            ContainerMockHelper.SetUpMocks<DriverMapper>("Drivers");

        _sut = new DriverRepository(db.Object, mapper.Object, envProvider.Object);

        _container.
            Setup(x => x.ReadItemAsync<DriverModel>(
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
            .Verify(x => x.ReadItemAsync<DriverModel>(
                It.Is<string>(y => y == id.ToString()),
                It.Is<PartitionKey>(y => y.ToString() == "[\"Drivers\"]"),
                It.IsAny<ItemRequestOptions>(),
                It.IsAny<CancellationToken>()));

    }
}
