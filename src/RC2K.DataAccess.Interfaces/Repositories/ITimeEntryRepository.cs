using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface ITimeEntryRepository
{
    Task<List<TimeEntry>> GetAll();
    Task<TimeEntry?> GetById(Guid id);
    Task<List<TimeEntry>> GetByStageId(int stageId);
    Task<List<TimeEntry>> GetByStageIdAndCarId(int stageId, int carId);
}
