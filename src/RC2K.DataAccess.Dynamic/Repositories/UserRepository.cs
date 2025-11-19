using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel.Exceptions;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class UserRepository(Database database, UserMapper mapper)
    : CosmosRepository<RC2K.DomainModel.User, UserModel, UserMapper>(database, mapper), IUserRepository
{
    public override string ContainerName => "Users";

    public override async Task Create(DomainModel.User entity)
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

    public async Task<DomainModel.User?> GetByName(string name)
    {
        // TODO query by name

        var allUsers = await GetAll();
        return allUsers.FirstOrDefault(x => x.Name == name);
    }
}
