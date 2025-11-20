using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAll();
    Task<User?> GetById(Guid id);
    Task<User?> GetByName(string name);
    Task Create(User user);
}
