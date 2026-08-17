using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;
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
        // TODO to polish time
        if (DateTime.Now <= new DateTime(2024, 07, 21)) return 1;
        if (DateTime.Now <= new DateTime(2024, 07, 22)) return 2;
        if (DateTime.Now <= new DateTime(2024, 07, 23)) return 3;
        if (DateTime.Now <= new DateTime(2024, 07, 24)) return 4;
        // TODO ...
        return 20;
    }

    public class StandingsInfo
    {
        public Driver Driver { get; set; }
        public TimeSpan Gap { get; set; }
        public TimeSpan Time { get; set; }
        public int Rank { get; set; }
        public List<Proof> Proofs { get; set; }
    }

    // TODO: memory cache
    public async static Task<List<StandingsInfo>> GetStandingsInfo(ITimeEntryService timeEntryService, Stage stage, string labelFilter)
    {
        var timeEntries = (await timeEntryService.Get(stage.Id))
            .Where(x => x.Labels?.Contains(labelFilter) ?? false)
            .ToList();

        var best = timeEntries.OrderBy(x => x.Time).First();
        List<StandingsInfo> res = timeEntries.Select(te => new StandingsInfo
        {
            Driver = te.Driver!,
            Gap = te.Time - best.Time,
            Proofs = te.Proofs,
            Time = te.Time.ToTimeSpan()
        }).ToList();
        
        var standings =
            res.OrderBy(x => x.Time)
               .GroupBy(x => x.Time)
               .ToList();
        var ranks = Utils.Utils.CalculateRanked(standings).ToDictionary(x => x.item, x => x.rank);
        res.ForEach(s => s.Rank = ranks[s] + 1);
        res = res.OrderBy(x => x.Rank).ToList();

        // TODO: add N/A: DNS, DSQ, DNF

        return res;
    }

    public static List<StandingsInfo> GetRallyStandingsInfo(Dictionary<int, List<StandingsInfo>> standingsByStageId)
    {
        List<StandingsInfo> res = [];
        var allDrivers =
            standingsByStageId.Values
                .SelectMany(x => x)
                .Select(x => x.Driver)
                .DistinctBy(x => x.Id)
                .ToList();
        foreach (var d in allDrivers)
        {
            StandingsInfo info = new()
            {
                Driver = d,
                Gap = TimeSpan.Zero,
                Proofs = [],
                Rank = -1,
            };

            TimeSpan totalTime = new(0, 0, 0, 0, 0);
            bool isGlobalNa = false;
            foreach (var (k,v) in standingsByStageId)
            {
                if (v.Count == 0)
                {
                    continue; // level not completed yet
                }

                var entry = v.FirstOrDefault(x => x.Driver.Id == d.Id);
                if (entry is null)
                {
                    isGlobalNa = true;
                }
                else
                {
                    totalTime = totalTime.Add(entry.Time);
                }
            }
            if (isGlobalNa)
            {
                totalTime = TimeSpan.MaxValue;
            }
            info.Time = totalTime;
            res.Add(info);
        }

        var best = res.OrderBy(x => x.Time).First();
        res.ForEach(x => x.Gap = x.Time - best.Time);

        var standings =
            res.OrderBy(x => x.Time)
               .GroupBy(x => x.Time)
               .ToList();
        var ranks = Utils.Utils.CalculateRanked(standings).ToDictionary(x => x.item, x => x.rank);
        res.ForEach(entry => entry.Rank = ranks[entry] + 1);
        res = res.OrderBy(x => x.Rank).ToList();

        return res;
    }
}

