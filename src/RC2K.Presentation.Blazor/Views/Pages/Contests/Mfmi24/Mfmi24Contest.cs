using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi24;

public class Mfmi24Participant : MfmiParticipant
{
    public int Group { get; set; }
    public string DriverProfile { get; set; }
    public Mfmi24Participant(string name, string car, int nr, int group, string driverProfile)
        : base(name, car, nr)
    {
        Group = group;
        DriverProfile = driverProfile;
        CustomProperties.Add(Constants.GroupProperty, Group);
        CustomProperties.Add(Constants.DriverProfileProperty, DriverProfile);
    }
}

public class Mfmi24Contest : IContest<Mfmi24Participant>
{
    public string MainLink => "mfmi24";
    public Dictionary<string, object> CustomProperties { get; } = new()
    {
        { Constants.PdfManualLinkProperty, "" }
    };
    public List<Mfmi24Participant> Participants { get; } =
    [
        new("Ephemeral", "Peugeot 206 WRC", 25, 1, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g1_ephemeral.png" ),
        new("SpartaRemixer", "Subaru Impreza WRC", 2, 1, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g1_spartaremixer.png" ),
        new("Tribell", "Mitsubishi Lancer Evo IV", 39, 1, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g1_tribell.png" ),
        new("TheKetrab", "Mitsubishi Lancer Evo V", 10, 2, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g2_theketrab.png" ),
        new("Karel Pipa", "Seat Cordoba WRC", 55, 2, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g2_karelpipa.png" ),
        new("Erwto", "Subaru Impreza WRC", 20, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g3_erwto.png" ),
        new("Lewsys", "Peugeot 206 WRC", 14, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g3_lewsys.png" ),
        new("SForman135LS", "Subaru Impreza WRC", 90, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g3_sforman135ls.png" ),
        new("Red_T", "Subaru Impreza WRC", 19, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g3_red_t.png" ),
        new("Pendzior", "Seat Cordoba WRC", 8, 3, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g3_pendzior.png" ),
        new("sBinnala", "Seat Cordoba WRC", 16, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g4_sbinnala.png" ),
        new("P. Klima", "Mitsubishi Lancer Evo V", 77, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g4_p.klima.png" ),
        new("Woeringen1288", "Mitsubishi Lancer Evo V", 12, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g4_woeringen1288.png" ),
        new("Noni", "Peugeot 206 WRC", 44, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g4_noni.png" ),
        new("InfamousGhost07", "Mitsubishi Lancer Evo IV", 7, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g4_infamousghost07.png" ),
        new("BrosTheThird", "Subaru Impreza WRC", 30, 4, "https://redchili385.github.io/RC2K_JanuaryContest_2020/resources/driver_profiles/MFMI24/g4_brosthethird.png" ),
    ];

}
