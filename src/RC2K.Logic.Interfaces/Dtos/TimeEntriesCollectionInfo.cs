using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Dtos;

public record TimeEntriesCollectionInfo(
    List<TimeEntry> OrderedTimeEntries, 
    TimeEntry Best,
    Dictionary<Guid, int> GeneralPoints,
    Dictionary<Guid, int> CarPoints,
    Dictionary<Guid, int> Places,
    Dictionary<Guid, int> PlacesByCar
);
