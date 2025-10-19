using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public class CarRepository(IDataContext context) : AbstractRepository<Car>(context), ICarRepository
{
    protected override IQueryable<Car> DataSet => _context.Cars;

    public Task<List<Car>> GetBonusCars() =>
        Task.FromResult<List<Car>>([new Car() { Id = 123, Class = 0, Name = "Fake" }]);

    Task<List<Car>> ICarRepository.GetAllByClass(int @class) =>
        DataSet.Where(x => x.Class == @class).ToListAsync();
}
