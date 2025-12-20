using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public class RankingService : IRankingService
{
    private readonly IRankingsRepository _rankingRepository;
    private readonly IStageService _stageService;
    private readonly ITimeEntryService _timeEntryService;
    private readonly IPointsProvider _pointsProvider;
    private readonly IBonusPointsService _bonusPointsService;
    private readonly IFillersBag _fillers;

    public RankingService(IRankingsRepository rankingRepository,
        IStageService stageService,
        ITimeEntryService timeEntryService,
        IPointsProvider pointsProvider,
        IBonusPointsService bonusPointsService,
                            IFillersBag fillers)
    {
        _rankingRepository = rankingRepository;
        _stageService = stageService;
        _timeEntryService = timeEntryService;
        _pointsProvider = pointsProvider;
        _bonusPointsService = bonusPointsService;

        _fillers = fillers;
    }

    public async Task<RankingSnapshot> GetLatest()
    {
        var ranking = await _rankingRepository.GetCurrent();

        await ranking.FillFullData(_fillers.RankingFiller, _fillers);

        return ranking;
    }

    public async Task<RankingSnapshot> CalculateCurrentRankingSnapshot()
    {
        Dictionary<Guid, int> _driver2generalPoints = [];
        Dictionary<Guid, int> _driver2generalTop30Count = [];
        Dictionary<Guid, int> _driver2generalTop10Count = [];
        Dictionary<Guid, int> _driver2generalTop3Count = [];
        Dictionary<Guid, int> _driver2generalTop1Count = [];

        Dictionary<Guid, int> _driver2carA8Points = [];
        Dictionary<Guid, int> _driver2carA8PointsTop5Count = [];
        Dictionary<Guid, int> _driver2carA8PointsTop1Count = [];

        Dictionary<Guid, int> _driver2carA7Points = [];
        Dictionary<Guid, int> _driver2carA7PointsTop5Count = [];
        Dictionary<Guid, int> _driver2carA7PointsTop1Count = [];

        Dictionary<Guid, int> _driver2carA6Points = [];
        Dictionary<Guid, int> _driver2carA6PointsTop5Count = [];
        Dictionary<Guid, int> _driver2carA6PointsTop1Count = [];

        Dictionary<Guid, int> _driver2carA5Points = [];
        Dictionary<Guid, int> _driver2carA5PointsTop5Count = [];
        Dictionary<Guid, int> _driver2carA5PointsTop1Count = [];

        Dictionary<Guid, int> _driver2bonusCarPoints = [];
        Dictionary<Guid, int> _driver2bonusCarPointsTop5Count = [];
        Dictionary<Guid, int> _driver2bonusCarPointsTop1Count = [];

        Dictionary<Guid, int> _driver2BonusPoints= [];


        var stages = await _stageService.GetAll();
        List<TimeEntry> totalTimeEntries = [];

        foreach (var stage in stages)
        {
            var timeEntries = await _timeEntryService.Get(stage.Id);
            Dictionary<Guid, Guid> _te2driver = timeEntries.ToDictionary(x => x.Id, x => x.DriverId);
            Dictionary<Guid, int> _te2carClass = timeEntries.ToDictionary(x => x.Id, x => x.Car!.Class);

            // ----- ----- ----- GENERAL POINTS
            var totalPoints = _pointsProvider.CalculateGeneralStagePoints(timeEntries);
            foreach (var (te, p) in totalPoints)
            {
                Guid driverId = _te2driver[te];

                int place = _pointsProvider.GetPlaceFromGeneralPoints(p);

                _driver2generalPoints.Inc(driverId, p);

                if (place <= 30)
                {
                    _driver2generalTop30Count.Inc(driverId, 1);
                }
                if (place <= 10)
                {
                    _driver2generalTop10Count.Inc(driverId, 1);
                }
                if (place <= 3)
                {
                    _driver2generalTop3Count.Inc(driverId, 1);
                }
                if (place == 1)
                {
                    _driver2generalTop1Count.Inc(driverId, 1);
                }
            }

            // ----- ----- ----- CAR POINTS
            var carPoints = _pointsProvider.CalculateCarStagePoints(timeEntries);
            foreach (var (te, p) in carPoints)
            {
                Guid driverId = _te2driver[te];
                int carClass = _te2carClass[te];

                // Class A5
                if (carClass == 5)
                {
                    _driver2carA5Points.Inc(driverId, p);
                    int place = _pointsProvider.GetPlaceFromA5CarPoints(p);
                    if (place <= 5)
                    {
                        _driver2carA5PointsTop5Count.Inc(driverId, 1);
                    }
                    if (place == 1)
                    {
                        _driver2carA5PointsTop1Count.Inc(driverId, 1);
                    }
                }
                // Class A6
                if (carClass == 6)
                {
                    _driver2carA6Points.Inc(driverId, p);
                    int place = _pointsProvider.GetPlaceFromA6CarPoints(p);
                    if (place <= 5)
                    {
                        _driver2carA6PointsTop5Count.Inc(driverId, 1);
                    }
                    if (place == 1)
                    {
                        _driver2carA6PointsTop1Count.Inc(driverId, 1);
                    }
                }
                // Class A7
                if (carClass == 7)
                {
                    _driver2carA7Points.Inc(driverId, p);
                    int place = _pointsProvider.GetPlaceFromA7CarPoints(p);
                    if (place <= 5)
                    {
                        _driver2carA7PointsTop5Count.Inc(driverId, 1);
                    }
                    if (place == 1)
                    {
                        _driver2carA7PointsTop1Count.Inc(driverId, 1);
                    }
                }
                // Class A8
                if (carClass == 8)
                {
                    _driver2carA8Points.Inc(driverId, p);
                    int place = _pointsProvider.GetPlaceFromA8CarPoints(p);
                    if (place <= 5)
                    {
                        _driver2carA8PointsTop5Count.Inc(driverId, 1);
                    }
                    if (place == 1)
                    {
                        _driver2carA8PointsTop1Count.Inc(driverId, 1);
                    }
                }
                // Bonus cars
                if (carClass == 4) // TODO bonu car
                {
                    _driver2bonusCarPoints.Inc(driverId, p);
                    int place = _pointsProvider.GetPlaceFromBonusCarPoints(p);
                    if (place <= 5)
                    {
                        _driver2bonusCarPointsTop5Count.Inc(driverId, 1);
                    }
                    if (place == 1)
                    {
                        _driver2bonusCarPointsTop1Count.Inc(driverId, 1);
                    }
                }
            }
        }

        // ----- ----- ----- BONUS POINTS
        var bonusPoints = await _bonusPointsService.GetAll();
        foreach (var bp in bonusPoints)
        {
            _driver2BonusPoints.Inc(bp.DriverId, bp.Points);
        }

        // ----- ----- ----- SUM
        Dictionary<Guid, int> _driver2totalPoints = [];
        foreach ((Guid driverId, int gp) in _driver2generalPoints)
        {
            _driver2totalPoints.Inc(driverId, gp);
        }
        foreach ((Guid driverId, int cp) in _driver2carA5Points)
        {
            _driver2totalPoints.Inc(driverId, cp);
        }
        foreach ((Guid driverId, int cp) in _driver2carA6Points)
        {
            _driver2totalPoints.Inc(driverId, cp);
        }
        foreach ((Guid driverId, int cp) in _driver2carA7Points)
        {
            _driver2totalPoints.Inc(driverId, cp);
        }
        foreach ((Guid driverId, int cp) in _driver2carA8Points)
        {
            _driver2totalPoints.Inc(driverId, cp);
        }
        foreach ((Guid driverId, int cp) in _driver2bonusCarPoints)
        {
            _driver2totalPoints.Inc(driverId, cp);
        }
        foreach ((Guid driverId, int bp) in _driver2BonusPoints)
        {
            _driver2totalPoints.Inc(driverId, bp);
        }

        // ----- ----- ----- SAVE DATA
        RankingSnapshot snapshot = new()
        {
            Id = Guid.NewGuid(),
            Date = DateTime.Now,
        };

        var entries = _driver2totalPoints.OrderByDescending(x => x.Value).Select((x, i) => new RankingEntry()
        {
            Place = i + 1,
            DriverId = x.Key,

            GeneralPoints = _driver2generalPoints.Get(x.Key),
            GeneralTop30Count = _driver2generalTop30Count.Get(x.Key),
            GeneralTop10Count = _driver2generalTop10Count.Get(x.Key),
            GeneralTop3Count = _driver2generalTop3Count.Get(x.Key),
            GeneralTop1Count = _driver2generalTop1Count.Get(x.Key),

            CarA5Points = _driver2carA5Points.Get(x.Key),
            CarA5PointsTop5Count = _driver2carA5PointsTop5Count.Get(x.Key),
            CarA5PointsTop1Count = _driver2carA5PointsTop1Count.Get(x.Key),

            CarA6Points = _driver2carA6Points.Get(x.Key),
            CarA6PointsTop5Count = _driver2carA6PointsTop5Count.Get(x.Key),
            CarA6PointsTop1Count = _driver2carA6PointsTop1Count.Get(x.Key),

            CarA7Points = _driver2carA7Points.Get(x.Key),
            CarA7PointsTop5Count = _driver2carA7PointsTop5Count.Get(x.Key),
            CarA7PointsTop1Count = _driver2carA7PointsTop1Count.Get(x.Key),

            CarA8Points = _driver2carA8Points.Get(x.Key),
            CarA8PointsTop5Count = _driver2carA8PointsTop5Count.Get(x.Key),
            CarA8PointsTop1Count = _driver2carA8PointsTop1Count.Get(x.Key),

            CarBonusPoints = _driver2bonusCarPoints.Get(x.Key),
            CarBonusPointsTop5Count = _driver2bonusCarPointsTop5Count.Get(x.Key),
            CarBonusPointsTop1Count = _driver2bonusCarPointsTop1Count.Get(x.Key),

            BonusPoints = _driver2BonusPoints.Get(x.Key),
        }).ToList();

        snapshot.Entries.AddRange(entries);

        return snapshot;
    }

    public async Task DoRankingSnapshot()
    {
        try
        {
            var current = await CalculateCurrentRankingSnapshot();
            await _rankingRepository.Create(current);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

public static class Extensions
{
    public static void Inc(this Dictionary<Guid, int> dict, Guid id, int value)
    {
        if (dict.ContainsKey(id))
        {
            dict[id] += value;
        }
        else
        {
            dict[id] = value;
        }
    }
    public static int Get(this Dictionary<Guid, int> dict, Guid id) =>
        dict.TryGetValue(id, out int value) ? value : 0;
}
