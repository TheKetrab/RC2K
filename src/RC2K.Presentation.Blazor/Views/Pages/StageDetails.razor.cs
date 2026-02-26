using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using RC2K.DomainModel;
using RC2K.Extensions;
using RC2K.Presentation.Blazor.Views.Components;
using RC2K.Presentation.Blazor.Views.Dialogs;

namespace RC2K.Presentation.Blazor.Views.Pages;

public partial class StageDetails
{

    [Parameter]
    public required string RaceName { get; init; }

    [Parameter]
    public required int Id { get; init; }

    [SupplyParameterFromQuery]
    [Parameter]
    public bool ShowWaypoints { get; set; } = true;

    private string _directionParameter = string.Empty;
    [SupplyParameterFromQuery(Name = "direction")]
    public string DirectionParameter
    {
        get => _directionParameter;
        set
        {
            _directionParameter = value;
            Direction = value == "Arcade" ? DomainModel.Direction.Arcade : DomainModel.Direction.Simulation;
        }
    }

    public DomainModel.Direction Direction { get; set; }

    public bool IsArcade => Direction == DomainModel.Direction.Arcade;

    private int? _selectedClass;
    private int _stageId;
    private string _api = string.Empty;
    private IList<double[]> _waypoints = [];
    private string _name = string.Empty;
    private string _raceName = string.Empty;
    private string _description = string.Empty;
    private string _imgName = string.Empty;
    private DomainModel.StageDetails _stageDetails = new();
    private List<DomainModel.TimeEntry> _timeEntries = new();
    private string? _path;
    private TimeEntryList _timeEntryListRef = default!;

    protected override async Task OnInitializedAsync()
    {
        var stage = await StageService.GetByCode(Id, Direction);
        if (stage == null)
        {
            NavigationManager.NavigateTo("/stages");
            return;
        }

        _stageId = stage.Id;
        _api = stage.StageWaypoints?.ApiHint ?? string.Empty;
        _waypoints = await StageService.GetWaypoints(Id, stage.Direction == DomainModel.Direction.Arcade);

        _path = await StageService.GetPath(Id);

        _raceName = LevelHelper.RallyCodeToRallyName(Enum.Parse<RallyCode>(RaceName, true));
        _name = stage.StageData?.Name ?? string.Empty;
        _imgName = stage.StageData?.ImgName ?? string.Empty;
        _description = stage.StageData?.Description ?? string.Empty;
        _stageDetails = stage.StageData?.StageDetails ?? new DomainModel.StageDetails();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await RecalculateHeight();
    }

    private async Task RecalculateHeight()
    {
        await JSRuntime.InvokeVoidAsync("calculateContainerHeight", "containerId");
    }

    private Task HandleOnTimeEntryListLoaded()
    {
        Task.Run(async () =>
        {
            await Task.Delay(500);
            await RecalculateHeight();
        });
        return Task.CompletedTask;
    }

    private async Task HandleNewPathCachedEvent(string path)
    {
        var stage = await StageService.GetByCode(Id, IsArcade ? DomainModel.Direction.Arcade : DomainModel.Direction.Simulation);
        if (stage == null) return;
        int stageCode = stage.Code;
        await StageService.SetPath(stageCode, path);
    }

    private async Task OpenUploadDialog()
    {
        DialogParameters<UploadTime> parameters = new()
    {
        { x => x.StageId, this._stageId },
    };

        object? result = await DialogHelper.ShowDialogAndGetResult<UploadTime, object>("Upload time", parameters);
        if ((result == null ? 0 : (int)result) == 1)
        {
            await this._timeEntryListRef.ReloadTimeEntries();
        }
    }

    private void GoToPrevious()
    {
        if (Id % 10 == 1) return;

        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var query = uri.Query;

        NavigationManager.NavigateTo($"/stages/{RaceName}/{Id - 1}{query}", forceLoad: true);
    }
    private void GoToNext()
    {
        if (Id % 10 == 6) return;

        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var query = uri.Query;

        NavigationManager.NavigateTo($"/stages/{RaceName}/{Id + 1}{query}", forceLoad: true);
    }
    private void GoToArcadeVersion()
    {
        string uri = NavigationManager.GetUriWithQueryParameter("direction", "Arcade");
        NavigationManager.NavigateTo(uri, forceLoad: true);
    }
    private void GoToSimulationVersion()
    {
        string uri = NavigationManager.GetUriWithQueryParameter("direction", "Simulation");
        NavigationManager.NavigateTo(uri, forceLoad: true);
    }
}