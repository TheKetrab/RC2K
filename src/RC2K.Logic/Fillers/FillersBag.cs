using RC2K.Logic.Interfaces.Fillers;
using System.Diagnostics.CodeAnalysis;

namespace RC2K.Logic.Fillers;

[ExcludeFromCodeCoverage]
public sealed class FillersBag(
    IBonusPointsFiller bonusPointsFiller,
    IDriverFiller driverFiller,
    IRankingFiller rankingFiller,
    ITimeEntryFiller timeEntryFiller,
    IUserFiller userFiller,
    IVerifyInfoFiller verifyInfoFiller)
    : IFillersBag
{
    public IBonusPointsFiller BonusPointsFiller { get; } = bonusPointsFiller;
    public IDriverFiller DriverFiller { get; } = driverFiller;
    public IRankingFiller RankingFiller { get; } = rankingFiller;
    public ITimeEntryFiller TimeEntryFiller { get; } = timeEntryFiller;
    public IUserFiller UserFiller { get; } = userFiller;
    public IVerifyInfoFiller VerifyInfoFiller { get; } = verifyInfoFiller;
}
