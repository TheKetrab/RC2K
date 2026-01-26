using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using static RC2K.Utils.Utils;

namespace RC2K.DataAccess.Dynamic.Repositories;

public class TimeEntryRepository(Database database, TimeEntryMapper mapper, IEnvironmentProvider envProvider)
    : CosmosRepository<TimeEntry, TimeEntryModel, TimeEntryMapper>(database, mapper, envProvider)
    , ITimeEntryRepository
{
    public override string EntityName => "TimeEntries";


    public override Task<List<TimeEntry>> GetAll()
    {
        // to reduce RU in cloud
        return Task.FromResult<List<TimeEntry>>([]);
    }

    public async Task<List<TimeEntry>> GetAllNotVerified()
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE NOT IS_DEFINED(c.verifyInfoId) OR IS_NULL(c.verifyInfoId)");

        return await FetchAll(query);
    }

    public async Task<List<TimeEntry>> GetByStageId(int stageId)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.stageId = @stageId")
            .WithParameter("@stageId", stageId);

        return await FetchAll(query);
    }

    public async Task<List<TimeEntry>> GetByStageIdAndCarId(int stageId, int carId)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.stageId = @stageId AND c.carId = @carId")
            .WithParameter("@stageId", stageId)
            .WithParameter("@carId", carId);

        return await FetchAll(query);
    }

    public async Task<List<TimeEntry>> GetByStageIdAndCarIdAndDriverIdAndTime(int stageId, int carId, Guid driverId, TimeOnly time)
    {
        int centiseconds = TimeOnlyToCentiseconds(time);

        var query = new QueryDefinition(@"
            SELECT * FROM c 
              WHERE c.stageId = @stageId
                AND c.carId = @carId
                AND c.driverId = @driverId
                AND c.time <= @centiseconds")
            .WithParameter("@stageId", stageId)
            .WithParameter("@carId", carId)
            .WithParameter("@driverId", driverId)
            .WithParameter("@centiseconds", centiseconds);

        return await FetchAll(query);
    }
}
