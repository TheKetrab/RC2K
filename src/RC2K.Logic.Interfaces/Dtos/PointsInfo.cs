
namespace RC2K.Logic.Interfaces.Dtos;

public record PointsInfo(
    int? Best,
    Dictionary<Guid, int> TotalPoints,
    Dictionary<Guid, int> GeneralPoints,
    Dictionary<Guid, int> CarA8Points,
    Dictionary<Guid, int> CarA7Points,
    Dictionary<Guid, int> CarA6Points,
    Dictionary<Guid, int> CarA5Points,
    Dictionary<Guid, int> CarBonusPoints);
