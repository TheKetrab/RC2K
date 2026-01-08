using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IDriverRepository
{
    Task Update(Driver driver);
    Task<List<Driver>> GetAll();
    Task<Driver?> GetByName(string name);
    Task<bool> Exist(string name);
    Task<Driver?> GetById(Guid id);
    Task Create(Driver driver);
}
