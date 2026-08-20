using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.ViewModels;

public class MfmiSummariesContestInfoViewModel
{
    public List<MfmiSummariesRallyContestInfoViewModel> Rallies = [];
}

public class MfmiSummariesRallyContestInfoViewModel
{
    public RallyCode? RallyCode { get; set; }
    public string RallyName { get; set; }
    public string RallyImage { get; set; }
    public bool IsFinalSummary { get; set; }
    public List<MfmiSummariesEntryListItemViewModel> Entries = [];
}

public class MfmiSummariesEntryListItemViewModel
{
    public int Rank { get; set; }
    public required int Nr { get; init; }
    public string? DriverNationality { get; set; }
    public required string DriverFriendlyName { get; init; }
    public required string Group { get; init; }
    public int CarId { get; set; }
    public TimeSpan Time { get; set; }
    public string TimeDisplay => (Time == TimeSpan.MaxValue) ? "N/A" : Time.ToString(@"h\:mm\:ss\.ff");
    public TimeSpan TimeWcb { get; set; }
    public string TimeWcbDisplay => (Time == TimeSpan.MaxValue) ? "N/A" : TimeWcb.ToString(@"h\:mm\:ss\.ff");
    public int Points { get; set; }
}

