using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface ITimeEntryService
{
    Task Upload(TimeEntry timeEntry);
    Task<List<TimeEntry>> Get(int stageId, int? carId = null);    
}
