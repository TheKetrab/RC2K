using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using HstTimeEntry = RC2K.Parser.Models.Hst.TimeEntry;

namespace RC2K.Presentation.Blazor;

public class HstUploadManager(
    ITimeEntryService timeEntryService,
    IUserService userService,
    IDriverService driverService,
    IStageService stageService,
    ICarService carService)
    : IHstUploadManager
{
    public async Task<Result<List<(int,string,string)>>> UploadMany(
        IEnumerable<HstTimeEntry> hstTimeEntries, IProgress<int>? progress = null)
    {
        string userName = await userService.GetCurrentUserName();
        var driver = await driverService.GetByName(userName);
        if (driver is null)
        {
            return new Result<List<(int, string, string)>> { Success = false, Message = "No access" };
        }

        int errorNo = 0;
        List<(int, string, string)> errors = [];

        var bests = await timeEntryService.GetBestTimesForDriver(driver.Id);

        bool anyUploaded = false;
        int total = hstTimeEntries.Count();
        foreach (var x in hstTimeEntries.Select((x,i) => new { hstTimeEntry = x, i }))
        {
            int percent = (int)Math.Ceiling((float)x.i * 100 / (float)total);
            progress?.Report(percent);

            int stageCode = x.hstTimeEntry.Parent.StageCode;
            Direction direction = x.hstTimeEntry.Parent.IsArcade ? Direction.Arcade : Direction.Simulation;

            Stage stage = (await stageService.GetByCode(stageCode, direction))!;
            int stageId = stage.Id;

            int carId = x.hstTimeEntry.Car + 1; // in RC2K Hub cars ids starts from 1
            int time = x.hstTimeEntry.Centiseconds;

            if (bests.TryGetValue((stageId, carId), out long best) && best <= time)
            {
                continue;
            }

            TimeEntry te = new()
            {
                Id = Guid.NewGuid(),
                CarId = carId, 
                DriverId = driver.Id,
                Driver = driver,
                StageId = stageId,
                Time = Utils.Utils.CentisecondsToTimeOnly(time),
                UploadTime = DateTime.Now,
                Labels = GetLabels(x.hstTimeEntry),
            };

            try
            {
                var res = await timeEntryService.Upload(te);
                if (!res.Success)
                {
                    errors.Add((++errorNo, $"{stage.StageData!.Name} | {stage.Direction} | car={te.CarId} | time={te.Time.ToString("m:ss.ff")}", res.Message!));
                }
                else
                {
                    anyUploaded = true;
                }
            }
            catch (Exception ex)
            {
                errors.Add((++errorNo, $"{stage.StageData!.Name} | {stage.Direction} | car={te.CarId} | time={te.Time}", ex.Message));
            }
        }

        if (!anyUploaded && errors.Count == 0)
        {
            return new Result<List<(int, string, string)>> { Success = true, Payload = [new(1, "---", "None time to upload")] };
        }

        return new Result<List<(int, string, string)>>
        {
            Success = true,
            Payload = errors
        };
    }

    private string GetLabels(HstTimeEntry hstTimeEntry)
    {
        string labels = "HST";
        if (hstTimeEntry.Parent.IsNormal)
        {
            if (hstTimeEntry.Parent.IsArcade)
            {
                labels += ",Arcade";
            }
            else
            {
                labels += carService.IsA8InternalCarId(hstTimeEntry.Car) ? ",A8" : ",BRC";
            }
        }
        else
        {
            labels += ",TA";
        }
        return labels;
    }
}