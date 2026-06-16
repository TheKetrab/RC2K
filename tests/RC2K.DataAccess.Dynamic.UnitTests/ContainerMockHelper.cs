using Microsoft.Azure.Cosmos;
using Moq;
using RC2K.DataAccess.Interfaces;

namespace RC2K.DataAccess.Dynamic.UnitTests;

internal static class ContainerMockHelper
{
    public static (Mock<Database>, Mock<Container>, Mock<TMapper>, Mock<IEnvironmentProvider>) SetUpMocks<TMapper>(string entityName) where TMapper : class
    {
        var db = new Mock<Database>();
        var container = new Mock<Container>();
        var mapper = new Mock<TMapper>();

        var envProvider = new Mock<IEnvironmentProvider>();
        envProvider
            .Setup(x => x.ResolveContainerName(It.Is<string>(y => y == entityName)))
            .Returns(entityName);

        db.Setup(x => x.GetContainer(entityName)).Returns(container.Object);

        return (db, container, mapper, envProvider);
    }
}
