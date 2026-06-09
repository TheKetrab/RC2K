using Microsoft.AspNetCore.Components;
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

    private int _stageId;
    private int _stageCode;
    private string _api = string.Empty;
    private IList<double[]> _waypoints = [];
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _imgName = string.Empty;
    private DomainModel.StageDetails _stageDetails = new();
    private string? _path;
    private TimeEntryList _timeEntryListRef = default!;
    private PointsList _pointsListRef = default!;

    protected override async Task OnParametersSetAsync()
    {
        var stage = await StageService.GetByCode(Id, Direction);
        if (stage == null)
        {
            NavigationManager.NavigateTo("/stages");
            return;
        }

        _stageId = stage.Id;
        _stageCode = stage.Code;
        _api = stage.StageWaypoints?.ApiHint ?? string.Empty;
        _waypoints = await StageService.GetWaypoints(Id, stage.Direction == DomainModel.Direction.Arcade);

        _path = await StageService.GetPath(Id);

        _name = stage.StageData?.Name ?? string.Empty;
        _imgName = stage.StageData?.ImgName ?? string.Empty;
        _description = stage.StageData?.Description ?? string.Empty;
        _stageDetails = stage.StageData?.StageDetails ?? new DomainModel.StageDetails();
    }

    private Task HandleOnTimeEntryListReloadRequested()
    {
        _pointsListRef?.OpenLoadingOverlay();
        return Task.CompletedTask;
    }

    private async Task HandleOnTimeEntryListLoaded()
    {
        var pointsInfo = _timeEntryListRef.PointsInfo;
        if (pointsInfo is not null)
        {
            await _pointsListRef.SetPointsInfo(pointsInfo);
        }
    }

    private async Task HandleNewPathCachedEvent(string path)
    {
        var stage = await StageService.GetByCode(Id, IsArcade ? DomainModel.Direction.Arcade : DomainModel.Direction.Simulation);
        if (stage == null)
        {
            return;
        }

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
        if (Id % 10 == 1)
        {
            return;
        }

        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var query = uri.Query;

        NavigationManager.NavigateTo($"/stages/{RaceName}/{Id - 1}{query}", forceLoad: false);
    }
    private void GoToNext()
    {
        if (Id % 10 == 6)
        {
            return;
        }

        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        var query = uri.Query;

        NavigationManager.NavigateTo($"/stages/{RaceName}/{Id + 1}{query}", forceLoad: false);
    }
    private void GoToArcadeVersion()
    {
        string uri = NavigationManager.GetUriWithQueryParameter("direction", "Arcade");
        NavigationManager.NavigateTo(uri, forceLoad: false);
    }
    private void GoToSimulationVersion()
    {
        string uri = NavigationManager.GetUriWithQueryParameter("direction", "Simulation");
        NavigationManager.NavigateTo(uri, forceLoad: false);
    }
}