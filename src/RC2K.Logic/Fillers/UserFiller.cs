using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class UserFiller(IDriverRepository driverRepository)
    : IUserFiller
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task FillRecursive(User user, FillingContext context, IFillersBag fillers)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (context.Users.ContainsKey(user.Id))
            {
                return;
            }
            context.Users.Add(user.Id, user);

            await FillDriver(user, context, fillers);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task FillDriver(User user, FillingContext context, IFillersBag fillers)
    {
        if (context.Drivers.TryGetValue(user.DriverId, out Driver? driver))
        {
            user.Driver = driver;
        }
        else
        {
            user.Driver = (await driverRepository.GetById(user.DriverId)) ?? throw new KeyNotFoundException();
            await fillers.DriverFiller.FillRecursive(user.Driver, context, fillers);
        }
    }
}
