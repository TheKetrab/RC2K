using Microsoft.AspNetCore.Components.Authorization;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Dtos;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Presentation.Blazor.AuthProxy;

public class AuthTimeEntryServiceProxy(
    AuthenticationStateProvider asp,
    TimeEntryService service,
    IDriverRepository driverRepository,
    IUserRepository userRepository,
    IFillersBag fillers) 
    : ITimeEntryService
{
    public async Task<TimeEntriesCollectionInfo> CalculateTimeEntriesWithPoints(int stageId, int maximum = -1, CancellationToken ct = default)
    {
        var auth = await asp.GetAuthenticationStateAsync();
        if (!Auth.TryAuthorize(auth))
        {
            if (maximum < 0 || maximum >= 20)
            {
                maximum = 20;
            }
        }

        var result = await service.CalculateTimeEntriesWithPoints(stageId, maximum, ct);
        return result;
    }

    public async Task Delete(List<TimeEntry> timeEntries)
    {
        var auth = await asp.GetAuthenticationStateAsync();

        if (timeEntries.Count == 1 && Auth.TryAuthorizeSelf(auth, timeEntries[0].Driver!.User?.Name ?? timeEntries[0].Driver!.Name!))
        {
            // OK
        }
        else
        {
            Auth.Authorize(auth, "admin");
        }

        await service.Delete(timeEntries);
    }

    public Task<List<TimeEntry>> Get(int stageId, int? carId = null, CancellationToken ct = default) =>
        service.Get(stageId, carId, ct);

    public Task<List<TimeEntry>> GetAllNotVerified() =>
        service.GetAllNotVerified();

    public Task<Dictionary<(int stageId, int carId), long>> GetBestTimesForDriver(Guid driverId) =>
        service.GetBestTimesForDriver(driverId);

    public async Task<Result> Upload(
        int stageId, int carId, Guid driverId,
        int min, int sec, int cc,
        List<Proof> proofs, string? labels)
    {
        Driver driver = await driverRepository.GetById(driverId, CancellationToken.None)
            ?? throw new ArgumentException();

        FillingContext context = new();
        await fillers.DriverFiller.FillRecursive(driver, context, fillers, CancellationToken.None);

        try
        {
            await AuthorizeSelf(driver);

            await service.Upload(
                stageId, carId, driverId,
                min, sec, cc,
                proofs, labels);

            return new Result() { Success = true };
        }
        catch (NotAuthorizedException ex)
        {
            return new Result { Success = false, Message = ex.Message };
        }
    }

    public async Task<Result> Upload(int stageId, int carId, Guid driverId, int min, int sec, int cc, List<Proof> proofs, string? labels, string driverKey)
    {
        Driver driver = await driverRepository.GetById(driverId, CancellationToken.None)
            ?? throw new ArgumentException(nameof(driverId));

        if (driver.Known)
        {
            throw new ArgumentException("For registered users use method without driver key");
        }

        if (driver.Key != driverKey)
        {
            return new Result() { Success = false, Message = "Invalid driver code" };
        }

        await service.Upload(
            stageId, carId, driverId,
            min, sec, cc,
            proofs, labels);

        return new Result() { Success = true };
    }

    public async Task<Result> Upload(TimeEntry timeEntry)
    {
        await AuthorizeSelf(timeEntry.Driver!);
        return await service.Upload(timeEntry);
    }

    public async Task Verify(List<TimeEntry> timeEntries, Guid verifierId, string comment)
    {
        var auth = await asp.GetAuthenticationStateAsync();
        Auth.Authorize(auth, "admin");

        await service.Verify(timeEntries, verifierId, comment);
    }

    public async Task Verify(List<TimeEntry> timeEntries, string comment)
    {
        var auth = await asp.GetAuthenticationStateAsync();
        Auth.Authorize(auth, "admin");

        string name = auth.User.Identity!.Name!;
        var user = await userRepository.GetByName(name);

        await service.Verify(timeEntries, user!.Id, comment);
    }

    private async Task AuthorizeSelf(Driver driver)
    {
        string name = driver.Known
            ? driver.User!.Name
            : driver.Name!;

        var auth = await asp.GetAuthenticationStateAsync();
        Auth.AuthorizeSelf(auth, name);
    }
}
