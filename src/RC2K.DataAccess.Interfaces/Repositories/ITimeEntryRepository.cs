using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface ITimeEntryRepository
{
    event EventHandler<(string, double)>? RequestUnitsHandler;
    Task Create(TimeEntry entity);
    Task<List<TimeEntry>> GetAll();
    Task<TimeEntry?> GetById(Guid id);
    Task<List<TimeEntry>> GetByStageId(int stageId);
    Task<List<TimeEntry>> GetByStageIdAndCarId(int stageId, int carId);
    Task<List<TimeEntry>> GetByStageIdAndCarIdAndDriverIdAndTime(int stageId, int carId, Guid driverId, TimeOnly time);
}
