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

    internal static readonly int[] _bonusCarPoints = _a5carPoints;

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