namespace RC2K.Presentation.Blazor.Views.Dialogs;

public class DeleteTimeEntryDialog : TextBoxDialog
{
    public DeleteTimeEntryDialog() : base(
        "DANGER - Delete zone",
        "",
        "Time",
        yes: "Delete")
    {

    }
}
