using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class RankingFiller(IDriverRepository driverRepository)
    : IRankingFiller
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task FillRecursive(RankingSnapshot ranking, FillingContext context, IFillersBag fillers)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (context.Rankings.ContainsKey(ranking.Id))
            {
                return;
            }
            context.Rankings.Add(ranking.Id, ranking);

            foreach (var entry in ranking.Entries)
            {
                entry.Driver = await driverRepository.GetById(entry.DriverId);
                await FillDriver(entry, context, fillers);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task FillDriver(RankingEntry entry, FillingContext context, IFillersBag fillers)
    {
        if (context.Drivers.TryGetValue(entry.DriverId, out Driver? driver))
        {
            entry.Driver = driver;
        }
        else
        {
            entry.Driver = (await driverRepository.GetById(entry.DriverId)) ?? throw new KeyNotFoundException();
            await fillers.DriverFiller.FillRecursive(entry.Driver, context, fillers);
        }
    }

}
