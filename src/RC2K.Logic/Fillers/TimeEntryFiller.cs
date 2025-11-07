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
    public async Task FillRecursive(TimeEntry timeEntry, FillingContext context, IFillersBag fillers)
    {
        if (context.TimeEntries.ContainsKey(timeEntry.Id))
        {
            return;
        }
        context.TimeEntries.Add(timeEntry.Id, timeEntry);

        timeEntry.Stage = await stageRepository.GetById(timeEntry.StageId);
        timeEntry.Car = await carRepository.GetById(timeEntry.CarId);

        await FillDriver(timeEntry, context, fillers);
        await FillVerifyInfo(timeEntry, context, fillers);
    }

    private async Task FillDriver(TimeEntry timeEntry, FillingContext context, IFillersBag fillers)
    {
        if (context.Drivers.TryGetValue(timeEntry.DriverId, out Driver? driver))
        {
            timeEntry.Driver = driver;
        }
        else
        {
            timeEntry.Driver = await driverRepository.GetById(timeEntry.DriverId) ?? throw new KeyNotFoundException();
            await fillers.DriverFiller.FillRecursive(timeEntry.Driver, context, fillers);
        }
    }

    private async Task FillVerifyInfo(TimeEntry timeEntry, FillingContext context, IFillersBag fillers)
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
            timeEntry.VerifyInfo = await verifyInfoRepository.GetById(timeEntry.VerifyInfoId.Value) ?? throw new KeyNotFoundException();
            await fillers.VerifyInfoFiller.FillRecursive(timeEntry.VerifyInfo, context, fillers);
        }
    }

}
