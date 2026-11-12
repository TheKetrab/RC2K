using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

public class Mfmi26Contest : IContest<Mfmi26Participant>
{
    public string MainLink => "mfmi26";
    public Dictionary<string, object> CustomProperties { get; } = new()
    {
        { Constants.PdfManualLinkProperty, "" }
    };
    public bool IsContestActive => Mfmi26Helper.GetCompetitionDay() < 20;

    public List<Mfmi26Participant> Participants { get; } =
    [
        new("Ephemeral", "Peugeot 206 WRC", 25, 1),
        new("TheKetrab", "Mitsubishi Lancer Evo V", 10, 3),
        new("mfmi26testUserA", "Seat Cordoba WRC", 77, 2),
        new("mfmi26testUserB", "Subaru Impreza WRC", 38, 3),
        new("mfmi26testUserC", "Peugeot 206 WRC", 33, 1),
        new("mfmi26testUserD", "Mitsubishi Lancer Evo IV", 92, 4),
    ];

}
