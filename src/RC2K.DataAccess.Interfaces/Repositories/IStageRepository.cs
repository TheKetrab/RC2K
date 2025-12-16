using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IStageRepository : IGenericRepository<Stage>
{
    Task<List<Stage>> GetAll();
    Task<List<Stage>> GetAllByStageCodeBetween(int min, int max);
    Task<string> GetWaypointsByStageCode(int stageCode);
    Task UpdatePath(int stageCode, string path);
    Task<string?> GetPathByStageCode(int stageCode);
    Task<Stage> GetByCode(int stageCode, Direction direction);

}
