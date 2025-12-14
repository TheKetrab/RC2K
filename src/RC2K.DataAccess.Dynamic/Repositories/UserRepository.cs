using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel.Exceptions;
using User = RC2K.DomainModel.User;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class UserRepository(Database database, UserMapper mapper, IEnvironmentProvider envProvider)
    : CosmosRepository<User, UserModel, UserMapper>(database, mapper, envProvider)
    , IUserRepository
{
    public override string EntityName => "Users";

    public override async Task Create(User entity)
    {
        try
        {
            await base.Create(entity);
        }
        catch (CosmosException e)
        {
            if (e.StatusCode == System.Net.HttpStatusCode.Conflict && 
                e.Message.Contains("Unique index constraint violation."))
            {
                throw new UserExistsException();
            }
            throw;
        }
    }

    public async Task<User?> GetByName(string name)
    {
        // TODO query by name

        var allUsers = await GetAll();
        return allUsers.FirstOrDefault(x => x.Name == name);
    }
}
