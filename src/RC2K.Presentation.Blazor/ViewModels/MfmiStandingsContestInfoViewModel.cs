using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.ViewModels;

public class MfmiStandingsContestInfoViewModel
{
    public List<MfmiStandingsStageContestInfoViewModel> Stages { get; set; } = [];
}

public class MfmiStandingsStageContestInfoViewModel
{
    public required string StageName { get; init; }
    public required string StageImage { get; init; }
    public bool IsRallySummary { get; set; }
    public List<MfmiStandingsEntryListItemViewModel> Entries { get; } = [];
}

public class MfmiStandingsEntryListItemViewModel
{
    public int Rank { get; set; }
    public int Nr { get; init; }
    public required Driver Driver { get; init; }
    public required string Group { get; init; }
    public required Car Car { get; init; }
    public TimeSpan Time { get; set; }
    public string TimeDisplay => (Time == TimeSpan.MaxValue) ? "N/A" : Time.ToString(@"m\:ss\.ff");
    public TimeSpan Gap { get; set; }
    public string GapDisplay => TimeDisplay == "N/A" ? "N/A" : (Gap == TimeSpan.Zero ? "" : $"- {Gap.ToString(@"m\:ss\.ff")}");
    public List<Proof> Proofs { get; set; } = [];
}
