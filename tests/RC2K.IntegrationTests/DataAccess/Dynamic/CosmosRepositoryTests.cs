using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.IntegrationTests.Helpers;

namespace RC2K.IntegrationTests.DataAccess.Dynamic;

public class CosmosRepositoryTests
{
    private CosmosRepository<Driver, DriverModel, DriverMapper> _cosmosRepository;
    private const string _cleanupKey = "[INTEGRATION_TESTS_CLEANUP]";

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _cosmosRepository = (CosmosRepository<Driver, DriverModel, DriverMapper>)
            IntegrationFixture.ServiceProvider.GetRequiredService<IDriverRepository>();
    }

    [TearDown]
    public async Task TearDown()
    {
        // TODO: getall fetches all -> make it better cosmos query
        var all = await _cosmosRepository.GetAll();
        foreach (var x in all.Where(i => i.Name?.Contains(_cleanupKey) ?? false))
        {
            await _cosmosRepository.Delete(x.Id.ToString());
        }
    }

    [Test]
    public async Task CrudOperationsTests()
    {
        Guid id = Guid.NewGuid();
        var entity = AnyEntity(id, "name");
        DriverValueComparer comparer = new();

        //Create
        await _cosmosRepository.Create(entity);

        //Retrieve
        var res1 = await _cosmosRepository.GetById(id);
        Assert.That(res1, Is.EqualTo(entity).Using(comparer));

        //Update
        SetName(entity, "new_name");
        await _cosmosRepository.Update(entity);
        var res2 = await _cosmosRepository.GetById(id);
        Assert.That(res2, Is.EqualTo(entity).Using(comparer));

        //Delete
        await _cosmosRepository.Delete(id.ToString());
        var res3 = await _cosmosRepository.GetById(id);
        Assert.That(res3, Is.Null);
    }

    [Test]
    public async Task GetAll_FetchesAllEntitiesAndFillsThem()
    {
        var result = await _cosmosRepository.GetAll();
        Assert.That(result, Is.Not.Null);
    }

    private static Driver AnyEntity(Guid id, string name) =>
        new()
        {
            Id = id,
            Known = false,
            Name = $"{_cleanupKey}{name}"
        };

    private static void SetName(Driver entity, string name) =>
        typeof(Driver)
            .GetProperty(nameof(entity.Name))?
            .SetMethod?
            .Invoke(entity, [$"{_cleanupKey}{name}"]);
}
