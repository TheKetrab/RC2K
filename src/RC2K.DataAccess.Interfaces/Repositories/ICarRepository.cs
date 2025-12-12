using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface ICarRepository : IGenericRepository<Car>
{
    Task<List<Car>> GetAll();
    Task<List<Car>> GetAllByClass(int @class);
    Task<Car?> GetById(int id);
    Task<List<Car>> GetBonusCars();
}
