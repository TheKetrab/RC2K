using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using RC2K.DomainModel;
using RC2K.Parser.Models.Repl;
using RC2K.Presentation.Blazor.ViewModels;
using RC2K.Presentation.Blazor.Views.Dialogs;


namespace RC2K.Presentation.Blazor.Views.Components;

public partial class UploadTime
{
    private sealed class ProofItem(ProofType type, string url)
    {
        public ProofType Type { get; set; } = type;
        public string Url { get; set; } = url;
    }

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

    private readonly List<ProofItem> _proofItems = new() { new(ProofType.Unknown, "") };
    private List<Proof> GetProofs() =>
      _proofItems.Where(x => !string.IsNullOrEmpty(x.Url))
                 .Select(x => new Proof() { Type = x.Type, Url = x.Url })
                 .ToList();

    private void OnValueChanged(int index, string newValue)
    {
        ProofType currentProofType = _proofItems[index].Type;
        _proofItems[index] = new ProofItem(currentProofType, newValue);

        if (index == _proofItems.Count - 1 && !string.IsNullOrWhiteSpace(newValue))
        {
            _proofItems.Add(new ProofItem(ProofType.Unknown, ""));
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

    protected override async Task OnInitializedAsync() =>
        UploadTimeModel.DriverName = await UserService.GetCurrentUserName();

    private readonly UploadTimeDto UploadTimeModel = new();

    private Task<bool> IsRobot() =>
        IsRobotHelper.IsRobot(JSRuntime, CaptchaVerifier, (msg) => MessageHelper.ShowError(msg));

    private bool _isLoading;
    private async Task UploadAndFillProofs()
    {
        _isLoading = true;
        try
        {
            var userName = await UserService.GetCurrentUserName();
            List<ProofItem> toRemove = _proofItems.Where(x => string.IsNullOrEmpty(x.Url)).ToList();
            toRemove.ForEach(x => _proofItems.Remove(x));

            if (_files.Count > 5)
            {
                throw new Exception("Maximum 5 files to upload");
            }

            List<(int, int, int, string)> ocrResults = [];
            foreach (var f in _files)
            {
                using var originalStream = f.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024); // max 5MB

                var uploadStream = new MemoryStream();
                await originalStream.CopyToAsync(uploadStream);
                uploadStream.Position = 0;

                string fileName = f.Name;
                ProofType type = ProofType.Unknown;

                var ext = Path.GetExtension(f.Name).ToLower().Trim();
                bool isStandardImage = ext == ".jpg" || ext == ".jpeg" || ext == ".png" || ext == ".bmp";

                if (!isStandardImage && uploadStream.Length >= 4)
                {
                    byte[] buffer = new byte[4];
                    await uploadStream.ReadAsync(buffer, 0, 4);
                    string magicString = System.Text.Encoding.ASCII.GetString(buffer);

                    if (magicString == "REPL")
                    {
                        type = ProofType.Hst;
                    }

                    uploadStream.Position = 0;
                }

                var res = await StorageManager.Upload(userName, StageId, fileName, uploadStream);

                if (isStandardImage)
                {
                    type = ProofType.Image;
                    var ocrRes = await AIManager.GetTimeAndDriverFromStageResultImageView(res);
                    if (ocrRes is not null)
                    {
                        ocrResults.Add(ocrRes.Value);
                    }
                }

                _proofItems.Add(new ProofItem(type, res));

                await uploadStream.DisposeAsync();
            }
            _proofItems.Add(new ProofItem(ProofType.Unknown, "")); // empty one

            // BEST TIME LOGIC
            if (ocrResults.Any())
            {
                var x1 = ocrResults.MinBy(x => x.Item1 * 60 * 100 + x.Item2 * 100 + x.Item3);
                if (UploadTimeModel.Min is null or 0 && 
                    UploadTimeModel.Sec is null or 0 &&
                    UploadTimeModel.Cc is null or 0)
                {
                    UploadTimeModel.Min = x1.Item1;
                    UploadTimeModel.Sec = x1.Item2;
                    UploadTimeModel.Cc = x1.Item3;
                }
            }

            StateHasChanged();
        }
        finally
        {
            _isLoading = false;
        }
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
        if (GetProofs().Count == 0)
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

        var proofs = GetProofs();
        var uploadResult = await TimeEntryService.Upload(
            StageId,
            UploadTimeModel.Car!.Id,
            newDriver.Id,
            UploadTimeModel.Min ?? 0,
            UploadTimeModel.Sec ?? 0,
            UploadTimeModel.Cc ?? 0,
            proofs,
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

        var proofs = GetProofs();
        var uploadResult = await TimeEntryService.Upload(
            StageId,
            UploadTimeModel.Car!.Id,
            driverId,
            UploadTimeModel.Min ?? 0,
            UploadTimeModel.Sec ?? 0,
            UploadTimeModel.Cc ?? 0,
            proofs,
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
        var proofs = GetProofs();
        var uploadResult = await TimeEntryService.Upload(
            StageId,
            UploadTimeModel.Car!.Id,
            driverId,
            UploadTimeModel.Min ?? 0,
            UploadTimeModel.Sec ?? 0,
            UploadTimeModel.Cc ?? 0,
            proofs,
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

    private void ShowUploadedMessage() =>
        MessageHelper.ShowSuccess("Uploaded.");
    
    private void ShowUploadedFailureMessage(string? msg) =>
        MessageHelper.ShowError($"Upload failed. {msg}");
}