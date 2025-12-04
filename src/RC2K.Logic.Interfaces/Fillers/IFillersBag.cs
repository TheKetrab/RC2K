
namespace RC2K.Logic.Interfaces.Fillers;

public interface IFillersBag
{
    IBonusPointsFiller BonusPointsFiller { get; }
    IDriverFiller DriverFiller { get; }
    ITimeEntryFiller TimeEntryFiller { get; }
    IUserFiller UserFiller { get; }
    IVerifyInfoFiller VerifyInfoFiller { get; }
}
