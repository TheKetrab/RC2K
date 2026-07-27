using MudBlazor;
using RC2K.DomainModel;
using RC2K.Presentation.Blazor.ViewModels;
using System.Globalization;

namespace RC2K.Presentation.Blazor.Views.Pages.Admin;

public partial class Messages
{
    private List<MessageItemViewModel> _messages = [];

    public bool AnyToUpdate =>
        _messages?.Any(x => x.IsDeleted || x.IsNew || x.IsDirty) ?? false;

    private static string? ValidateDateTime(string dateTimeStr)
    {
        if (string.IsNullOrWhiteSpace(dateTimeStr))
        {
            return "DateTime is required";
        }

        if (!DateTime.TryParseExact(dateTimeStr, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
        {
            return "DateTime should be in format: dd/MM/yyyy HH:mm:ss";
        }

        TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, localZone);
        if (utcDateTime < DateTime.UtcNow)
        {
            return "Date should not be from the past";
        }

        return null;
    }

    private string RowStyleFunc(MessageItemViewModel item, int rowNumber)
    {
        if (item.IsDeleted)
        {
            return "background: var(--mud-palette-error);";
        }

        if (item.IsDirty && !item.IsNew)
        {
            return "background: var(--mud-palette-warning);";
        }

        if (item.IsNew)
        {
            return "background: var(--mud-palette-info);";
        }

        return string.Empty;
    }

    private async Task NewItemAsync()
    {
        var elem = new MessageItemViewModel() { Published = false };
        elem.IsNew = true;
        _messages.Add(elem);
    }

    private async Task Update()
    {
        var toDelete = _messages.Where(x => x.IsDeleted).ToList();
        var toUpdate = _messages.Where(x => x.IsDirty && !x.IsDeleted && !x.IsNew).ToList();
        var toAdd = _messages.Where(x => x.IsNew).ToList();

        bool success = true;
        toDelete.ForEach(async x => success &= (await TryUpdateItem(MessageService.Delete, x)));
        toUpdate.ForEach(async x => success &= (await TryUpdateItem(MessageService.Update, x)));
        toAdd.ForEach(async x => success &= (await TryUpdateItem(MessageService.Create, x)));

        if (success)
        {
            await Task.Delay(1000);
            await ReloadMessages();
            MessageHelper.ShowSuccess("Update operation done.");
        }
        else
        {
            MessageHelper.ShowWarning("Update operation done with some errors.");
        }
    }

    private async Task<bool> TryUpdateItem(Func<DateTimeMessage, Task> operation, MessageItemViewModel viewModel)
    {
        try
        {
            var model = viewModel.ToDomainModel();
            await operation(model);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed tu update item on messages.");
            MessageHelper.ShowError($"Failed to update message {(!string.IsNullOrEmpty(viewModel.Name) ? viewModel.Name : viewModel.Id)}");
            return false;
        }
    }

    private void DeleteItemClicked(MessageItemViewModel item)
    {
        if (item.IsDeleted)
        {
            item.IsDeleted = false;
            return;
        }

        if (item.IsNew)
        {
            _messages.Remove(item);
        }
        else
        {
            item.IsDeleted = true;
        }
    }

    private void ResetItemClicked(MessageItemViewModel item)
    {
        if (!item.IsDirty)
        {
            return;
        }

        item.Reset();
    }

    protected override async Task OnInitializedAsync()
    {
        await ReloadMessages();
    }

    private async Task ReloadMessages()
    {
        var messages = (await MessageService.GetAll()).OrderBy(x => x.DateTime).ToList();
        _messages = messages.Select(x => x.ToViewModel()).ToList();
        StateHasChanged();
    }



}
