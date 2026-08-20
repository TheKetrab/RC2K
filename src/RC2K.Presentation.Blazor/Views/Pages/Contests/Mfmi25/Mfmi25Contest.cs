using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi25;

public class Mfmi25Participant : MfmiParticipant
{
    public int Group { get; set; }
    public string Team { get; set; }
    public Mfmi25Participant(string name, string car, int nr, int group, string team)
        : base(name, car, nr)
    {
        Group = group;
        Team = team;
        CustomProperties.Add(Constants.GroupProperty, Group);
        CustomProperties.Add(Constants.TeamProperty, Team);
    }
}

public class Mfmi25Contest : IContest<Mfmi25Participant>
{
    public string MainLink => "mfmi25";
    public Dictionary<string, object> CustomProperties { get; } = new()
    {
        { Constants.PdfManualLinkProperty, "" }
    };
    public List<Mfmi25Participant> Participants { get; } =
    [
        new("Tribell", "Subaru Impreza WRC", 39, 1, "Alliart Rally Team" ),
        new("Karel Pipa", "Proton Wira/Persona", 55, 2, "Alliart Rally Team" ),
        new("Red_T", "Seat Cordoba WRC", 19, 3, "Alliart Rally Team" ),
        new("sBinnala", "Seat Cordoba WRC", 16, 4, "Alliart Rally Team" ),
        new("TPL", "Peugeot 206 WRC", 88, 4, "Alliart Rally Team" ),
        new("SpartaRemixer", "Subaru Impreza WRC", 2, 1, "🅱️ubaru Team" ),
        new("Twajlot", "Subaru Impreza WRC", 27, 1, "🅱️ubaru Team" ),
        new("TheKetrab", "Mitsubishi Lancer Evo V", 10, 2, "🅱️ubaru Team" ),
        new("Kryspa7", "Subaru Impreza WRC", 3, 4, "🅱️ubaru Team" ),
        new("InfamousGhost07", "Mitsubishi Lancer Evo IV", 7, 4, "🅱️ubaru Team" ),
        new("Ephemeral", "Proton Wira/Persona", 25, 1, "Kakaové Chlebíčki" ),
        new("Migger", "Mitsubishi Lancer Evo IV", 23, 2, "Kakaové Chlebíčki" ),
        new("Erwto", "Seat Cordoba WRC", 20, 2, "Kakaové Chlebíčki" ),
        new("SForman135LS", "Proton Wira/Persona", 90, 3, "Kakaové Chlebíčki" ),
        new("Noni", "Peugeot 206 WRC", 44, 3, "Kakaové Chlebíčki" ),
    ];

}
