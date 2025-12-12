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

    public Task<List<Car>> GetBonusCars() =>
        _rallyUoW.Cars.GetBonusCars();

}