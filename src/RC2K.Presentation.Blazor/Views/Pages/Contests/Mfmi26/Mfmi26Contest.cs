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
         new("Diatosta", "Peugeot 206 WRC", 67, 3), // XsaraTorrada
         new("Red_T", "Mitsubishi Lancer Evo V", 19, 2),
         new("sBinnala", "Subaru Impreza WRC", 16, 4),
         new("TPL", "Proton Wira/Persona", 88, 4),
         new("SpartaRemixer", "Subaru Impreza WRC", 2, 1),
         new("Twajlot", "Peugeot 206 WRC", 27, 2),
         new("TheKetrab", "Subaru Impreza WRC", 10, 2),
         new("doufun11", "Subaru Impreza WRC", 3, 4),
         new("InfamousGhost07", "Peugeot 206 WRC", 7, 4),
         new("Ephemeral", "Mitsubishi Lancer Evo V", 25, 1),
         new("Migger", "Subaru Impreza WRC", 23, 2),
         new("Casey", "Peugeot 206 WRC", 20, 2),
         new("SForman135LS", "Mitsubishi Lancer Evo V", 92, 3),
         new("Noni", "Mitsubishi Lancer Evo IV", 44, 3),
    ];

}
