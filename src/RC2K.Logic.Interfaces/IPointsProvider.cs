
using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IPointsProvider
{
    Dictionary<Guid, int> CalculateGeneralStagePoints(List<TimeEntry> timeEntries);
    Dictionary<Guid, int> CalculateCarStagePoints(List<TimeEntry> timeEntries);
    Dictionary<Guid, int> CalculatePlace(List<TimeEntry> timeEntries);
    Dictionary<Guid, int> CalculatePlaceByCar(List<TimeEntry> timeEntries);

    int GetPlaceFromGeneralPoints(int generalPoints);
    int GetPlaceFromA8CarPoints(int a8carPoints);
    int GetPlaceFromA7CarPoints(int a7carPoints);
    int GetPlaceFromA6CarPoints(int a6carPoints);
    int GetPlaceFromA5CarPoints(int a5carPoints);
    int GetPlaceFromBonusCarPoints(int bonusCarPoints);
}
