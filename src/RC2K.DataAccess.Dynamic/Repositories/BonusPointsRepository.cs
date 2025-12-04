using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class BonusPointsRepository(Database database, BonusPointsMapper mapper)
    : CosmosRepository<BonusPoints, BonusPointsModel, BonusPointsMapper>(database, mapper), IBonusPointsRepository
{
    public override string ContainerName => "BonusPoints";
}
