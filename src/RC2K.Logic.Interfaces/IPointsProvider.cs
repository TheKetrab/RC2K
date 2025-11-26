
using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IPointsProvider
{
    Dictionary<Guid, int> CalculateGeneralStagePoints(List<TimeEntry> timeEntries);
    Dictionary<Guid, int> CalculateCarStagePoints(List<TimeEntry> timeEntries);
}
