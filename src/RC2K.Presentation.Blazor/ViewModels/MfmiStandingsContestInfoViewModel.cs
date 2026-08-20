using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.ViewModels;

public class MfmiStandingsContestInfoViewModel
{
    public List<MfmiStandingsStageContestInfoViewModel> Stages = [];
}

public class MfmiStandingsStageContestInfoViewModel
{
    public string StageName { get; set; }
    public string StageImage { get; set; }
    public bool IsRallySummary { get; set; }
    public List<MfmiStandingsEntryListItemViewModel> Entries = [];
}

public class MfmiStandingsEntryListItemViewModel
{
    public int Rank { get; set; }
    public int Nr { get; init; }
    public Driver Driver { get; set; }
    public string Group { get; init; }
    public Car Car { get; init; }
    public TimeSpan Time { get; set; }
    public string TimeDisplay => (Time == TimeSpan.MaxValue) ? "N/A" : Time.ToString(@"m\:ss\.ff");
    public TimeSpan Gap { get; set; }
    public string GapDisplay => TimeDisplay == "N/A" ? "N/A" : (Gap == TimeSpan.Zero ? "" : $"- {Gap.ToString(@"m\:ss\.ff")}");
    public List<Proof> Proofs { get; set; }
}
