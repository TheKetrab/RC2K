using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public class TimeEntryRepository(IDataContext context)
    : AbstractRepository<TimeEntry>(context), ITimeEntryRepository
{
    protected override IQueryable<TimeEntry> DataSet => _context.TimeEntries;
}
