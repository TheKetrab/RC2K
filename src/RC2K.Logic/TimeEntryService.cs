using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Dtos;
using RC2K.Logic.Interfaces.Fillers;
using SerilogTimings;

namespace RC2K.Logic;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IVerifyInfoRepository _verifyInfoRepository;
    private readonly IPointsProvider _pointsProvider;
    private readonly IFillersBag _fillers;
    private readonly ILogger<TimeEntryService> _logger;

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

        _timeEntryRepository.RequestUnitsHandler += (s, e) =>
        {
            _logger.LogInformation($"RU: {e.Item2} for query: {e.Item1}");
        };
    }

    public async Task<List<TimeEntry>> Get(int stageId, int? carId = null)
    {
        var timeEntries = carId is not null
            ? await _timeEntryRepository.GetByStageIdAndCarId(stageId, carId.Value)
            : await _timeEntryRepository.GetByStageId(stageId);

        await timeEntries.FillFullData(_fillers.TimeEntryFiller, _fillers);

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
                await _timeEntryRepository.Delete(timeEntry.Id.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete time entry {timeEntry.Id}");
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
            throw new Exception($"TimeEntries with ids [{ids}] are already verified.");
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
                _logger.LogError(ex, $"Failed to update {timeEntry.Id}");
            }
        }
    }

    public async Task Upload(
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
    }

    public async Task Upload(TimeEntry timeEntry)
    {
        var identicEntries = await _timeEntryRepository.GetByStageIdAndCarIdAndDriverIdAndTime(
            timeEntry.StageId, timeEntry.CarId, timeEntry.DriverId, timeEntry.Time);

        if (identicEntries.Any())
        {
            // user can put identical times ONLY if it has different label
            if (string.IsNullOrEmpty(timeEntry.Labels) ||
                identicEntries.Any(x => x.Labels == timeEntry.Labels))
            {
                throw new Exception($"There already exists TimeEntry for: Stage={timeEntry.StageId}; Car={timeEntry.CarId}; Driver={timeEntry.DriverId}, Time={timeEntry.Time}");
            }
        }

        await _timeEntryRepository.Create(timeEntry);
    }




    public async Task<TimeEntriesCollectionInfo> CalculateTimeEntriesWithPoints(int stageId, int maximum = -1)
    {
        using (Operation.Time("Fetch TimeEntryList | StageId = {stageId} | Maximum = {maximum}", stageId, maximum))
        {
            List<TimeEntry> timeEntries = await this.Get(stageId);
            TimeEntry best = timeEntries.OrderBy(x => x.Time).FirstOrDefault();

            Dictionary<Guid, int> places = _pointsProvider.CalculatePlace(timeEntries);
            Dictionary<Guid, int> placesByCar = _pointsProvider.CalculatePlaceByCar(timeEntries);

            List<TimeEntry> orderedTimeEntries = timeEntries.OrderBy(x => x.Time).ToList();
            Dictionary<Guid, int> generalPoints = _pointsProvider.CalculateGeneralStagePoints(timeEntries);
            Dictionary<Guid, int> carPoints = _pointsProvider.CalculateCarStagePoints(timeEntries);

            if (maximum > 0)
            {
                // cut off to get only top 'maximum'
                orderedTimeEntries = orderedTimeEntries.Take(maximum).ToList();
                HashSet<Guid> existingTimeEntries = orderedTimeEntries.Select(x => x.Id).ToHashSet();
                generalPoints = generalPoints.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
                carPoints = carPoints.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
                places = places.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
                placesByCar = placesByCar.Where(x => existingTimeEntries.Contains(x.Key)).ToDictionary();
            }

            return new TimeEntriesCollectionInfo(
                orderedTimeEntries,
                best,
                generalPoints,
                carPoints,
                places,
                placesByCar);
        }
    }


}
