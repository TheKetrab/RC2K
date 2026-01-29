using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class CarService : ICarService
{
    public readonly IRallyUoW _rallyUoW;

    public CarService(IRallyUoW rallyUoW)
    {
        _rallyUoW = rallyUoW;
    }

    public Task<List<Car>> GetAll() => _rallyUoW.Cars.GetAll();

    public Task<List<Car>> GetAllByClass(int @class) =>
        _rallyUoW.Cars.GetAllByClass(@class);

    public bool IsA8(int carId)
    {
        return
            carId == 2 || // cordoba
            carId == 4 || // subaru
            carId == 7 || // evo5
            carId == 10 || // wira
            carId == 14 || // peugeot
            carId == 22; // evo4
    }

    public Task<List<Car>> GetBonusCars() =>
        _rallyUoW.Cars.GetBonusCars();

}