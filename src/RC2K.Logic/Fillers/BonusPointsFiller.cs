using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class BonusPointsFiller(IDriverRepository driverRepository)
    : IBonusPointsFiller
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task FillRecursive(BonusPoints bonusPoints, FillingContext context, IFillersBag fillers)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (context.BonusPoints.ContainsKey(bonusPoints.Id))
            {
                return;
            }
            context.BonusPoints.Add(bonusPoints.Id, bonusPoints);

            await FillDriver(bonusPoints, context, fillers);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task FillDriver(BonusPoints bonusPoints, FillingContext context, IFillersBag fillers)
    {
        if (context.Drivers.TryGetValue(bonusPoints.DriverId, out Driver? driver))
        {
            bonusPoints.Driver = driver;
        }
        else
        {
            bonusPoints.Driver = (await driverRepository.GetById(bonusPoints.DriverId)) ?? throw new KeyNotFoundException();
            await fillers.DriverFiller.FillRecursive(bonusPoints.Driver, context, fillers);
        }
    }
}
