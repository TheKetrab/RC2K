
namespace RC2K.Resources.UnitTests;

public class CountriesTests
{
    [SetUp]
    public void Setup()
    {
        // invoke static ctor
        _ = Countries.GetCountries();
    }

    [Test]
    public void GetStages_ReturnsProperCount()
    {
        //Arrange-Act
        var countries = Countries.GetCountries().ToDictionary(x => x.code, x => x.name);

        //Assert
        Assert.That(countries["pl"], Is.EqualTo("Poland"));
    }
}