namespace RC2K.DomainModel;

public class Stage : IEntity
{
    public required int Id { get; init; }
    /// <summary>
    /// Refers to LEVEL[Code] of Rally/files/level
    /// </summary>
    public required int Code { get; init; }
    public required bool IsArcade { get; init; }

    public StageData? StageData { get; set; }
    public StageWaypoints? StageWaypoints { get; set; }

    public override string ToString() => $"Stage{Code} {(IsArcade ? "(A)" : "")}";
}
