using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface ICarService
{
    Task<List<Car>> GetAll();
    Task<List<Car>> GetAllByClass(int @class);
    Task<List<Car>> GetBonusCars();
    bool IsA8(int carId);
}
