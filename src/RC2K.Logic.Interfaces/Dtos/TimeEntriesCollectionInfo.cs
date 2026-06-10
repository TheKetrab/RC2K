using RC2K.DomainModel;
using System.Diagnostics.CodeAnalysis;

namespace RC2K.Logic.Interfaces.Dtos;

[ExcludeFromCodeCoverage]
public record TimeEntriesCollectionInfo(
    List<TimeEntry> OrderedTimeEntries, 
    TimeEntry? Best,
    Dictionary<Guid, int> GeneralPoints,
    Dictionary<Guid, int> CarPoints,
    Dictionary<Guid, int> Places,
    Dictionary<Guid, int> PlacesByCar,
    Dictionary<Guid, int> PlacesByClass,
    PointsInfo PointsInfo
);
