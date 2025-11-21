using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using System.Collections.Generic;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class TimeEntryRepository(Database database, TimeEntryMapper mapper)
    : CosmosRepository<TimeEntry, TimeEntryModel, TimeEntryMapper>(database, mapper), ITimeEntryRepository
{
    public override string ContainerName => "TimeEntries";

    public override Task<List<TimeEntry>> GetAll()
    {
        // to reduce RU in cloud
        return Task.FromResult<List<TimeEntry>>([]);
    }

    public async Task<List<TimeEntry>> GetByStageId(int stageId)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.stageId = @stageId")
            .WithParameter("@stageId", stageId);

        using var it = Container.GetItemQueryIterator<TimeEntryModel>(query);

        List<TimeEntry> result = new();
        while (it.HasMoreResults)
        {
            var val = (await it.ReadNextAsync()).Resource;
            result.AddRange(val.Select(Mapper.ToDomainModel));
        }

        return result;
    }

    public async Task<List<TimeEntry>> GetByStageIdAndCarId(int stageId, int carId)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.stageId = @stageId AND c.carId = @carId")
            .WithParameter("@stageId", stageId)
            .WithParameter("@carId", carId);

        using var it = Container.GetItemQueryIterator<TimeEntryModel>(query);

        List<TimeEntry> result = new();
        while (it.HasMoreResults)
        {
            var val = (await it.ReadNextAsync()).Resource;
            result.AddRange(val.Select(Mapper.ToDomainModel));
        }

        return result;
    }

    public async Task<List<TimeEntry>> GetByStageIdAndCarIdAndDriverIdAndTime(int stageId, int carId, Guid driverId, TimeOnly time)
    {
        int centiseconds = Utils.TimeOnlyToCentiseconds(time);

        var query = new QueryDefinition(@"
            SELECT * FROM c 
              WHERE c.stageId = @stageId
                AND c.carId = @carId
                AND c.driverId = @driverId
                AND c.time = @centiseconds")
            .WithParameter("@stageId", stageId)
            .WithParameter("@carId", carId)
            .WithParameter("@driverId", driverId)
            .WithParameter("@centiseconds", centiseconds);

        using var it = Container.GetItemQueryIterator<TimeEntryModel>(query);

        List<TimeEntry> result = new();
        while (it.HasMoreResults)
        {
            var val = (await it.ReadNextAsync()).Resource;
            result.AddRange(val.Select(Mapper.ToDomainModel));
        }

        return result;
    }

}
