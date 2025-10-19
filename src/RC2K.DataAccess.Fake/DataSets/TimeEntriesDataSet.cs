
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class TimeEntriesDataSet(IDataContext? context = null)
    : AbstractDataSet<TimeEntry, TimeEntryFaker, TimeEntriesDataSet>(context)
{
    protected override TimeEntryFaker CreateFaker() =>
        new TimeEntryFaker(_context ?? throw new NullReferenceException());
}
