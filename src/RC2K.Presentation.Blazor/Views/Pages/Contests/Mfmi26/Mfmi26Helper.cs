using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;
using System.Reflection.Metadata.Ecma335;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

public static class Mfmi26Helper
{
    private static readonly Dictionary<string, int> _name2nr = 
        ContestsDefinitions.Instance.Mfmi26.Participants.ToDictionary(x => x.Name, x => x.Nr);

    private static readonly Dictionary<string, int> _name2group =
        ContestsDefinitions.Instance.Mfmi26.Participants.ToDictionary(x => x.Name, x => x.Group);

    private static readonly Dictionary<string, string> _name2car =
        ContestsDefinitions.Instance.Mfmi26.Participants.ToDictionary(x => x.Name, x => x.Car);

    public static int GetNr(string driver) =>
        _name2nr.TryGetValue(driver, out int res) ? res : -1;

    public static string GetGroup(string driver) => 
        _name2group.TryGetValue(driver, out int res) ? res.ToString() : "?";

    public static string GetCar(string driver) =>
        _name2car.TryGetValue(driver, out string? res) ? (res ?? "?") : "?";

    public static int GetCompetitionDay()
    {
        DateTime nowUtc = DateTime.UtcNow;

        if (nowUtc <= UtcFromPolishTime(2026, 09, 24, 18)) return -1; // not started;
        if (nowUtc <= UtcFromPolishTime(2026, 09, 26, 20)) return 1;
        if (nowUtc <= UtcFromPolishTime(2026, 09, 27, 20)) return 2;
        if (nowUtc <= UtcFromPolishTime(2026, 09, 28, 20)) return 3;
        return 20; // competition ended
    }

    private static DateTime UtcFromPolishTime(int year, int month, int day, int hour)
    {
        TimeZoneInfo polandZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        DateTime targetPolishLocal = new(year, month, day, hour, 0, 0, DateTimeKind.Unspecified);
        DateTime targetUtc = TimeZoneInfo.ConvertTimeToUtc(targetPolishLocal, polandZone); // handles CET and CEST
        return targetUtc;
    }
}
