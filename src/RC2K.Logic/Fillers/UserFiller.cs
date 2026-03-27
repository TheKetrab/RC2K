using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class UserFiller(IDriverRepository driverRepository)
    : IUserFiller
{
    public async Task FillRecursive(User user, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.Users.ContainsKey(user.Id))
        {
            return;
        }
        context.Users.Add(user.Id, user);

        await FillDriver(user, context, fillers, ct);
    }

    private async Task FillDriver(User user, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.Drivers.TryGetValue(user.DriverId, out Driver? driver))
        {
            user.Driver = driver;
        }
        else
        {
            user.Driver = (await driverRepository.GetById(user.DriverId, ct)) ?? throw new KeyNotFoundException();
            await fillers.DriverFiller.FillRecursive(user.Driver, context, fillers, ct);
        }
    }
}
