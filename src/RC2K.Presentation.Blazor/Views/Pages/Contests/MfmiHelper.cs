using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Utils;

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

    public async static Task<List<StandingsInfo>> GetStandingsInfo(ITimeEntryService timeEntryService, Stage stage, string labelFilter)
    {
        var timeEntries = (await timeEntryService.Get(stage.Id))
            .Where(x => x.Labels?.Contains(labelFilter) ?? false)
            .ToList();

        if (timeEntries.Count == 0)
        {
            return [];
        }

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
        if (res.Count == 0)
        {
            return [];
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

    public static HashSet<int> GetStagesCodesForCompetitionDay(int day)
    {
        HashSet<int> codesForToday = day switch
        {
            1 => [
                LevelHelper.GetStageCode(RallyCode.Vauxhall, 1),
                LevelHelper.GetStageCode(RallyCode.Vauxhall, 2)],
            2 => [
                LevelHelper.GetStageCode(RallyCode.Vauxhall, 3),
                LevelHelper.GetStageCode(RallyCode.Vauxhall, 4)],
            3 => [
                LevelHelper.GetStageCode(RallyCode.Vauxhall, 5),
                LevelHelper.GetStageCode(RallyCode.Vauxhall, 6)],
            4 => [LevelHelper.GetStageCode(RallyCode.Pirelli, 1)],
            5 => [LevelHelper.GetStageCode(RallyCode.Pirelli, 2)],
            6 => [LevelHelper.GetStageCode(RallyCode.Pirelli, 3)],
            7 => [LevelHelper.GetStageCode(RallyCode.Pirelli, 4)],
            8 => [
                LevelHelper.GetStageCode(RallyCode.Pirelli, 5),
                LevelHelper.GetStageCode(RallyCode.Pirelli, 6)],
            9 => [
                LevelHelper.GetStageCode(RallyCode.Scottish, 1),
                LevelHelper.GetStageCode(RallyCode.Scottish, 2),
                LevelHelper.GetStageCode(RallyCode.Scottish, 3)],
            10 => [
                LevelHelper.GetStageCode(RallyCode.Scottish, 4),
                LevelHelper.GetStageCode(RallyCode.Scottish, 5)],
            11 => [LevelHelper.GetStageCode(RallyCode.Scottish, 6)],
            12 => [
                LevelHelper.GetStageCode(RallyCode.Seat, 1),
                LevelHelper.GetStageCode(RallyCode.Seat, 2),
                LevelHelper.GetStageCode(RallyCode.Seat, 3)],
            13 => [
                LevelHelper.GetStageCode(RallyCode.Seat, 4),
                LevelHelper.GetStageCode(RallyCode.Seat, 5),
                LevelHelper.GetStageCode(RallyCode.Seat, 6)],
            14 => [
                LevelHelper.GetStageCode(RallyCode.Stena, 1),
                LevelHelper.GetStageCode(RallyCode.Stena, 2)],
            15 => [
                LevelHelper.GetStageCode(RallyCode.Stena, 3),
                LevelHelper.GetStageCode(RallyCode.Stena, 4)],
            16 => [
                LevelHelper.GetStageCode(RallyCode.Stena, 5),
                LevelHelper.GetStageCode(RallyCode.Stena, 6)],
            17 => [
                LevelHelper.GetStageCode(RallyCode.Sony, 1),
                LevelHelper.GetStageCode(RallyCode.Sony, 2),
                LevelHelper.GetStageCode(RallyCode.Sony, 3)],
            18 => [
                LevelHelper.GetStageCode(RallyCode.Sony, 4),
                LevelHelper.GetStageCode(RallyCode.Sony, 5),
                LevelHelper.GetStageCode(RallyCode.Sony, 6)],
            _ => []
        };

        return codesForToday;
    }
}