using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class RankingFiller(IDriverRepository driverRepository)
    : IRankingFiller
{
    public async Task FillRecursive(RankingSnapshot ranking, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.Rankings.ContainsKey(ranking.Id))
        {
            return;
        }
        context.Rankings.Add(ranking.Id, ranking);

        foreach (var entry in ranking.Entries)
        {
            entry.Driver = await driverRepository.GetById(entry.DriverId, ct);
            await FillDriver(entry, context, fillers, ct);
        }
    }

    private async Task FillDriver(RankingEntry entry, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.Drivers.TryGetValue(entry.DriverId, out Driver? driver))
        {
            entry.Driver = driver;
        }
        else
        {
            entry.Driver = (await driverRepository.GetById(entry.DriverId, ct)) ?? throw new KeyNotFoundException();
            await fillers.DriverFiller.FillRecursive(entry.Driver, context, fillers, ct);
        }
    }

}
