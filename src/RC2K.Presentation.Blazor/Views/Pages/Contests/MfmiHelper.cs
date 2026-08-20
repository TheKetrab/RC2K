using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi24;
using RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

public static class MfmiHelper
{
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
            foreach (var (k, v) in standingsByStageId)
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