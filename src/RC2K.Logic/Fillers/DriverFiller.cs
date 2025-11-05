using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class DriverFiller(IUserRepository userRepository)
    : IDriverFiller
{
    public async Task FillRecursive(Driver driver, FillingContext context, IFillersBag fillers)
    {
        if (context.Drivers.ContainsKey(driver.Id))
        {
            return;
        }
        context.Drivers.Add(driver.Id, driver);

        await FillUser(driver, context, fillers);
    }

    private async Task FillUser(Driver driver, FillingContext context, IFillersBag fillers)
    {
        if (driver.UserId is null)
        {
            return;
        }

        if (context.Users.TryGetValue(driver.UserId.Value, out User? user))
        {
            driver.User = user;
        }
        else
        {
            driver.User = await userRepository.GetById(driver.UserId.Value) ?? throw new Exception();
            await fillers.UserFiller.FillRecursive(driver.User, context, fillers);
        }
    }
}
