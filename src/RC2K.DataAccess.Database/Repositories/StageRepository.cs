using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public class StageRepository(IDataContext context)
    : AbstractRepository<Stage>(context), IStageRepository
{
    protected override IQueryable<Stage> DataSet => _context.Stages;

    public Task<Stage?> TryGetByCode(string code, bool arcade) =>
        DataSet.Include(x => x.StageData).ThenInclude(x => x.StageDetails).FirstOrDefaultAsync(x => x.Code == int.Parse(code) && x.IsArcade == arcade);

    public Task<List<Stage>> GetAllByRallyCodeBetween(int min, int max)
    {
        var query = DataSet.Where(x => x.Code >= min && x.Code <= max);

        query = query.Include(x => x.StageData).ThenInclude(x => x.StageDetails);

        return query.ToListAsync();
    }

    public async Task<string> GetWaypointsByStageCode(int stageCode) =>
        (await _context.StageWaypoints.FirstAsync(x => x.StageCode == stageCode)).Waypoints;

    public Task<List<Stage>> GetAll()
    {
        var query = DataSet;

        query = query.Include(x => x.StageData).ThenInclude(x => x.StageDetails);

        return query.ToListAsync();
    }
}
