using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Database.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.IntegrationTests.DataAccess.Database;

public class GenericRepositoryTests : BaseRepositoryRevertingObject
{
    private GenericRepository<Car, ICarCache> _genericRepository = default!;
    private IRallyUoW _rallyUoW = default!;

    protected override void OnOneTimeSetup()
    {
        _genericRepository = (GenericRepository<Car, ICarCache>)
            IntegrationFixture.ServiceProvider.GetRequiredService<ICarRepository>();

        _rallyUoW = IntegrationFixture.ServiceProvider.GetRequiredService<IRallyUoW>();
    }

    [Test]
    public async Task CrudOperationsTests()
    {
        // create
        var entity = new Car() { Id = 100, Class = 1, Name = "test" };
        await _genericRepository.Insert(entity);
        await _rallyUoW.Save();
        var result1 = await _genericRepository.GetById(100);

        // update
        SetCarClass(entity, 2);
        _genericRepository.Update(entity);
        await _rallyUoW.Save();
        var result2 = await _genericRepository.DbGet(100); // no cache
        Assert.That(result2?.Class, Is.EqualTo(2));

        // delete
        await _genericRepository.Delete(100);
        await _rallyUoW.Save();
        var result3 = await _genericRepository.DbGet(100);
        Assert.That(result3, Is.Null);
    }

    private void SetCarClass(Car car, int @class) =>
        typeof(Car).GetProperty(nameof(car.Class))!.GetSetMethod()!.Invoke(car, [@class]);
}
