using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class DriverRepository(
    Database database, DriverMapper mapper, IEnvironmentProvider environmentProvider)
    : CosmosRepository<Driver, DriverModel, DriverMapper>(database, mapper, environmentProvider)
    , IDriverRepository
{
    public override string EntityName => "Drivers";
    public Task<bool> Exist(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<Driver?> GetByName(string name)
    {
        // TODO query by name

        var allDrivers = await GetAll();
        return allDrivers.FirstOrDefault(x => x.Name == name);
    }
}
