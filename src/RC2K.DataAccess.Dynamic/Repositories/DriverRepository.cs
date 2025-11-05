using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class DriverRepository(Database database, DriverMapper mapper)
    : CosmosRepository<Driver, DriverModel, DriverMapper>(database, mapper), IDriverRepository
{
    public override string ContainerName => "Drivers";
    public Task<bool> Exist(string name)
    {
        throw new NotImplementedException();
    }
}
