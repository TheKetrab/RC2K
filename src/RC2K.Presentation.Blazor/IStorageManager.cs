namespace RC2K.Presentation.Blazor;

public interface IStorageManager
{
    Task<string> Upload(string userName, int stageId, string fileName, Stream file);
}
