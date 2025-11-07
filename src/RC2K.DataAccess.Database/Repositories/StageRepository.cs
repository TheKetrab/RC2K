using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public class StageRepository(RallyDbContext dbContext, IStageCache cache) 
    : GenericRepository<Stage, IStageCache>(dbContext, cache), IStageRepository
{
    public Task<Stage?> TryGetByCode(string code, Direction direction) =>
        _dbContext.Stages.FillFullData()
            .FirstOrDefaultAsync(x => x.Code == int.Parse(code) && x.Direction == direction);

    public Task<List<Stage>> GetAllByRallyCodeBetween(int min, int max) =>
        _dbContext.Stages.FillFullData()
            .Where(x => x.Code >= min && x.Code <= max)
            .ToListAsync();

    public async Task<string> GetWaypointsByStageCode(int stageCode) =>
        (await _dbContext.StageWaypoints.FirstAsync(x => x.StageCode == stageCode)).Waypoints;

    public async Task<string?> GetPathByStageCode(int stageCode) =>
        (await _dbContext.StageWaypoints.FirstAsync(x => x.StageCode == stageCode)).Path;

    public async Task UpdatePath(int stageCode, string path)
    {
        var stageWaypoints = await _dbContext.StageWaypoints.FirstAsync(x => x.StageCode == stageCode);
        stageWaypoints.Path = path; 
    }

    public Task<List<Stage>> GetAll() =>
        _dbContext.Stages.FillFullData().ToListAsync();

    protected override IQueryable<Stage> Full(IQueryable<Stage> query)
    {
        return query.FillFullData();
    }
}

public static class StageRepositoryExtensions
{
    public static IQueryable<Stage> FillFullData(this IQueryable<Stage> query) =>
        query
            .Include(x => x.StageData)
                .ThenInclude(x => x.StageDetails)
            .Include(x => x.StageWaypoints);
}