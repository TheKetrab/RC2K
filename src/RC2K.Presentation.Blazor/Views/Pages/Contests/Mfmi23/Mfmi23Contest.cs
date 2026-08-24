using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi23;

public class Mfmi23Participant : MfmiParticipant
{
    public int Group { get; set; }
    public string DriverProfile { get; set; }
    public Mfmi23Participant(string name, string car, int nr, int group, string driverProfile)
        : base(name, car, nr)
    {
        Group = group;
        DriverProfile = driverProfile;
        CustomProperties.Add(Constants.GroupProperty, Group);
        CustomProperties.Add(Constants.DriverProfileProperty, DriverProfile);
    }
}

public class Mfmi23Contest : IContest<Mfmi23Participant>
{
    public string MainLink => "mfmi23";
    public Dictionary<string, object> CustomProperties { get; } = new()
    {
        { Constants.PdfManualLinkProperty, "" }
    };
    public bool IsContestActive => Mfmi23Helper.GetCompetitionDay() < 20;

    public List<Mfmi23Participant> Participants { get; } =
    [
        new("Ephemeral", "Mitsubishi Lancer Evo IV", 25, 1, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g1_ephemeral.png" ),
        new("Tribell", "Peugeot 206 WRC", 39, 1, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g1_tribell.png" ),
        new("SpartaRemixer", "Subaru Impreza WRC", 2, 1, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g1_spartaremixer.png" ),
        new("Erwto", "Mitsubishi Lancer Evo IV", 20, 2, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g2_erwto.png" ),
        new("TheKetrab", "Mitsubishi Lancer Evo V", 10, 2, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g2_theketrab.png" ),
        new("Migger", "Proton Wira/Persona", 23, 2, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g2_migger.png" ),
        new("Pendzior", "Subaru Impreza WRC", 8, 2, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g2_pendzior.png" ),
        new("Red_T", "Proton Wira/Persona", 19, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g3_red_t.png" ),
        new("XsaraTorrada", "Peugeot 206 WRC", 69, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g3_xsaratorrada.png" ),
        new("Karel Pipa", "Seat Cordoba WRC", 55, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g3_karelpipa.png" ),
        new("Lewsys", "Peugeot 206 WRC", 14, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g4_lewsys.png" ),
        new("sBinnala", "Subaru Impreza WRC", 16, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g4_sbinnala.png" ),
        new("SForman135LS", "Subaru Impreza WRC", 90, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g4_sforman135ls.png" ),
        new("Certare", "Peugeot 206 WRC", 26, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g4_certare.png" ),
        new("P. Klima", "Mitsubishi Lancer Evo V", 77, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g4_p.klima.png" ),
        new("Kryspa7", "Subaru Impreza WRC", 7, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI23/g4_kryspa7.png" ),
    ];

}
