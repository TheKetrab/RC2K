using Microsoft.Azure.Cosmos;
using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using System.Text.Json;
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

        return await FetchAll(query, CancellationToken.None);
    }

    public async Task<Dictionary<(int stageId, int carId), long>> GetBestTimesForDriver(Guid driverId)
    {
        var query = new QueryDefinition(@"
            SELECT c.stageId 
                  ,c.carId
                  ,MIN(c.time) AS best
            FROM c
            WHERE c.driverId = @driverId
            GROUP BY c.stageId, c.carId")
            .WithParameter("@driverId", driverId);

        using var it = Container.GetItemQueryIterator<JsonElement>(query);
        var (result, ru) = await _iterator.FetchAll(query, it, (element) => new
        {
            StageId = element.TryGetProperty("stageId", out var s) && s.TryGetInt32(out var sVal) ? (int?)sVal : null,
            CarId = element.TryGetProperty("carId", out var c) && c.TryGetInt32(out var cVal) ? (int?)cVal : null,
            Best = element.TryGetProperty("best", out var b) && b.TryGetInt64(out var bVal) ? (long?)bVal : null,
        });

        RequestUnitsHandlerInvoke(query, ru);

        return result
            .Where(x => x.CarId.HasValue && x.StageId.HasValue && x.Best.HasValue)
            .ToDictionary(x => (x.StageId!.Value, x.CarId!.Value), x => x.Best!.Value);
    }

    public async Task<List<TimeEntry>> GetByStageId(int stageId, CancellationToken ct)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.stageId = @stageId")
            .WithParameter("@stageId", stageId);

        return await FetchAll(query, ct);
    }

    public async Task<List<TimeEntry>> GetByStageIdAndCarId(int stageId, int carId, CancellationToken ct)
    {
        var query = new QueryDefinition(@"
            SELECT * FROM c WHERE c.stageId = @stageId AND c.carId = @carId")
            .WithParameter("@stageId", stageId)
            .WithParameter("@carId", carId);

        return await FetchAll(query, ct);
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

        return await FetchAll(query, CancellationToken.None);
    }
}
