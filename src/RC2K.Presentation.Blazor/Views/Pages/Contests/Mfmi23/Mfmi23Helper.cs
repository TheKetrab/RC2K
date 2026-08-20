using RC2K.DomainModel;
using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi23;

public static class Mfmi23Helper
{
    static Mfmi23Helper()
    {
        var contest = ContestsDefinitions.Instance.Mfmi23;
        contest.Participants.ForEach(x =>
        {
            _name2nr.Add(x.Name, x.Nr);
            _name2group.Add(x.Name, x.Group);
            _name2car.Add(x.Name, x.Car);
        });
    }

    private static readonly Dictionary<string, int> _name2nr = [];
    private static readonly Dictionary<string, int> _name2group = [];
    private static readonly Dictionary<string, string> _name2car = [];

    public static int GetNr(string driver) =>
        _name2nr.TryGetValue(driver, out int res) ? res : -1;

    public static string GetGroup(string driver) =>
        _name2group.TryGetValue(driver, out int res) ? res.ToString() : "?";

    public static string GetCar(string driver) =>
        _name2car.TryGetValue(driver, out string? res) ? (res ?? "?") : "?";

    public static double GetCarModifier(Car car) => car.Id switch
    {
        8 => 1.05, // Mitsubishi Lancer Evo V
        15 => 1.04, // Peugeot 206 WRC
        5 => 1.03, // Subaru Impreza WRC
        23 => 1.02, // Mitsubishi Lancer Evo IV
        3 => 1.01, // Seat Cordoba WRC
        _ => 1
    };

    public static int GetCompetitionDay()
    {
        // TODO to polish time
        if (DateTime.Now <= new DateTime(2023, 07, 21)) return 1;
        if (DateTime.Now <= new DateTime(2023, 07, 22)) return 2;
        if (DateTime.Now <= new DateTime(2023, 07, 23)) return 3;
        if (DateTime.Now <= new DateTime(2023, 07, 24)) return 4;
        // TODO ...
        return 20;
    }


}

