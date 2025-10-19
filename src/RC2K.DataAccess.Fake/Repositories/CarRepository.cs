using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class CarRepository(IDataContext context) : AbstractRepository<Car>(context), ICarRepository
{
    protected override IQueryable<Car> DataSet => _context.Cars;

    public Task<List<Car>> GetBonusCars() =>
    Task.FromResult<List<Car>>([new Car() { Id = 123, Class = 0, Name = "Fake" }]);

    public Task<List<Car>> GetAllByClass(int @class) =>
        Task.FromResult(_context.Cars.Where(x => x.Class == @class).ToList());

}
