using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class FillersBag : IFillersBag
{
    public IBonusPointsFiller BonusPointsFiller { get; set; }
    public IDriverFiller DriverFiller { get; }

    public ITimeEntryFiller TimeEntryFiller { get; }

    public IUserFiller UserFiller { get; }

    public IVerifyInfoFiller VerifyInfoFiller { get; }

    public FillersBag(IBonusPointsFiller bonusPointsFiller,
                      IDriverFiller driverFiller,
                      ITimeEntryFiller timeEntryFiller,
                      IUserFiller userFiller,
                      IVerifyInfoFiller verifyInfoFiller)
    {
        BonusPointsFiller = bonusPointsFiller,
        DriverFiller = driverFiller;
        TimeEntryFiller = timeEntryFiller;
        UserFiller = userFiller;
        VerifyInfoFiller = verifyInfoFiller;
    }

}
