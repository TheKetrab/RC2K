using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public class DriverService(
    IUserRepository userRepository,
    IDriverRepository driverRepository,
    IFillersBag fillers,
    IPasswordProvider passwordProvider)
    : IDriverService
{
    public async Task<Result<Driver>> CreateAnonymous(string name, string? nationality)
    {
        Driver driver = new()
        {
            Id = Guid.NewGuid(),
            Known = false,
            Name = name,
            Key = passwordProvider.GenerateTemporaryPassword(),
            Nationality = nationality
        };

        try
        {
            await driverRepository.Create(driver);
            return new Result<Driver>() { Success = true, Payload = driver };
        }
        catch (DriverExistsException)
        {
            return new Result<Driver>() { Success = false, Message = "Driver exists" };
        }
    }

    public async Task<Dictionary<Guid, string>> GetAllNames()
    {
        Dictionary<Guid, string> res = [];
        var drivers = await driverRepository.GetAll();
        await drivers.FillFullData(fillers.DriverFiller, fillers);

        foreach (var driver in drivers)
        {
            res[driver.Id] = driver.Known
                ? driver.User!.Name
                : driver.Name!;
        }

        return res;
    }

    public async Task<Driver?> GetById(Guid id, CancellationToken ct = default)
    {
        Driver? driver = await driverRepository.GetById(id, ct);
        if (driver is not null)
        {
            FillingContext context = new();
            await fillers.DriverFiller.FillRecursive(driver, context, fillers, ct);
        }
        return driver;
    }

    public async Task<Driver?> GetByName(string name)
    {
        User? user = await userRepository.GetByName(name);
        if (user is not null)
        {
            FillingContext context = new();
            await fillers.UserFiller.FillRecursive(user, context, fillers, CancellationToken.None);
            return user.Driver;
        }

        Driver? anonymousDriver = await driverRepository.GetByName(name);
        if (anonymousDriver is not null) 
        {
            FillingContext context = new();
            await fillers.DriverFiller.FillRecursive(anonymousDriver, context, fillers, CancellationToken.None);
            return anonymousDriver;
        }

        return null;
    }
}