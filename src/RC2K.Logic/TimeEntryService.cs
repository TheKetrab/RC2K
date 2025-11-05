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
        stageId = 1;
                
        var timeEntries = carId is not null
            ? await _timeEntryRepository.GetByStageIdAndCarId(stageId, carId.Value)
            : await _timeEntryRepository.GetByStageId(stageId);

        // TODO handle concurency
        FillingContext context = new();
        Task[] tasks = timeEntries.Select(x => _fillers.TimeEntryFiller.FillRecursive(x, context, _fillers)).ToArray();
        await Task.WhenAll(tasks);
        return timeEntries;
    }

    public Task Upload(TimeEntry timeEntry)
    {
        throw new NotImplementedException();
    }
}
