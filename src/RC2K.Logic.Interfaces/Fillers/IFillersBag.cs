using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Fillers;

public interface IFillersBag
{
    IDriverFiller DriverFiller { get; }
    ITimeEntryFiller TimeEntryFiller { get; }
    IUserFiller UserFiller { get; }
    IVerifyInfoFiller VerifyInfoFiller { get; }
}
