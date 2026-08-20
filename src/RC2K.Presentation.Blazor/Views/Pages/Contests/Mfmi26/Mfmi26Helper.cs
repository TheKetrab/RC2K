namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

public static class Mfmi26Helper
{
    private static readonly Dictionary<string, int> _name2nr = new() {
       {"TheKetrab",10},
       {"Ephemeral",25},
       {"Migger",23},
    };

    private static readonly Dictionary<string, int> _name2group = new() {
       {"TheKetrab",3},
       {"Ephemeral",1},
       {"Migger",3},
    };

    private static readonly Dictionary<string, string> _name2car = new()
    {
        {"TheKetrab", "Mitsubishi Lancer Evo V" },
        {"Ephemeral", "Seat Cordoba WRC" },
        {"Migger", "Mitsubishi Lancer Evo V" },
    };

    public static int GetNr(string driver) =>
        _name2nr.TryGetValue(driver, out int res) ? res : -1;

    public static string GetGroup(string driver) =>
        _name2group.TryGetValue(driver, out int res) ? res.ToString() : "?";

    public static string GetCar(string driver) =>
        _name2car.TryGetValue(driver, out string? res) ? (res ?? "?") : "?";

    public static int GetCompetitionDay()
    {
        return -1; // not started
    }
}
