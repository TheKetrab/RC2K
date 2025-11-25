using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace RC2K.Logic;

public class PointsProvider : IPointsProvider
{
    // ski-jumping points system
    private static readonly int[] _generalPoints = [
        100,80,60,50,45,
        40,36,32,29,26,
        24,22,20,18,16,
        15,14,13,12,11,
        10,9,8,7,6,
        5,4,3,2,1
    ];

    private static readonly int[] _a8carPoints = [
        30,24,18,12,6
    ];

    private static readonly int[] _a7carPoints = [
        20,16,12,8,4
    ];

    private static readonly int[] _a6carPoints = _a7carPoints;

    private static readonly int[] _a5carPoints = [
        10,8,6,4,2
    ];

    private static readonly int[] _bonusCarPoints = _a5carPoints;

    public Dictionary<Guid, int> CalculateCarStagePoints(List<TimeEntry> timeEntries)
    {
        Dictionary<Guid, int> res = [];
        foreach (var carGroup in timeEntries.GroupBy(x => x.CarId))
        {
            var timeEntriesPerCar = carGroup.ToList();
            Car car = timeEntriesPerCar.First().Car!;

            var perDriver =
                timeEntriesPerCar.GroupBy(x => x.DriverId)
                                 .Select(g => g.MinBy(x => x.Time)!)
                                 .OrderBy(x => x.Time)
                                 .ToList();

            Func<int,int> getPoints = car.Class switch
            {
                8 => i => _a8carPoints[i],
                7 => i => _a7carPoints[i], 
                6 => i => _a6carPoints[i], 
                5 => i => _a5carPoints[i],
                _ => i => 0 // TODO handle bonus cars
            };

            for (int i=0; i<5 && i<perDriver.Count; i++)
            {
                res.Add(perDriver[i].Id, getPoints(i));
            }
        }

        return res;
    }

    public Dictionary<Guid, int> CalculateGeneralStagePoints(List<TimeEntry> timeEntries)
    {
        var perDriver =
            timeEntries.GroupBy(x => x.DriverId)
                       .Select(g => g.MinBy(x => x.Time)!)
                       .OrderBy(x => x.Time)
                       .ToList();

        Dictionary<Guid, int> res = [];
        for (int i = 0; i < 30 && i < perDriver.Count; i++)
        {
            res.Add(perDriver[i].Id, _generalPoints[i]);
        }

        return res;
    }
}