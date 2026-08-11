using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

public class MfmiLayoutState
{
    public IContest? Contest { get; private set; }
    public event Action? OnStateChanged;

    public void SetContest(IContest contest)
    {
        Contest = contest;
        OnStateChanged?.Invoke();
    }
}