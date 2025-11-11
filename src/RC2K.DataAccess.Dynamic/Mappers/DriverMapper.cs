using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class DriverMapper : IModelMapper<Driver, DriverModel>
{
    public DriverModel ToCosmosModel(Driver driver)
    {
        DriverModel model = new()
        {
            Id = driver.Id,
            Known = driver.Known,
            Key = driver.Key,
            Name = driver.Name,
            Nationality = driver.Nationality,
            UserId = driver.UserId
        };

        return model;
    }

    public Driver ToDomainModel(DriverModel model)
    {
        Driver driver = new()
        {
            Id = model.Id,
            Known = model.Known,
            Key = model.Key,
            Name = model.Name,
            Nationality = model.Nationality,
            UserId = model.UserId
        };

        return driver;
    }
}
