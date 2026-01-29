using RC2K.Logic.Interfaces;

namespace RC2K.Presentation.Blazor;

public interface IHstUploadManager
{
    Task<Result<List<(int, string, string)>>> UploadMany(IEnumerable<Parser.Models.Hst.TimeEntry> hstTimeEntries, IProgress<int>? progress = null);
}