using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;
using RC2K.Extensions;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class StageService : IStageService
{
    public readonly IRallyUoW _rallyUoW;

    public StageService(IRallyUoW rallyUoW)
    {
        _rallyUoW = rallyUoW;
    }

    public async Task<Stage?> GetByCode(int stageCode, Direction direction) =>
        (await _rallyUoW.Stages.Get(
            x => x.Code == stageCode && x.Direction == direction, full: true))
        .FirstOrDefault();

    public Task<List<Stage>> GetAll() =>
        _rallyUoW.Stages.GetAll();

    public Task<List<Stage>> GetAllByRallyCode(RallyCode rallyCode)
    {
        (int from, int to) = rallyCode switch
        {
            RallyCode.Sony => (21, 26),
            RallyCode.Vauxhall => (41, 46),
            RallyCode.Pirelli => (61, 66),
            RallyCode.Scottish => (31, 36),
            RallyCode.Seat => (71, 76),
            RallyCode.Stena => (51, 56),
            _ => throw new Exception()
        };

        return _rallyUoW.Stages.Get(
            x => x.Code >= from && x.Code <= to,
            x => Queryable.OrderBy(x, stage => stage.Code),
            full: true);
    }

    public async Task<List<double[]>> GetWaypoints(int stageCode, bool arcade)
    {
        var stageWaypoints = await _rallyUoW.Stages.GetWaypointsByStageCode(stageCode);

        if (stageWaypoints is null)
            return [];

        var waypoints = stageWaypoints.SplitClean(";")
            .Select(x => x.Split(",").Select(y => double.Parse(y.Trim())).ToArray());

        if (arcade)
            waypoints = waypoints.Reverse();

        return waypoints.ToList();
    }

    public async Task<string?> GetPath(int stageCode)
    {
        var stagePath = await _rallyUoW.Stages.GetPathByStageCode(stageCode);
        return stagePath;
    }

    public async Task SetPath(int stageCode, string path)
    {
        await _rallyUoW.Stages.UpdatePath(stageCode, path);
        await _rallyUoW.Save();
    }
}
