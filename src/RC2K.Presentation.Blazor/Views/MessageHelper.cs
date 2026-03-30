using MudBlazor;

namespace RC2K.Presentation.Blazor.Views;

public class MessageHelper(ISnackbar Snackbar)
{
    public void ShowError(string message)
    {
        Snackbar.Add(message, Severity.Error, config =>
        {
            config.ShowCloseIcon = true;
            config.VisibleStateDuration = 5000;
        });
    }

    public void ShowSuccess(string message)
    {
        Snackbar.Add(message, Severity.Success, config =>
        {
            config.ShowCloseIcon = true;
            config.VisibleStateDuration = 5000;
        });
    }

    public void ShowWarning(string message)
    {
        Snackbar.Add(message, Severity.Warning, config =>
        {
            config.ShowCloseIcon = true;
            config.VisibleStateDuration = 5000;
        });
    }

}
