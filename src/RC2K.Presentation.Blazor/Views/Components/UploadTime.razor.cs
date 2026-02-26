using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using RC2K.DomainModel;
using RC2K.Presentation.Blazor.Views.Dialogs;

namespace RC2K.Presentation.Blazor.Views.Components;

public partial class UploadTime
{
    private ErrorBoundary? _errorBoundaryRef;
    private CarSelector _carSelectorRef = default!;

    [Parameter]
    public int StageId { get; set; }

    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; set; }

    protected override void OnParametersSet()
    {
        _errorBoundaryRef?.Recover();
    }

    private List<Tuple<ProofType, string>> _proofItems = new() { Tuple.Create<ProofType, string>(ProofType.Unknown, "") };
    private List<Proof> Proofs =>
      _proofItems.Where(x => !string.IsNullOrEmpty(x.Item2))
                 .Select(x => new Proof() { Type = x.Item1, Url = x.Item2 })
                 .ToList();

    private void OnValueChanged(int index, string newValue)
    {
        ProofType currentProofType = _proofItems[index].Item1;
        _proofItems[index] = Tuple.Create<ProofType, string>(currentProofType, newValue);

        if (index == _proofItems.Count - 1 && !string.IsNullOrWhiteSpace(newValue))
        {
            _proofItems.Add(Tuple.Create(ProofType.Unknown, ""));
        }
    }

    private void RemoveRow(int index)
    {
        if (_proofItems.Count > 1)
        {
            _proofItems.RemoveAt(index);
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }
        _carSelectorRef.OnSelectedCarChanged += (_, c) =>
        {
            UploadTimeModel.Car = c;
        };
    }

    protected override async Task OnInitializedAsync()
    {
        UploadTimeModel.DriverName = await UserService.GetCurrentUserName();
    }

    UploadTimeModelType UploadTimeModel = new();

    public class UploadTimeModelType
    {
        public int Min { get; set; }
        public int Sec { get; set; }
        public int Cc { get; set; }
        public Car? Car { get; set; }

        public string DriverName { get; set; } = string.Empty;
        public string Label { get; set; } = "TA";
    }

    private async Task<bool> IsRobot()
    {
        return await IsRobotHelper.IsRobot(JSRuntime, CaptchaVerifier, (msg) => MessageHelper.ShowError(msg));
    }

    private async Task UploadClick()
    {
        if (await IsRobot())
        {
            return;
        }
        List<string> validationResult = DoValidation();
        if (validationResult.Count > 0)
        {
            MessageHelper.ShowError(string.Join(" ", validationResult));
            return;
        }

        var driver = await DriverService.GetByName(UploadTimeModel.DriverName);
        if (driver is null)
        {
            await HandleNewDriverUploadTime();
        }
        else if (!driver.Known)
        {
            await HandleExistingDriverUploadTime(driver.Id);
        }
        else
        {
            await HandleUserUploadTime(driver.Id);
        }
    }

    private List<string> DoValidation()
    {
        List<string> errors = [];
        if (UploadTimeModel.Car is null)
        {
            errors.Add("You must select a car.");
        }
        if (UploadTimeModel.Min == 0 &&
            UploadTimeModel.Sec == 0 &&
            UploadTimeModel.Cc == 0)
        {
            errors.Add("You need to set a time!");
        }
        if (string.IsNullOrWhiteSpace(UploadTimeModel.DriverName))
        {
            errors.Add("You need to put your name.");
        }
        if (Proofs.Count == 0)
        {
            errors.Add("You need to add proofs! Screen with stage, car and time is required. " +
                "Otherwise your time will be rejected by verifiers. Upload your screen to Google Driver " +
                "or other storage and put url here.");
        }
        return errors;
    }

    private async Task HandleNewDriverUploadTime()
    {
        if (!await DialogHelper.ShowYesNoDialog("Driver creation",
            "There is no driver with this name yet. If you continue, you will get a unique pass-code for this driver, " +
            "that you will have to enter every time when you will upload another time entry using your nickname. " +
            "Do you want to continue?", yes: "Yes", no: "No"))
        {
            return;
        }

        string? nat = await DialogHelper.ShowDialogAndGetResult<NationalitySelectorDialog, string>("Select nationality");
        var result = await DriverService.CreateAnonymous(UploadTimeModel.DriverName, nat);
        if (!result.Success)
        {
            MessageHelper.ShowError(result.Message!);
            return;
        }

        Driver newDriver = result.Payload!;
        await DialogHelper.ShowMessageBox("Your pass code",
            $"Driver instance has been created, your pass-code is: {newDriver.Key}. " +
            "Save it for latter! Otherwise you'll lose access to upload times with this nickname.");

        var uploadResult = await TimeEntryService.Upload(
            StageId,
            UploadTimeModel.Car!.Id,
            newDriver.Id,
            UploadTimeModel.Min,
            UploadTimeModel.Sec,
            UploadTimeModel.Cc,
            Proofs,
            UploadTimeModel.Label,
            newDriver.Key!
            );

        if (uploadResult.Success)
        {
            ShowUploadedMessage();
            MudDialog.Close(1); // refresh
        }
        else
        {
            ShowUploadedFailureMessage(uploadResult.Message);
        }
    }

    private async Task HandleExistingDriverUploadTime(Guid driverId)
    {
        string? driverCode = await DialogHelper.ShowDialogAndGetResult<GetDriverPassCodeDialog, string>("Get driver pass code");
        if (driverCode is null)
        {
            return;
        }

        var uploadResult = await TimeEntryService.Upload(
            StageId,
            UploadTimeModel.Car!.Id,
            driverId,
            UploadTimeModel.Min,
            UploadTimeModel.Sec,
            UploadTimeModel.Cc,
            Proofs,
            UploadTimeModel.Label,
            driverCode
        );

        if (uploadResult.Success)
        {
            ShowUploadedMessage();
            MudDialog.Close(1); // refresh
        }
        else
        {
            ShowUploadedFailureMessage(uploadResult.Message);
        }
    }

    private async Task HandleUserUploadTime(Guid driverId)
    {
        var uploadResult = await TimeEntryService.Upload(
            StageId,
            UploadTimeModel.Car!.Id,
            driverId,
            UploadTimeModel.Min,
            UploadTimeModel.Sec,
            UploadTimeModel.Cc,
            Proofs,
            UploadTimeModel.Label
        );

        if (uploadResult.Success)
        {
            ShowUploadedMessage();
            MudDialog.Close(1); // refresh
        }
        else
        {
            ShowUploadedFailureMessage(uploadResult.Message);
        }
    }

    private void ShowUploadedMessage()
    {
        MessageHelper.ShowSuccess("Uploaded.");
    }

    private void ShowUploadedFailureMessage(string? msg)
    {
        MessageHelper.ShowError($"Upload failed. {msg}");
    }
}