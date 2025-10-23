using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces;

public interface IDataContext
{
    IQueryable<Car> Cars { get; }
    IQueryable<Driver> Drivers { get; }
    IQueryable<Stage> Stages { get; }
    IQueryable<StageData> StagesData { get; }
    IQueryable<StageWaypoints> StageWaypoints { get; }
    IQueryable<TimeEntry> TimeEntries { get; }
    IQueryable<User> Users { get; }
    IQueryable<VerifyInfo> VerifyInfos { get; }

    int SaveChanges();
}
