using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class FillersBag : IFillersBag
{
    public IBonusPointsFiller BonusPointsFiller { get; set; }
    public IDriverFiller DriverFiller { get; }

    public ITimeEntryFiller TimeEntryFiller { get; }
    public IRankingFiller RankingFiller { get; }

    public IUserFiller UserFiller { get; }

    public IVerifyInfoFiller VerifyInfoFiller { get; }

    public FillersBag(IBonusPointsFiller bonusPointsFiller,
                      IDriverFiller driverFiller,
                      IRankingFiller rankingFiller,
                      ITimeEntryFiller timeEntryFiller,
                      IUserFiller userFiller,
                      IVerifyInfoFiller verifyInfoFiller)
    {
        BonusPointsFiller = bonusPointsFiller;
        DriverFiller = driverFiller;
        RankingFiller = rankingFiller;
        TimeEntryFiller = timeEntryFiller;
        UserFiller = userFiller;
        VerifyInfoFiller = verifyInfoFiller;
    }

}
