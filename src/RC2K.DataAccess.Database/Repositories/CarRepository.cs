using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public class CarRepository(RallyDbContext dbContext, ICarCache cache) 
    : GenericRepository<Car, ICarCache>(dbContext, cache), ICarRepository
{
    private const string AllCarsKey = "AllCarsKey";
    private const string AllCarsByClassKey = "AllCarsByClassKey";

    public async Task<List<Car>> GetAll()
    {
        List<Car>? cacheValue = _cache.Get<List<Car>>(AllCarsKey);
        if (cacheValue != null)
        {
            return cacheValue;
        }

        var dbValue = await dbContext.Cars.ToListAsync();
        _cache.Set(AllCarsKey, dbValue);
        return dbValue;
    }

    public Task<List<Car>> GetBonusCars() =>
        Task.FromResult<List<Car>>([new Car() { Id = 123, Class = 0, Name = "Fake" }]);

    public async Task<List<Car>> GetAllByClass(int @class)
    {
        string cacheKey = $"{AllCarsByClassKey}{@class}";

        List<Car>? cacheValue = _cache.Get<List<Car>>(cacheKey);
        if (cacheValue != null)
        {
            return cacheValue;
        }

        var dbValue = await base.Get(x => x.Class == @class, full: true);
        _cache.Set(cacheKey, dbValue);
        return dbValue;
    }

}
