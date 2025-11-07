using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class UserRepository(Database database, UserMapper mapper)
    : CosmosRepository<RC2K.DomainModel.User, UserModel, UserMapper>(database, mapper), IUserRepository
{
    public override string ContainerName => "Users";
}
