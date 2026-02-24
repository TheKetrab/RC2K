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

    /// <summary>
    /// Checks if given internal binary car id (not RC2K domain id!) is for A8 car.
    /// </summary>
    /// <param name="internalCarId">Car id stored in game.</param>
    public bool IsA8InternalCarId(int internalCarId)
    {
        return
            internalCarId == 2 || // cordoba
            internalCarId == 4 || // subaru
            internalCarId == 7 || // evo5
            internalCarId == 10 || // wira
            internalCarId == 14 || // peugeot
            internalCarId == 22; // evo4
    }

    public Task<List<Car>> GetBonusCars() =>
        _rallyUoW.Cars.GetBonusCars();

}