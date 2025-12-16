using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class VerifyInfoRepository(Database database, VerifyInfoMapper mapper, IEnvironmentProvider envProvider)
    : CosmosRepository<VerifyInfo, VerifyInfoModel, VerifyInfoMapper>(database, mapper, envProvider)
    , IVerifyInfoRepository
{
    public override string EntityName => "VerifyInfos";
}
