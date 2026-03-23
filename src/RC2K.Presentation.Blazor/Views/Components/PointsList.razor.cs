using Microsoft.AspNetCore.Components;
using MudBlazor;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Dtos;
using RC2K.Presentation.Shared;

namespace RC2K.Presentation.Blazor.Views.Components;

public partial class PointsList
{
    private MudDataGrid<PointsListItem>? _gridRef;
    private List<PointsListItem> _items = [];

    public int? Best { get; set; }
    public Dictionary<Guid, int> GeneralPoints { get; set; } = [];
    public Dictionary<Guid, int> CarPoints { get; set; } = [];

    [SupplyParameterFromQuery] 
    public int Page { get; set; }
    
    [Parameter] 
    public int PerPage { get; set; }

    [Parameter]
    public int StageId { get; set; }

    [Parameter]
    public Func<Task>? OnLoaded { get; set; }

    // async loading
    private bool overlayVisible;
    private bool dataLoaded;

    protected override void OnInitialized()
    {
        OpenLoadingOverlay();
        base.OnInitialized();
    }

    private void OpenLoadingOverlay()
    {
        overlayVisible = true;
        dataLoaded = false;
    }
    private void CloseLoadingOverlay()
    {
        overlayVisible = false;
        dataLoaded = true;
    }

    public async Task SetPointsInfo(PointsInfo pointsInfo)
    {
        OpenLoadingOverlay();

        this.Best = pointsInfo.Best;

        List<PointsListItem> items = [];
        int place = 1;
        int rankedPlace = 1;
        int prevPoints = int.MaxValue;
        foreach (var tp in pointsInfo.TotalPoints.OrderByDescending(x => x.Value))
        {
            if (prevPoints > tp.Value)
            {
                rankedPlace = place;
            }

            Guid driverId = tp.Key;
            Driver driver = (await DriverService.GetById(driverId, CancellationToken.None))!;
            var itm = new PointsListItem(this, driver, rankedPlace, tp.Value,
                pointsInfo.GeneralPoints.TryGetValue(driverId, out int valGp) ? valGp : 0,
                pointsInfo.CarA8Points.TryGetValue(driverId, out int valA8) ? valA8 : 0,
                pointsInfo.CarA7Points.TryGetValue(driverId, out int valA7) ? valA7 : 0,
                pointsInfo.CarA6Points.TryGetValue(driverId, out int valA6) ? valA6 : 0,
                pointsInfo.CarA5Points.TryGetValue(driverId, out int valA5) ? valA5 : 0,
                pointsInfo.CarBonusPoints.TryGetValue(driverId, out int valB) ? valB : 0);

            items.Add(itm);

            prevPoints = tp.Value;
            place++;
        }

        _items = items;

        CloseLoadingOverlay();
        StateHasChanged();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        OpenLoadingOverlay();
    }

    private string _rowStyleFunc(PointsListItem item, int index)
    {
        string? name = HttpContextAccessor.HttpContext?.User.Identity?.Name;
        if (name is not null)
        {
            if (item.DisplayName == name)
            {
                return $"background-color: {Rc2kColorPallete.complementary200};";
            }
        }

        if (index % 2 != 0)
        {
            return $"background-color: {Rc2kColorPallete.primary50};";
        }
        return string.Empty;
    }
}