using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface ITimeEntryRepository
{
    event EventHandler<(string, double)>? RequestUnitsHandler;
    Task Create(TimeEntry entity);
    Task Update(TimeEntry entity);
    Task Delete(string id);

    Task<List<TimeEntry>> GetAll();
    Task<List<TimeEntry>> GetAllNotVerified();
    Task<TimeEntry?> GetById(Guid id, CancellationToken ct);
    Task<List<TimeEntry>> GetByStageId(int stageId, CancellationToken ct);
    Task<List<TimeEntry>> GetByStageIdAndCarId(int stageId, int carId, CancellationToken ct);
    Task<List<TimeEntry>> GetByStageIdAndCarIdAndDriverIdAndTime(int stageId, int carId, Guid driverId, TimeOnly time);
}
