using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface ICarRepository : IGenericRepository<Car>
{
    Task<List<Car>> GetAll();
    Task<Car?> GetById(int id);
    Task<List<Car>> GetBonusCars();
}
