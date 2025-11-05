using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class UserFiller(IDriverRepository driverRepository)
    : IUserFiller
{
    public async Task FillRecursive(User user, FillingContext context, IFillersBag fillers)
    {
        if (context.Users.ContainsKey(user.Id))
        {
            return;
        }
        context.Users.Add(user.Id, user);

        await FillDriver(user, context, fillers);
    }

    private async Task FillDriver(User user, FillingContext context, IFillersBag fillers)
    {
        if (context.Drivers.TryGetValue(user.DriverId, out Driver? driver))
        {
            user.Driver = driver;
        }
        else
        {
            user.Driver = await driverRepository.GetById(user.DriverId) ?? throw new Exception();
            await fillers.DriverFiller.FillRecursive(user.Driver, context, fillers);
        }
    }
}
