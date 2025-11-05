using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IStageService
{
    Task<Stage?> GetByCode(int stageCode, Direction direction);
    Task<List<Stage>> GetAll();
    Task<List<Stage>> GetAllByRallyCode(RallyCode rallyCode);
    Task<List<double[]>> GetWaypoints(int stageCode, bool arcade);
    Task SetPath(int stageCode,  string path);
    Task<string?> GetPath(int stageCode);
}
