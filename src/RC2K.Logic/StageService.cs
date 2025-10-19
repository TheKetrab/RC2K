using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Extensions;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class StageService : IStageService
{
    public readonly IStageRepository _stageRepository;

    public StageService(IStageRepository stageRepository)
    {
        _stageRepository = stageRepository;
    }

    public Task<List<Stage>> GetAllFilled() =>
        _stageRepository.GetAll();

    public Task<List<Stage>> GetAllFilledByRallyCode(RallyCode rallyCode)
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

        return _stageRepository.GetAllByRallyCodeBetween(from, to);
    }

    public async Task<List<double[]>> GetWaypoints(int stageCode, bool arcade)
    {
        var stageWaypoints = await _stageRepository.GetWaypointsByStageCode(stageCode);

        if (stageWaypoints is null)
            return [];

        var waypoints = stageWaypoints.SplitClean(";")
            .Select(x => x.Split(",").Select(y => double.Parse(y.Trim())).ToArray());

        if (arcade)
            waypoints = waypoints.Reverse();

        return waypoints.ToList();
    }
}
