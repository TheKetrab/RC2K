using RC2K.DomainModel;
using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;
using RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi24;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi25;

public static class Mfmi25Helper
{
    static Mfmi25Helper()
    {
        var contest = ContestsDefinitions.Instance.Mfmi25;
        contest.Participants.ForEach(x =>
        {
            _name2nr.Add(x.Name, x.Nr);
            _name2group.Add(x.Name, x.Group);
            _name2team.Add(x.Name, x.Team);
            _name2car.Add(x.Name, x.Car);
        });
    }

    private static readonly Dictionary<string, int> _name2nr = [];
    private static readonly Dictionary<string, int> _name2group = [];
    private static readonly Dictionary<string, string> _name2team = [];
    private static readonly Dictionary<string, string> _name2car = [];

    public static int GetNr(string driver) =>
        _name2nr.TryGetValue(driver, out int res) ? res : -1;

    public static string GetGroup(string driver) =>
        _name2group.TryGetValue(driver, out int res) ? res.ToString() : "?";

    public static string GetTeam(string driver) =>
        _name2team.TryGetValue(driver, out string? res) ? res.ToString() : "?";

    public static string GetCar(string driver) =>
        _name2car.TryGetValue(driver, out string? res) ? (res ?? "?") : "?";

    public static double GetCarModifier(Car car) =>
        Mfmi24Helper.GetCarModifier(car);

    public static int GetCompetitionDay()
    {
        if (DateTime.Now <= new DateTime(2025, 10, 27)) return 1;
        if (DateTime.Now <= new DateTime(2025, 10, 28)) return 2;
        if (DateTime.Now <= new DateTime(2025, 10, 29)) return 3;
        if (DateTime.Now <= new DateTime(2025, 10, 31)) return 4;
        if (DateTime.Now <= new DateTime(2025, 11, 1)) return 5;
        if (DateTime.Now <= new DateTime(2025, 11, 2)) return 6;
        if (DateTime.Now <= new DateTime(2025, 11, 3)) return 7;
        if (DateTime.Now <= new DateTime(2025, 11, 4)) return 8;
        if (DateTime.Now <= new DateTime(2025, 11, 6)) return 9;
        if (DateTime.Now <= new DateTime(2025, 11, 7)) return 10;
        if (DateTime.Now <= new DateTime(2025, 11, 8)) return 11;
        if (DateTime.Now <= new DateTime(2025, 11, 10)) return 12;
        if (DateTime.Now <= new DateTime(2025, 11, 11)) return 13;
        if (DateTime.Now <= new DateTime(2025, 11, 13)) return 14;
        if (DateTime.Now <= new DateTime(2025, 11, 14)) return 15;
        if (DateTime.Now <= new DateTime(2025, 11, 15)) return 16;
        if (DateTime.Now <= new DateTime(2025, 11, 17)) return 17;
        if (DateTime.Now <= new DateTime(2025, 11, 18)) return 18;
        return 20;
    }
}
