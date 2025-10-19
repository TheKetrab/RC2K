using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;

namespace RC2K.DataAccess;

public class RallyUoW : IRallyUoW
{
    public RallyUoW(ICarRepository carRepository,
                    IDriverRepository driverRepository,
                    IStageRepository stageRepository, 
                    ITimeEntryRepository timeEntryRepository,
                    IUserRepository userRepository,
                    IVerifyInfoRepository verifyInfoRepository)
    {
        Cars = carRepository;
        Drivers = driverRepository;
        Stages = stageRepository;
        TimeEntries = timeEntryRepository;
        Users = userRepository;
        VerifyInfos = verifyInfoRepository;
    }

    public ICarRepository Cars { get; set; }
    public IDriverRepository Drivers { get; set; }
    public IStageRepository Stages { get; set; }
    public ITimeEntryRepository TimeEntries { get; set; }
    public IUserRepository Users { get; set; }
    public IVerifyInfoRepository VerifyInfos { get; set; }
}
