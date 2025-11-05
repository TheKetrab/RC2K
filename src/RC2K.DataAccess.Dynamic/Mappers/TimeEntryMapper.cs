using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class TimeEntryMapper : IModelMapper<TimeEntry, TimeEntryModel>
{
    public TimeEntry ToDomainModel(TimeEntryModel model)
    {
        TimeEntry timeEntry = new()
        {
            Id = model.Id,
            CarId = model.CarId,
            DriverId = model.DriverId,
            StageId = model.StageId,
            Time = Utils.CentisecondsToTimeOnly(model.Time),
            UploadTime = Utils.StringToDateTime(model.UploadTime),
            VerifyInfoId = model.VerifyInfoId,
        };

        return timeEntry;
    }


    public TimeEntryModel ToCosmosModel(TimeEntry timeEntry)
    {
        TimeEntryModel model = new()
        {
            Id = timeEntry.Id,
            CarId = timeEntry.CarId,
            DriverId = timeEntry.DriverId,
            StageId = timeEntry.StageId,
            Time = Utils.TimeOnlyToCentiseconds(timeEntry.Time),
            UploadTime = Utils.DateTimeToString(timeEntry.UploadTime),
            VerifyInfoId = timeEntry.VerifyInfoId,
        };

        return model;
    }
}
