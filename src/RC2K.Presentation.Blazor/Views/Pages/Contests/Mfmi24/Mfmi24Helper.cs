using RC2K.DomainModel;
using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi24;

public static class Mfmi24Helper
{
    static Mfmi24Helper()
    {
        var contest = ContestsDefinitions.Instance.Mfmi24;
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
        8 => 1.08, // Mitsubishi Lancer Evo V
        15 => 1.12, // Peugeot 206 WRC
        5 => 1.12, // Subaru Impreza WRC
        23 => 1.06, // Mitsubishi Lancer Evo IV
        3 => 1.04, // Seat Cordoba WRC
        _ => 1
    };

    public static int GetCompetitionDay()
    {
        return 20; // it's over
    }


}

