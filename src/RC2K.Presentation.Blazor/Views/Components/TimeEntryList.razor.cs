using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Dtos;
using RC2K.Presentation.Blazor.Views.Dialogs;
using RC2K.Presentation.Shared;
using System.Collections;
using System.Collections.Specialized;

namespace RC2K.Presentation.Blazor.Views.Components;

public class TimeEntryListItemsCollection : INotifyCollectionChanged, IEnumerable<TimeEntryListItem>
{
    private readonly List<TimeEntryListItem> _list = [];

    public void Set(IEnumerable<TimeEntryListItem> items)
    {
        _list.Clear();
        _list.AddRange(items);
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public IEnumerator<TimeEntryListItem> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
}

public partial class TimeEntryList
{

    private CancellationTokenSource? _cts;
    private MudDataGrid<TimeEntryListItem>? _gridRef;
    public TimeEntryListItemsCollection Items { get; set; } = new();

    public PointsInfo? PointsInfo { get; set; }
    public TimeEntry? Best { get; set; }
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

    [Parameter]
    public Func<Task>? OnReloadRequested { get; set; }

    [Parameter] 
    public bool VerificationMode { get; set; }

    // async loading
    private bool overlayVisible;
    private bool dataLoaded;

    // filters
    public bool filterDriverCheckBox;
    public string? filterDriverText;
    public bool filterCarCheckBox;
    public Car? filterCar;
    public bool filterLabelsCheckBox;
    public string? filterLabelsText;
    public bool filterMfmiCheckBox;
    private int? _selectedClass;

    private TimeEntryListItem? _currentContextMenuItem;

    private HashSet<TimeEntryListItem> _selectedItems = [];

    public List<TimeEntry> GetSelectedTimeEntries()
    {
        var selectedTimeEntries =
            _selectedItems.Select(x => x.Data).ToList();

        return selectedTimeEntries;
    }

    private double _tableHeight = 250;

    private async Task GetTableHeight()
    {
        _tableHeight = await JSRuntime.InvokeAsync<double>(
            "eval",
            "document.getElementById('time-entry-list-table')?.offsetHeight || 0"
        );
    }

    protected override async Task OnParametersSetAsync()
    {
        if (VerificationMode)
        {
            await ReloadTimeEntriesForVerification();
        }
        else
        {
            if (_gridRef != null)
            {
                await GetTableHeight();
            }

            if (prevStageId != StageId)
            {
                prevStageId = StageId;
                await ReloadTimeEntries();
            }
        }
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

    private int? prevStageId;

    public async Task ReloadTimeEntries()
    {
        OpenLoadingOverlay();
        if (OnReloadRequested != null)
        {
            await OnReloadRequested();
        }
        
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        await Task.Delay(250); // intentional delay to handle multiple clicking at once
        _cts.Token.ThrowIfCancellationRequested();

        var info = await TimeEntryService.CalculateTimeEntriesWithPoints(StageId, ct: _cts.Token);

        Best = info.Best;
        GeneralPoints = info.GeneralPoints;
        CarPoints = info.CarPoints;
        PointsInfo = info.PointsInfo;

        var items = info.OrderedTimeEntries.Select(x => new TimeEntryListItem(this, x)).ToList();
        foreach (var itm in items)
        {
            itm.Place = info.Places.TryGetValue(itm.Data.Id, out int place) ? place : 0;
            itm.PlaceByCar = info.PlacesByCar[itm.Data.Id];
            itm.PlaceByClass = info.PlacesByClass[itm.Data.Id];
        }

        Items.Set(items.OrderBy(x => x.Place));

        CloseLoadingOverlay();

        if (OnLoaded != null)
        {
            await OnLoaded();
        }
    }

    public async Task ReloadTimeEntriesForVerification()
    {
        OpenLoadingOverlay();

        var notVerified = await TimeEntryService.GetAllNotVerified();
        var items = notVerified.Select(x => new TimeEntryListItem(this, x)).ToList();

        Items.Set(items.OrderBy(x => x.Place));

        CloseLoadingOverlay();
        OnLoaded?.Invoke();
    }

    private Func<TimeEntryListItem, bool> QuickFilter => 
        x => IsOkForDriverFilter(x) &&
             IsOkForCarFilter(x) &&
             IsOkForLabelsFilter(x) &&
             IsOkForMfmiFilter(x) &&
             IsOkForClassFilter(x);

    private bool IsOkForDriverFilter(TimeEntryListItem item)
    {
        if (!filterDriverCheckBox || string.IsNullOrEmpty(filterDriverText))
        {
            return true;
        }

        return item.DisplayName.StartsWith(filterDriverText, StringComparison.InvariantCultureIgnoreCase);
    }

    private bool IsOkForCarFilter(TimeEntryListItem item)
    {
        if (!filterCarCheckBox || filterCar == null)
        {
            return true;
        }

        return item.Data.CarId == filterCar.Id;
    }

    private bool IsOkForLabelsFilter(TimeEntryListItem item)
    {
        if (!filterLabelsCheckBox || string.IsNullOrEmpty(filterLabelsText))
        {
            return true;
        }

        var split = filterLabelsText.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var s in split)
        {
            if (item.Labels.Any(l => string.Equals(l, s, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsOkForMfmiFilter(TimeEntryListItem item)
    {
        if (!filterMfmiCheckBox)
        {
            return true;
        }

        if (item.Labels.Any(x => x.StartsWith("MFMI")))
        {
            return true;
        }
        return false;
    }

    private bool IsOkForClassFilter(TimeEntryListItem item)
    {
        if (_selectedClass == null)
        {
            // 'ALL' filter means not bonus
            return item.Data.Car?.Class != Car.BonusClass;
        }

        return item.Data.Car?.Class == _selectedClass;
    }

    private async Task CopyIdToClipboard(Guid id)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", id.ToString());
            _currentContextMenuItem = null;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to copy to clipboard");
        }
    }

    private void ShowContextMenu(MouseEventArgs _, TimeEntryListItem item)
    {
        if (!CanShowContextMenu(item))
        {
            return;
        }

        _currentContextMenuItem = item;
    }

    private void CloseContextMenu() => _currentContextMenuItem = null;

    private bool CanShowContextMenu(TimeEntryListItem item)
    {
        var userName = HttpContextAccessor.HttpContext?.User.Identity?.Name;
        var isAdmin = HttpContextAccessor.HttpContext?.User.IsInRole("admin") ?? false;

        if (isAdmin)
        {
            return true;
        }

        // Check if user is the owner (driver)
        if (item.Data.Driver?.Known == true && item.Data.Driver?.User?.Name == userName)
        {
            return true;
        }

        return false;
    }

    private bool CanDeleteTimeEntry(TimeEntryListItem item) => CanShowContextMenu(item);

    private async Task OpenDeleteConfirmation(TimeEntryListItem item)
    {
        DialogParameters parameters = [];
        parameters.Add("DynamicMsg", $"If you are sure you want to delete this time entry, type its time: {item.TimeDisplay}");
        string? typedTime = await DialogHelper.ShowDialogAndGetResult<DeleteTimeEntryDialog, string>("Delete time entry", parameters);

        if (typedTime != item.TimeDisplay)
        {
            return;
        }
        else
        {
            await ConfirmDelete(item);
        }

        _currentContextMenuItem = null;
    }

    private async Task ConfirmDelete(TimeEntryListItem item)
    {
        try
        {
            await TimeEntryService.Delete([item.Data]);
            await ReloadTimeEntries();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to delete time entry");
        }
    }

    private string RowStyleFunc(TimeEntryListItem item, int index)
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