using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using HstTimeEntry = RC2K.Parser.Models.Hst.TimeEntry;

namespace RC2K.Presentation.Blazor;

public class HstUploadManager : IHstUploadManager
{
    private readonly ITimeEntryService _timeEntryService;
    private readonly IUserService _userService;
    private readonly IDriverService _driverService;
    private readonly IStageService _stageService;
    private readonly ICarService _carService;

    public HstUploadManager(
        ITimeEntryService timeEntryService,
        IUserService userService, 
        IDriverService driverService,
        IStageService stageService,
        ICarService carService)
    {
        _timeEntryService = timeEntryService;
        _userService = userService;
        _driverService = driverService;
        _stageService = stageService;
        _carService = carService;
    }

    public async Task<Result<List<(int,string,string)>>> UploadMany(IEnumerable<HstTimeEntry> hstTimeEntries, IProgress<int>? progress = null)
    {
        string userName = await _userService.GetCurrentUserName();
        var driver = await _driverService.GetByName(userName);
        if (driver is null)
        {
            return new Result<List<(int, string, string)>> { Success = false, Message = "No access" };
        }

        int errorNo = 0;
        List<(int, string, string)> errors = [];

        int total = hstTimeEntries.Count();
        foreach (var x in hstTimeEntries.Select((x,i) => new { hstTimeEntry = x, i = i }))
        {
            int percent = (int)Math.Ceiling((float)x.i * 100 / (float)total);
            progress?.Report(percent);

            int stageCode = x.hstTimeEntry.Parent.StageCode;
            Direction direction = x.hstTimeEntry.Parent.IsArcade ? Direction.Arcade : Direction.Simulation;

            Stage stage = (await _stageService.GetByCode(stageCode, direction))!;
            int stageId = stage.Id;

            TimeEntry te = new TimeEntry()
            {
                Id = Guid.NewGuid(),
                CarId = x.hstTimeEntry.Car + 1, // in RC2K Hub cars ids starts from 1
                DriverId = driver.Id,
                Driver = driver,
                StageId = stageId,
                Time = Utils.Utils.CentisecondsToTimeOnly(x.hstTimeEntry.Centiseconds),
                UploadTime = DateTime.Now,
                Labels = GetLabels(x.hstTimeEntry),
            };

            try
            {
                var res = await _timeEntryService.Upload(te);
                if (!res.Success)
                {
                    errors.Add((++errorNo, $"{stage.StageData!.Name} | {stage.Direction} | car={te.CarId} | time={te.Time.ToString("m:ss.ff")}", res.Message!));
                }
            }
            catch (Exception ex)
            {
                errors.Add((++errorNo, $"{stage.StageData!.Name} | {stage.Direction} | car={te.CarId} | time={te.Time}", ex.Message));
            }
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
                labels += _carService.IsA8(hstTimeEntry.Car) ? ",A8" : ",BRC";
            }
        }
        else
        {
            labels += ",TA";
        }
        return labels;
    }
}