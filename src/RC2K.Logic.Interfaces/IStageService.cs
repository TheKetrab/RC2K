using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IStageService
{
    Task<List<Stage>> GetAllFilled();
    Task<List<Stage>> GetAllFilledByRallyCode(RallyCode rallyCode);
    Task<List<double[]>> GetWaypoints(int stageCode, bool arcade);
}
