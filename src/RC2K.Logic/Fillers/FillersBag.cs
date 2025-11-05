using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class FillersBag : IFillersBag
{
    public IDriverFiller DriverFiller { get; }

    public ITimeEntryFiller TimeEntryFiller { get; }

    public IUserFiller UserFiller { get; }

    public IVerifyInfoFiller VerifyInfoFiller { get; }

    public FillersBag(IDriverFiller driverFiller,
                      ITimeEntryFiller timeEntryFiller,
                      IUserFiller userFiller,
                      IVerifyInfoFiller verifyInfoFiller)
    {
        DriverFiller = driverFiller;
        TimeEntryFiller = timeEntryFiller;
        UserFiller = userFiller;
        VerifyInfoFiller = verifyInfoFiller;
    }

}
