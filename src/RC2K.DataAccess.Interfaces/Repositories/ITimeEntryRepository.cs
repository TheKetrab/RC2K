using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface ITimeEntryRepository
{
    Task<List<TimeEntry>> GetAll();
    Task<TimeEntry> GetById(int id);
}
