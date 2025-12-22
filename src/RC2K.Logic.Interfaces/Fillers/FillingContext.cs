using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Fillers;

public class FillingContext
{
    public Dictionary<Guid, BonusPoints> BonusPoints { get; } = [];
    public Dictionary<Guid, RankingSnapshot> Rankings { get; } = [];
    public Dictionary<Guid, TimeEntry> TimeEntries { get; } = [];
    public Dictionary<Guid, Driver> Drivers { get; } = [];
    public Dictionary<Guid, User> Users { get; } = [];
    public Dictionary<Guid, VerifyInfo> VerifyInfos { get; } = [];
}
