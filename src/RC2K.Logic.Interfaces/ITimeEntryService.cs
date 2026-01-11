using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Dtos;

namespace RC2K.Logic.Interfaces;

public interface ITimeEntryService
{
    Task<Result> Upload(int stageId, int carId, Guid driverId, int min, int sec, int cc, List<Proof> proofs, string? labels, string driverKey);
    Task<Result> Upload(int stageId, int carId, Guid driverId, int min, int sec, int cc, List<Proof> proofs, string? labels);
    Task Upload(TimeEntry timeEntry);
    Task Delete(List<TimeEntry> timeEntries);

    Task<List<TimeEntry>> Get(int stageId, int? carId = null);
    Task<List<TimeEntry>> GetAllNotVerified();
    Task Verify(List<TimeEntry> timeEntries, Guid verifierId, string comment);
    Task Verify(List<TimeEntry> timeEntries, string comment);
    Task<TimeEntriesCollectionInfo> CalculateTimeEntriesWithPoints(int stageId, int maximum = -1);

}
