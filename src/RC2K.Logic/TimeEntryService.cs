using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IFillersBag _fillers;
    private readonly ILogger<TimeEntryService> _logger;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository,
                            IFillersBag fillers,
                            ILogger<TimeEntryService> logger)
    {
        _logger = logger;
        _timeEntryRepository = timeEntryRepository;
        _fillers = fillers;

        _timeEntryRepository.RequestUnitsHandler += (s, e) =>
        {
            _logger.LogInformation($"RU: {e.Item2}");
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
}
