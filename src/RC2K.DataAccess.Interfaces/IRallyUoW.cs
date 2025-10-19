using RC2K.DataAccess.Interfaces.Repositories;

namespace RC2K.DataAccess.Interfaces;

public interface IRallyUoW
{
    ICarRepository Cars { get; }
    IDriverRepository Drivers { get; }
    IStageRepository Stages { get; }
    ITimeEntryRepository TimeEntries { get; }
    IUserRepository Users { get; }
    IVerifyInfoRepository VerifyInfos { get; }
}
