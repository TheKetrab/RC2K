using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

public class Mfmi26Contest : IContest<MfmiParticipant>
{
    public string MainLink => "mfmi26";
    public Dictionary<string, object> CustomProperties { get; } = new()
    {
        { Constants.PdfManualLinkProperty, "" }
    };
    public bool IsContestActive => Mfmi26Helper.GetCompetitionDay() < 20;

    public List<MfmiParticipant> Participants { get; } =
    [
        new("Ephemeral", "Peugeot 206 WRC", 25),
        new("TheKetrab", "Mitsubishi Lancer Evo V", 10),
    ];

}
