namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

public static class Mfmi26Helper
{
    private static readonly Dictionary<string, int> _name2nr = new() {
    //    {"TheKetrab",10},
    //    {"Ephemeral",25},
    //    {"Migger",23},
    };

    private static readonly Dictionary<string, int> _name2group = new() {
    //    {"TheKetrab",3},
    //    {"Ephemeral",1},
    //    {"Migger",3},
    };

    private static readonly Dictionary<string, string> _name2car = new()
    {
        // {"TheKetrab", "Mitsubishi Lancer Evo V" },
        // {"Ephemeral", "Seat Cordoba WRC" },
        // {"Migger", "Mitsubishi Lancer Evo V" },
    };

    public static int GetNr(string driver) =>
        _name2nr.TryGetValue(driver, out int res) ? res : -1;

    public static string GetGroup(string driver) =>
        _name2group.TryGetValue(driver, out int res) ? res.ToString() : "?";

    public static string GetCar(string driver) =>
        _name2car.TryGetValue(driver, out string? res) ? (res ?? "?") : "?";

    public static int GetCompetitionDay()
    {
        TimeZoneInfo polishZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        DateTime plNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, polishZone);

        if (plNow <= new DateTime(2026, 11, 21, 20, 0, 0)) return -1;

        if (plNow <= new DateTime(2026, 11, 22, 20, 0, 0)) return 1;
        if (plNow <= new DateTime(2026, 11, 23, 20, 0, 0)) return 2;
        if (plNow <= new DateTime(2026, 11, 24, 20, 0, 0)) return 3;

        if (plNow <= new DateTime(2026, 11, 26, 20, 0, 0)) return 4;
        if (plNow <= new DateTime(2026, 11, 27, 20, 0, 0)) return 5;
        if (plNow <= new DateTime(2026, 11, 28, 20, 0, 0)) return 6;
        if (plNow <= new DateTime(2026, 11, 29, 20, 0, 0)) return 7;
        if (plNow <= new DateTime(2026, 11, 30, 20, 0, 0)) return 8;

        if (plNow <= new DateTime(2026, 12, 2, 20, 0, 0)) return 9;
        if (plNow <= new DateTime(2026, 12, 3, 20, 0, 0)) return 10;
        if (plNow <= new DateTime(2026, 12, 4, 20, 0, 0)) return 11;

        if (plNow <= new DateTime(2026, 12, 6, 20, 0, 0)) return 12;
        if (plNow <= new DateTime(2026, 12, 7, 20, 0, 0)) return 13;

        if (plNow <= new DateTime(2026, 12, 9, 20, 0, 0)) return 14;
        if (plNow <= new DateTime(2026, 12, 10, 20, 0, 0)) return 15;
        if (plNow <= new DateTime(2026, 12, 11, 20, 0, 0)) return 16;

        if (plNow <= new DateTime(2026, 12, 13, 20, 0, 0)) return 17;
        if (plNow <= new DateTime(2026, 12, 14, 20, 0, 0)) return 18;

        return 20;
    }

}
