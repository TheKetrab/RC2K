using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Cache;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public class StageRepository(RallyDbContext dbContext, IStageCache cache) 
    : GenericRepository<Stage, IStageCache>(dbContext, cache), IStageRepository
{
    private const string AllStagesKey = "AllStagesKey";
    private const string AllStagesByMinMaxKey = "AllStagesByMinMaxKey";
    private const string StageByCodeAndDirection = "StageByCodeAndDirectionKey";

    public async Task<string> GetWaypointsByStageCode(int stageCode) =>
        (await this.GetByCode(stageCode, Direction.Simulation)).StageWaypoints!.Waypoints;

    public async Task<string?> GetPathByStageCode(int stageCode) =>
        (await this.GetByCode(stageCode, Direction.Simulation)).StageWaypoints!.Path;

    public async Task UpdatePath(int stageCode, string path)
    {
        var stageWaypoints = await _dbContext.StageWaypoints.FirstAsync(x => x.StageCode == stageCode);
        stageWaypoints.Path = path;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Stage> GetByCode(int stageCode, Direction direction)
    {
        string cacheKey = $"{StageByCodeAndDirection}_{stageCode}_{direction}";

        Stage? cacheValue = _cache.Get<Stage>(cacheKey);
        if (cacheValue != null)
        {
            return cacheValue;
        }

        var dbValue = await _dbContext.Stages
            .Where(x => x.Code == stageCode && x.Direction == direction)
            .FillFullData()
            .FirstAsync();
        _cache.Set(cacheKey, dbValue);
        return dbValue;
    }

    public async Task<List<Stage>> GetAllByStageCodeBetween(int min, int max)
    {
        string cacheKey = $"{AllStagesByMinMaxKey}_{min}_{max}";

        List<Stage>? cacheValue = _cache.Get<List<Stage>>(cacheKey);
        if (cacheValue != null)
        {
            return cacheValue;
        }

        var dbValue = await _dbContext.Stages.FillFullData()
            .Where(x => x.Code >= min && x.Code <= max)
            .ToListAsync();
        _cache.Set(cacheKey, dbValue);
        return dbValue;
    }

    public async Task<List<Stage>> GetAll()
    {
        List<Stage>? cacheValue = _cache.Get<List<Stage>>(AllStagesKey);
        if (cacheValue != null)
        {
            return cacheValue;
        }

        var dbValue = await _dbContext.Stages.FillFullData().ToListAsync();
        _cache.Set(AllStagesKey, dbValue);
        return dbValue;
    }

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