using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace RC2K.Presentation.Blazor.Views.Dialogs;

public class DialogHelper(IDialogService dialogService)
{
    private readonly DialogOptions _commonOptions = new()
    {
        CloseButton = true,
        BackdropClick = true,
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.Medium
    };

    public async Task<TResult?> ShowDialogAndGetResult<TDialog, TResult>(string header, DialogParameters? parameters = null)
        where TDialog : IComponent
        where TResult : class
    {
        var dialogRef = parameters is null
            ? await dialogService.ShowAsync<TDialog>(header, _commonOptions)
            : await dialogService.ShowAsync<TDialog>(header, parameters, _commonOptions);
        
        var dialogResult = await dialogRef.Result;
        if (dialogResult is null || dialogResult.Canceled)
        {
            return default;
        }

        return dialogResult.Data as TResult;
    }

    public async Task ShowMessageBox(string title, string message)
    {
        await dialogService.ShowMessageBoxAsync(title, message, options: _commonOptions);
    }

    public async Task<bool> ShowYesNoDialog(string title, string message, string yes, string no)
    {
        var res = await dialogService.ShowMessageBoxAsync(title, message,  options: _commonOptions);
        return res is true;
    }

}
