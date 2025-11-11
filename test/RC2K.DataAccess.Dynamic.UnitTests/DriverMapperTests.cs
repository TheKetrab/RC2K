using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests;

public class DriverMapperTests
{
    private DriverMapper _driverMapper;

    [SetUp]
    public void Setup()
    {
        _driverMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        DriverModel model = AnyDriverModel();

        //Act
        var result = _driverMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.Known, Is.EqualTo(model.Known));
            Assert.That(result.UserId, Is.EqualTo(model.UserId));
            Assert.That(result.Name, Is.EqualTo(model.Name));
            Assert.That(result.Key, Is.EqualTo(model.Key));
            Assert.That(result.Nationality, Is.EqualTo(model.Nationality));
        }
        
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        Driver driver = AnyDriver();

        //Act
        var result = _driverMapper.ToCosmosModel(driver);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(driver.Id));
            Assert.That(result.Known, Is.EqualTo(driver.Known));
            Assert.That(result.UserId, Is.EqualTo(driver.UserId));
            Assert.That(result.Name, Is.EqualTo(driver.Name));
            Assert.That(result.Key, Is.EqualTo(driver.Key));
            Assert.That(result.Nationality, Is.EqualTo(driver.Nationality));
        }

    }

    private DriverModel AnyDriverModel() => new DriverModel()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        Key = "asd",
        Known = true,
        Name = "name",
        Nationality = "pl",
        UserId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3")
    };

    private Driver AnyDriver() => new Driver()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        Key = "asd",
        Known = true,
        Name = "name",
        Nationality = "pl",
        UserId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3")
    };
}