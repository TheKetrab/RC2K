using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

[assembly: InternalsVisibleTo("RC2K.Logic.UnitTests")]

namespace RC2K.Logic;

public class PointsProvider : IPointsProvider
{
    // ski-jumping points system
    internal static readonly int[] _generalPoints = [
        100,80,60,50,45,
        40,36,32,29,26,
        24,22,20,18,16,
        15,14,13,12,11,
        10,9,8,7,6,
        5,4,3,2,1
    ];

    internal static readonly int[] _a8carPoints = [
        30,24,18,12,6
    ];

    internal static readonly int[] _a7carPoints = [
        20,16,12,8,4
    ];

    internal static readonly int[] _a6carPoints = _a7carPoints;

    internal static readonly int[] _a5carPoints = [
        10,8,6,4,2
    ];

    // not used, but stored to be guide for points additions
    internal static readonly int[] _mfmiPoints = [
        500,400,300,250,200,
        150,100,75,50,25
    ];

    internal static readonly int[] _bonusCarPoints = _a5carPoints;

    private static readonly Dictionary<int, int> _generalPointsReverseMap = ReverseMap(_generalPoints);
    private static readonly Dictionary<int, int> _a8carPointsReverseMap = ReverseMap(_a8carPoints);
    private static readonly Dictionary<int, int> _a7carPointsReverseMap = ReverseMap(_a7carPoints);
    private static readonly Dictionary<int, int> _a6carPointsReverseMap = ReverseMap(_a6carPoints);
    private static readonly Dictionary<int, int> _a5carPointsReverseMap = ReverseMap(_a5carPoints);
    private static readonly Dictionary<int, int> _bonusCarPointsReverseMap = ReverseMap(_bonusCarPoints);

    private static Dictionary<int, int> ReverseMap(int[] arr) =>
        arr.Select((x, i) => new { Val = x, Index = i }).ToDictionary(x => x.Val, x => x.Index);

    public int GetPlaceFromGeneralPoints(int generalPoints) =>
        _generalPointsReverseMap.TryGetValue(generalPoints, out int place) ? place : -1;

    public int GetPlaceFromA8CarPoints(int a8carPoints) =>
        _a8carPointsReverseMap.TryGetValue(a8carPoints, out int place) ? place : -1;

    public int GetPlaceFromA7CarPoints(int a7carPoints) =>
        _a7carPointsReverseMap.TryGetValue(a7carPoints, out int place) ? place : -1;

    public int GetPlaceFromA6CarPoints(int a6carPoints) =>
        _a6carPointsReverseMap.TryGetValue(a6carPoints, out int place) ? place : -1;

    public int GetPlaceFromA5CarPoints(int a5carPoints) =>
        _a5carPointsReverseMap.TryGetValue(a5carPoints, out int place) ? place : -1;

    public int GetPlaceFromBonusCarPoints(int bonusCarPoints) =>
        _bonusCarPointsReverseMap.TryGetValue(bonusCarPoints, out int place) ? place : -1;

    public Dictionary<Guid, int> CalculateCarStagePoints(List<TimeEntry> timeEntries)
    {
        Dictionary<Guid, int> res = [];
        foreach (var carGroup in timeEntries.GroupBy(x => x.CarId))
        {
            var timeEntriesPerCar = carGroup.ToList();
            Car car = timeEntriesPerCar.First().Car!;

            var bestOfDriverByTime =
                timeEntriesPerCar.GroupBy(x => x.DriverId)
                                 .Select(g => g.MinBy(x => x.Time)!)
                                 .OrderBy(x => x.Time)
                                 .GroupBy(x => x.Time)
                                 .ToList();

            var ranked = CalculateRanked(bestOfDriverByTime);

            Func<int,int> getPoints = car.Class switch
            {
                8 => i => _a8carPoints[i],
                7 => i => _a7carPoints[i], 
                6 => i => _a6carPoints[i], 
                5 => i => _a5carPoints[i],
                _ => i => 0 // TODO handle bonus cars
            };

            for (int i=0; i<5 && i< ranked.Count; i++)
            {
                res.Add(ranked[i].timeEntry.Id, getPoints(ranked[i].rank));
            }
        }

        return res;
    }

    public Dictionary<Guid, int> CalculateGeneralStagePoints(List<TimeEntry> timeEntries)
    {
        var bestOfDriverByTime =
            timeEntries.GroupBy(x => x.DriverId)
                       .Select(g => g.MinBy(x => x.Time)!) // only best time
                       .OrderBy(x => x.Time)
                       .GroupBy(x => x.Time)
                       .ToList();

        var ranked = CalculateRanked(bestOfDriverByTime);

        Dictionary<Guid, int> res = [];
        for (int i = 0; i < 30 && i < ranked.Count; i++)
        {
            res.Add(ranked[i].timeEntry.Id, _generalPoints[ranked[i].rank]);
        }

        return res;
    }

    public Dictionary<Guid, int> CalculatePlace(List<TimeEntry> timeEntries)
    {
        var standings =
            timeEntries.OrderBy(x => x.Time)
                       .GroupBy(x => x.Time)
                       .ToList();

        var ranked = CalculateRanked(standings);

        return ranked.ToDictionary(x => x.timeEntry.Id, x => x.rank + 1);
    }

    public Dictionary<Guid, int> CalculatePlaceByCar(List<TimeEntry> timeEntries)
    {
        Dictionary<Guid, int> res = [];
        foreach (var carGroup in timeEntries.GroupBy(x => x.CarId))
        {
            var timeEntriesPerCar = carGroup.ToList();
            Car car = timeEntriesPerCar.First().Car!;

            var standings =
                timeEntriesPerCar.OrderBy(x => x.Time)
                                 .GroupBy(x => x.Time)
                                 .ToList();

            var ranked = CalculateRanked(standings);

            foreach (var r in ranked)
            {
                res.Add(r.timeEntry.Id, r.rank + 1);
            }
        }

        return res;
    }

    private List<(TimeEntry timeEntry, int rank)> CalculateRanked(List<IGrouping<TimeOnly, TimeEntry>> standingsByTime)
    {
        List<(TimeEntry timeEntry, int rank)> ranked = [];        

        int rank = 0;
        foreach (var g in standingsByTime)
        {
            ranked.AddRange(g.Select(x => (x, rank)));
            rank += g.Count();
        }
        
        return ranked;
    }
}