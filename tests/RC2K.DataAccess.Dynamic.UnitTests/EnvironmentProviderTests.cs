using RC2K.DataAccess.Dynamic.EnvironmentProviders;

namespace RC2K.DataAccess.Dynamic.UnitTests;

public class EnvironmentProviderTests
{
    [Test]
    public void DevEnvironmentProvider_ReturnsPlainContainerName()
    {
        //Arrange
        var sut = new DevEnvironmentProvider();
        
        //Act
        var result = sut.ResolveContainerName("container");

        //Assert
        Assert.That(result, Is.EqualTo("container"));
    }

    [Test]
    public void ProdEnvironmentProvider_AddsProdSuffixToContainerName()
    {
        //Arrange
        var sut = new ProdEnvironmentProvider();

        //Act
        var result = sut.ResolveContainerName("container");

        //Assert
        Assert.That(result, Is.EqualTo("container-prod"));
    }

}
