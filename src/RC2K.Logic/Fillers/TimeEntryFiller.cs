using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class TimeEntryFiller(ICarRepository carRepository, 
                             IDriverRepository driverRepository,
                             IStageRepository stageRepository,                             
                             IVerifyInfoRepository verifyInfoRepository)
    : ITimeEntryFiller
{
    public async Task FillRecursive(TimeEntry timeEntry, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.TimeEntries.ContainsKey(timeEntry.Id))
        {
            return;
        }
        context.TimeEntries.Add(timeEntry.Id, timeEntry);

        timeEntry.Stage = await stageRepository.GetById(timeEntry.StageId);
        timeEntry.Car = await carRepository.GetById(timeEntry.CarId);

        await FillDriver(timeEntry, context, fillers, ct);
        await FillVerifyInfo(timeEntry, context, fillers, ct);
    }

    private async Task FillDriver(TimeEntry timeEntry, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.Drivers.TryGetValue(timeEntry.DriverId, out Driver? driver))
        {
            timeEntry.Driver = driver;
        }
        else
        {
            timeEntry.Driver = (await driverRepository.GetById(timeEntry.DriverId, ct)) ?? throw new KeyNotFoundException();
            await fillers.DriverFiller.FillRecursive(timeEntry.Driver, context, fillers, ct);
        }
    }

    private async Task FillVerifyInfo(TimeEntry timeEntry, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (timeEntry.VerifyInfoId is null)
        {
            return;
        }

        if (context.VerifyInfos.TryGetValue(timeEntry.VerifyInfoId.Value, out VerifyInfo? verifyInfo))
        {
            timeEntry.VerifyInfo = verifyInfo;
        }
        else
        {
            timeEntry.VerifyInfo = (await verifyInfoRepository.GetById(timeEntry.VerifyInfoId.Value, ct)) ?? throw new KeyNotFoundException();
            await fillers.VerifyInfoFiller.FillRecursive(timeEntry.VerifyInfo, context, fillers, ct);
        }
    }

}
