namespace RC2K.Presentation.Blazor;

public interface IAIManager
{
    Task<(int, int, int, string)?> GetTimeAndDriverFromStageResultImageView(string imageUrl);
}
