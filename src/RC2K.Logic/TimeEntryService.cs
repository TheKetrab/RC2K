using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public class TimeEntryService : ITimeEntryService
{
    private readonly ITimeEntryRepository _timeEntryRepository;
    private readonly IFillersBag _fillers;

    public TimeEntryService(ITimeEntryRepository timeEntryRepository,
                            IFillersBag fillers)
    {
        _timeEntryRepository = timeEntryRepository;
        _fillers = fillers;
    }

    public async Task<List<TimeEntry>> Get(int stageId, int? carId = null)
    {    
        var timeEntries = carId is not null
            ? await _timeEntryRepository.GetByStageIdAndCarId(stageId, carId.Value)
            : await _timeEntryRepository.GetByStageId(stageId);

        // TODO handle concurency
        FillingContext context = new();
        Task[] tasks = timeEntries.Select(x => _fillers.TimeEntryFiller.FillRecursive(x, context, _fillers)).ToArray();
        await Task.WhenAll(tasks);

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
            throw new Exception();
        }

        await _timeEntryRepository.Create(timeEntry);
    }
}
