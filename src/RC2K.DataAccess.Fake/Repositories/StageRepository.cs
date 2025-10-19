using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class StageRepository(IDataContext context)
    : AbstractRepository<Stage>(context), IStageRepository
{
    protected override IQueryable<Stage> DataSet => _context.Stages;

    public Task<List<Stage>> GetAllByRallyCodeBetween(int min, int max) =>
        Task.FromResult(DataSet.Where(x => x.Code >= min && x.Code <= max).ToList());

    public Task<string> GetWaypointsByStageCode(int stageCode) =>
        Task.FromResult(_context.StageWaypoints.First(x => x.StageCode == stageCode).Waypoints);

    public Task<Stage?> TryGetByCode(string code) =>
        Task.FromResult(DataSet.FirstOrDefault(x => x.Code == int.Parse(code)));
}
