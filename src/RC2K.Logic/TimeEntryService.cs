using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Dtos;
using RC2K.Logic.Interfaces.Fillers;
using Microsoft.Extensions.Caching.Memory;
using SerilogTimings;

namespace RC2K.Logic;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IVerifyInfoRepository _verifyInfoRepository;
    private readonly IPointsProvider _pointsProvider;
    private readonly IFillersBag _fillers;
    private readonly ILogger<TimeEntryService> _logger;
    private readonly TimeEntryCache _cache;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository,
                            IVerifyInfoRepository verifyInfoRepository,
                            IPointsProvider pointsProvider,
                            IFillersBag fillers,
                            ILogger<TimeEntryService> logger)
    {
        _logger = logger;
        _timeEntryRepository = timeEntryRepository;
        _verifyInfoRepository = verifyInfoRepository;
        _pointsProvider = pointsProvider;
        _fillers = fillers;
        _cache = new();

        _timeEntryRepository.RequestUnitsHandler += (s, e) =>
        {
            _logger.LogInformation("RU: {RU} for query: {Query}", e.Item2, e.Item1);
        };
    }

    public async Task<List<TimeEntry>> Get(int stageId, int? carId = null, CancellationToken ct = default)
    {
        List<TimeEntry>? cacheEntry = _cache.GetTimeEntries(stageId, carId);
        if (cacheEntry is not null)
        {
            return cacheEntry;
        }

        var timeEntries = carId is not null
            ? await _timeEntryRepository.GetByStageIdAndCarId(stageId, carId.Value, ct)
            : await _timeEntryRepository.GetByStageId(stageId, ct);

        await timeEntries.FillFullData(_fillers.TimeEntryFiller, _fillers, ct);

        _cache.SetTimeEntries(stageId, carId, timeEntries);

        return timeEntries;
    }

    public async Task<List<TimeEntry>> GetAllNotVerified()
    {
        var timeEntries = await _timeEntryRepository.GetAllNotVerified();

        await timeEntries.FillFullData(_fillers.TimeEntryFiller, _fillers);

        return timeEntries;
    }

    public async Task Delete(List<TimeEntry> timeEntries)
    {
        foreach (var timeEntry in timeEntries)
        {
            try
            {
                _cache.RemoveTimeEntries(timeEntry.StageId, null);
                _cache.RemoveTimeEntries(timeEntry.StageId, timeEntry.CarId);
                await _timeEntryRepository.Delete(timeEntry.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete time entry {Id}", timeEntry.Id);
            }
        }
    }

    public Task Verify(List<TimeEntry> timeEntries, string comment)
    {
        _logger.LogWarning("Call Verify with verifierId");
        return Task.CompletedTask;
    }

    public async Task Verify(List<TimeEntry> timeEntries, Guid verifierId, string comment)
    {
        if (timeEntries.Any(x => x.VerifyInfoId is not null))
        {
            string ids = string.Join(",",
                timeEntries.Where(x => x.VerifyInfoId is not null)
                           .Select(x => x.Id));
            throw new ArgumentException($"TimeEntries with ids [{ids}] are already verified.", nameof(ids));
        }

        VerifyInfo verifyInfo = new()
        {
            Id = Guid.NewGuid(),
            VerifierId = verifierId,
            Comment = comment,
            VerifyDate = DateTime.Now
        };

        await _verifyInfoRepository.Create(verifyInfo);

        timeEntries.ForEach(x => x.VerifyInfoId = verifyInfo.Id);

        foreach (var timeEntry in timeEntries)
        {
            try
            {
                await _timeEntryRepository.Update(timeEntry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update {Id}", timeEntry.Id);
            }
        }
    }

    public async Task<Result> Upload(
       int stageId, int carId, Guid driverId,
       int min, int sec, int cc,
       List<Proof> proofs, string? labels,
       string driverKey)
    {
        await Upload(stageId, carId, driverId, min, sec, cc, proofs, labels);
        return new Result() { Success = true };
    }

    public async Task<Result> Upload(
        int stageId, int carId, Guid driverId,
        int min, int sec, int cc,
        List<Proof> proofs, string? labels)
    {
        TimeEntry timeEntry = new()
        {
            Id = Guid.NewGuid(),
            CarId = carId,
            StageId = stageId,
            DriverId = driverId,
            Proofs = proofs,
            UploadTime = DateTime.UtcNow,
            Time = new TimeOnly(0, min, sec, cc * 10),
            Labels = labels
        };

        await Upload(timeEntry);
        return new Result() { Success = true };
    }

    public async Task<Result> Upload(TimeEntry timeEntry)
    {
        var identicEntries = await _timeEntryRepository.GetByStageIdAndCarIdAndDriverIdAndTime(
            timeEntry.StageId, timeEntry.CarId, timeEntry.DriverId, timeEntry.Time);

        // user can put identical times ONLY if it has different label (but is not automatic = no HST)
        if (identicEntries.Count > 0 &&
            (string.IsNullOrWhiteSpace(timeEntry.Labels) || // has no labels
             identicEntries.Any(x => x.Labels == timeEntry.Labels) || // has the same labels
             timeEntry.Labels.Contains("HST"))) // is automatic
        {
            var best = identicEntries.OrderBy(x => x.Time).First();
            return new Result()
            {
                Success = false,
                Message = $"There already exists better or the same TimeEntry: Stage={timeEntry.StageId}; Car={timeEntry.CarId}; Driver={timeEntry.DriverId}, Time={best.Time:m:ss.ff}"
            };
        }

        _cache.RemoveTimeEntries(timeEntry.StageId, timeEntry.CarId);
        _cache.RemoveTimeEntries(timeEntry.StageId, null);
        await _timeEntryRepository.Create(timeEntry);
        return new Result() { Success = true };
    }

    private sealed record DriverWithPoints(Guid DriverId, int Points);
    private sealed record DriverWithClassAndPoints(Guid DriverId, int Class, int Points);

    private static Dictionary<Guid, int> GetDriverIdToCarPointsByClass(IEnumerable<DriverWithClassAndPoints> colCp, int @class)
    {
        return colCp
            .Where(x => x.Class == @class)
            .GroupBy(x => x.DriverId)
            .Select(g => new { DriverId = g.Key, Points = g.Sum(x => x.Points) })
            .ToDictionary(x => x.DriverId, x => x.Points);
    }

    private static PointsInfo CalculatePointsInfo(
        List<TimeEntry> timeEntries,
        Dictionary<Guid, int> generalPoints,
        Dictionary<Guid, int> carPoints)
    {
        Dictionary<Guid, TimeEntry> timeEntriesMap = timeEntries.ToDictionary(x => x.Id, x => x);

        var colGp = generalPoints.Join(timeEntriesMap,
            x => x.Key,
            y => y.Value.Id,
            (x, y) => new DriverWithPoints(y.Value.DriverId, x.Value));

        Dictionary<Guid, int> generalPointsByDriver = colGp.ToDictionary(x => x.DriverId, x => x.Points);

        var colCp = carPoints.Join(timeEntriesMap,
            x => x.Key,
            y => y.Value.Id,
            (x, y) => new DriverWithClassAndPoints(y.Value.DriverId, y.Value.Car!.Class, x.Value));

        Dictionary<Guid, int> carPointsA8ByDriver = GetDriverIdToCarPointsByClass(colCp, 8);
        Dictionary<Guid, int> carPointsA7ByDriver = GetDriverIdToCarPointsByClass(colCp, 7);
        Dictionary<Guid, int> carPointsA6ByDriver = GetDriverIdToCarPointsByClass(colCp, 6);
        Dictionary<Guid, int> carPointsA5ByDriver = GetDriverIdToCarPointsByClass(colCp, 5);
        Dictionary<Guid, int> carPointsBonusByDriver = GetDriverIdToCarPointsByClass(colCp, Car.BonusClass);

        Dictionary<Guid, int> totalPointsByDriver = new[] 
        { 
            generalPointsByDriver,
            carPointsA8ByDriver,
            carPointsA7ByDriver,
            carPointsA6ByDriver,
            carPointsA5ByDriver,
            carPointsBonusByDriver,
        }
        .SelectMany(d => d)
        .GroupBy(x => x.Key)
        .ToDictionary(g => g.Key, g => g.Sum(x => x.Value));

        int best = totalPointsByDriver.OrderByDescending(x => x.Value).FirstOrDefault().Value;

        return new PointsInfo(
            best,
            totalPointsByDriver,
            generalPointsByDriver,
            carPointsA8ByDriver,
            carPointsA7ByDriver,
            carPointsA6ByDriver,
            carPointsA5ByDriver,
            carPointsBonusByDriver
        );
    }

    public async Task<TimeEntriesCollectionInfo> CalculateTimeEntriesWithPoints(int stageId, int maximum = -1, CancellationToken ct = default)
    {
        if (_cache.GetCollectionInfo(stageId, maximum) is TimeEntriesCollectionInfo cacheEntry)
        {
            return cacheEntry;
        }

        using (Operation.Time("Fetch TimeEntryList | StageId = {stageId} | Maximum = {maximum}", stageId, maximum))
        {
            List<TimeEntry> timeEntries = await this.Get(stageId, ct: ct);
            TimeEntry? best = timeEntries.OrderBy(x => x.Time).FirstOrDefault();

            Dictionary<Guid, int> places = _pointsProvider.CalculatePlace(timeEntries);
            Dictionary<Guid, int> placesByCar = _pointsProvider.CalculatePlaceByCar(timeEntries);
            Dictionary<Guid, int> placesByClass = _pointsProvider.CalculatePlaceByClass(timeEntries);

            List<TimeEntry> orderedTimeEntries = timeEntries.OrderBy(x => x.Time).ToList();
            Dictionary<Guid, int> generalPoints = _pointsProvider.CalculateGeneralStagePoints(timeEntries);
            Dictionary<Guid, int> carPoints = _pointsProvider.CalculateCarStagePoints(timeEntries);

            PointsInfo pointsInfo = CalculatePointsInfo(timeEntries, generalPoints, carPoints);
            
            if (maximum > 0)
            {
                // cut off to get only top 'maximum' (cut off is NOT for points info on purpose)
                orderedTimeEntries = orderedTimeEntries.Take(maximum).ToList();
                HashSet<Guid> existingTimeEntries = orderedTimeEntries.Select(x => x.Id).ToHashSet();
                generalPoints = generalPoints.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
                carPoints = carPoints.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
                places = places.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
                placesByCar = placesByCar.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
                placesByClass = placesByClass.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
            }

            TimeEntriesCollectionInfo res = new(
                orderedTimeEntries,
                best,
                generalPoints,
                carPoints,
                places,
                placesByCar,
                placesByClass,
                pointsInfo);

            _cache.SetCollectionInfo(stageId, maximum, res);
            return res;
        }
    }

    public Task<Dictionary<(int stageId, int carId), long>> GetBestTimesForDriver(Guid driverId) =>
        _timeEntryRepository.GetBestTimesForDriver(driverId);

    private class TimeEntryCache
    {
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);
        private readonly MemoryCache _cache = new(new MemoryCacheOptions {});

        private static string GetTimeEntriesCacheKey(int stageId, int? carId) =>
            $"time-entries-{stageId}-{carId}";

        private static string GetCollectionInfoCacheKey(int stageId, int maximum) =>
            $"te-collection-info-{stageId}-max:{maximum}";

        public void SetTimeEntries(int stageId, int? carId, List<TimeEntry> timeEntries) =>
            _cache.Set(GetTimeEntriesCacheKey(stageId, carId), timeEntries, _cacheExpiration);

        public void SetCollectionInfo(int stageId, int maximum, TimeEntriesCollectionInfo collectionInfo) =>
            _cache.Set(GetCollectionInfoCacheKey(stageId, maximum), collectionInfo, _cacheExpiration);

        public void RemoveTimeEntries(int stageId, int? carId) =>
            _cache.Remove(GetTimeEntriesCacheKey(stageId, carId));

        public void RemoveCollectionInfo(int stageId, int maximum) =>
            _cache.Remove(GetCollectionInfoCacheKey(stageId, maximum));

        public List<TimeEntry>? GetTimeEntries(int stageId, int? carId) =>
            _cache.TryGetValue(GetTimeEntriesCacheKey(stageId, carId),
                out List<TimeEntry>? timeEntries) ? timeEntries : null;

        public TimeEntriesCollectionInfo? GetCollectionInfo(int stageId, int maximum) =>
            _cache.TryGetValue(GetCollectionInfoCacheKey(stageId, maximum),
                out TimeEntriesCollectionInfo? collectionInfo) ? collectionInfo : null;

    }
}
