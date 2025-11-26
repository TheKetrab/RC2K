using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface ITimeEntryService
{
    Task Upload(int stageId, int carId, Guid driverId, int min, int sec, int cc, List<Proof> proofs, string? labels);
    Task Upload(TimeEntry timeEntry);
    Task<List<TimeEntry>> Get(int stageId, int? carId = null);    
}
