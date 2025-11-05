using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IDriverRepository
{
    Task<List<Driver>> GetAll();
    Task<bool> Exist(string name);
    Task<Driver?> GetById(Guid id);
}
