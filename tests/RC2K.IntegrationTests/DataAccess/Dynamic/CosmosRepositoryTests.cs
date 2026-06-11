using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RC2K.DataAccess.Dynamic.EnvironmentProviders;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Repositories;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.IntegrationTests;

namespace RC2K.IntegrationTests.DataAccess.Dynamic;

public class CosmosRepositoryTests
{
    private ITimeEntryRepository _timeEntryRepository;

    [SetUp]
    public void Setup()
    {
        _timeEntryRepository = IntegrationFixture.ServiceProvider.GetRequiredService<ITimeEntryRepository>();
    }

    [Test]
    public async Task Scenario1_ContainerIsAccessible()
    {
        var res = await _timeEntryRepository.GetByStageId(1, CancellationToken.None);
        ;
    }
}
