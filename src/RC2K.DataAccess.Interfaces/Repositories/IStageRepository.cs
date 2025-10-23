using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IStageRepository
{
    Task<List<Stage>> GetAll();
    Task<List<Stage>> GetAllByRallyCodeBetween(int min, int max);
    Task<Stage?> TryGetByCode(string code, bool arcade);
    Task<string> GetWaypointsByStageCode(int stageCode);
    Task UpdatePath(int stageCode, string path);
    Task<string?> GetPathByStageCode(int stageCode);

}
