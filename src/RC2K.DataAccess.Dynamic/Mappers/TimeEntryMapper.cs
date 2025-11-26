using RC2K.DataAccess.Dynamic.Models;
using static RC2K.Utils.Utils;
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
            Time = CentisecondsToTimeOnly(model.Time),
            UploadTime = StringToDateTime(model.UploadTime),
            VerifyInfoId = model.VerifyInfoId,
            Labels = model.Labels,
        };

        if (model.Proofs is not null)
        {
            timeEntry.Proofs.AddRange(
                model.Proofs.Select(DeserializeProof));
        }

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
            Time = TimeOnlyToCentiseconds(timeEntry.Time),
            UploadTime = DateTimeToString(timeEntry.UploadTime),
            VerifyInfoId = timeEntry.VerifyInfoId,
            Labels = timeEntry.Labels
        };

        if (timeEntry.Proofs.Any())
        {
            model.Proofs = timeEntry.Proofs.Select(SerializeProof).ToList();
        }

        return model;
    }
}
