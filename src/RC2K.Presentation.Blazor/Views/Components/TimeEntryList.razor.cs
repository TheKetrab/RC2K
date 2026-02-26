using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using RC2K.DomainModel;
using RC2K.Presentation.Blazor.Views.Dialogs;
using RC2K.Presentation.Shared;

namespace RC2K.Presentation.Blazor.Views.Components;

public partial class TimeEntryList
{

    private MudDataGrid<TimeEntryListItem>? _gridRef;
    private List<TimeEntryListItem> _items = [];

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
    public bool VerificationMode { get; set; }

    [Parameter]
    public int? FilterClass { get; set; }

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

    private TimeEntryListItem? _currentContextMenuItem;

    public List<TimeEntry> GetSelectedTimeEntries()
    {
        var selectedTimeEntries =
            _gridRef?.SelectedItems.Select(x => x.Data).ToList() ?? [];

        return selectedTimeEntries;
    }

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

    public async Task ReloadTimeEntries()
    {
        OpenLoadingOverlay();

        var info = await TimeEntryService.CalculateTimeEntriesWithPoints(StageId);

        Best = info.Best;
        GeneralPoints = info.GeneralPoints;
        CarPoints = info.CarPoints;

        var items = info.OrderedTimeEntries.Select(x => new TimeEntryListItem(this, x)).ToList();
        foreach (var itm in items)
        {
            itm.Place = info.Places.TryGetValue(itm.Data.Id, out int place) ? place : 0;
            itm.PlaceByCar = info.PlacesByCar[itm.Data.Id];
            itm.PlaceByClass = info.PlacesByClass[itm.Data.Id];
        }

        _items = items.OrderBy(x => x.Place).ToList();

        CloseLoadingOverlay();
        StateHasChanged();
        OnLoaded?.Invoke();
    }

    public async Task ReloadTimeEntriesForVerification()
    {
        OpenLoadingOverlay();

        var notVerified = await TimeEntryService.GetAllNotVerified();
        var items = notVerified.Select(x => new TimeEntryListItem(this, x)).ToList();

        _items = items;

        CloseLoadingOverlay();
        StateHasChanged();
        OnLoaded?.Invoke();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
        {
            return;
        }

        if (VerificationMode)
        {
            await ReloadTimeEntriesForVerification();
        }
        else
        {
            await ReloadTimeEntries();
        }
    }

    void GoToPage(int p)
    {
        var newLocation = NavigationManager.GetUriWithQueryParameter("Page", p);
        NavigationManager.NavigateTo(newLocation);
    }

    private Func<TimeEntryListItem, bool> _quickFilter => x =>
    {
        return IsOkForDriverFilter(x) &&
               IsOkForCarFilter(x) &&
               IsOkForLabelsFilter(x) &&
               IsOkForMfmiFilter(x) &&
               IsOkForClassFilter(x);
    };

    private bool IsOkForDriverFilter(TimeEntryListItem item)
    {
        if (!filterDriverCheckBox) return true;
        if (string.IsNullOrEmpty(filterDriverText)) return true;

        return item.DisplayName.StartsWith(filterDriverText, StringComparison.InvariantCultureIgnoreCase);
    }

    private bool IsOkForCarFilter(TimeEntryListItem item)
    {
        if (!filterCarCheckBox) return true;
        if (filterCar == null) return true;

        return item.Data.CarId == filterCar.Id;
    }

    private bool IsOkForLabelsFilter(TimeEntryListItem item)
    {
        if (!filterLabelsCheckBox) return true;
        if (string.IsNullOrEmpty(filterLabelsText)) return true;

        var split = filterLabelsText.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var s in split)
        {
            if (item.Labels.Any(l => string.Equals(l, s, StringComparison.InvariantCultureIgnoreCase)))
                return true;
        }
        return false;
    }

    private bool IsOkForMfmiFilter(TimeEntryListItem item)
    {
        if (!filterMfmiCheckBox) return true;

        if (item.Labels.Any(x => x.StartsWith("MFMI")))
        {
            return true;
        }
        return false;
    }

    private bool IsOkForClassFilter(TimeEntryListItem item)
    {
        if (FilterClass == null)
        {
            // 'ALL' filter means not bonus
            return item.Data.Car?.Class != Car.BonusClass;
        }

        return item.Data.Car?.Class == FilterClass;
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
            Console.WriteLine($"Failed to copy to clipboard: {ex.Message}");
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

    private void CloseContextMenu()
    {
        _currentContextMenuItem = null;
    }

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

    private bool CanDeleteTimeEntry(TimeEntryListItem item)
    {
        return CanShowContextMenu(item);
    }

    private async Task OpenDeleteConfirmation(TimeEntryListItem item)
    {
        DialogParameters parameters = new();
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
            await TimeEntryService.Delete(new List<TimeEntry> { item.Data });
            await ReloadTimeEntries();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete time entry: {ex.Message}");
        }
    }

    private string _rowStyleFunc(TimeEntryListItem item, int index)
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