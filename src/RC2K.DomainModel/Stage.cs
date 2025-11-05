namespace RC2K.DomainModel;

public class Stage
{
    public required int Id { get; init; }
    /// <summary>
    /// Refers to LEVEL[Code] of Rally/files/level
    /// </summary>
    public required int Code { get; init; }
    public required Direction Direction { get; init; }

    public StageData? StageData { get; set; }
    public StageWaypoints? StageWaypoints { get; set; }

    public override string ToString() => 
        string.Join(" ", new string?[] {
            $"Stage{Code}",
            Direction == Direction.Arcade ? "(A)" : "",
            StageData?.Name}
        .Where(x => !string.IsNullOrEmpty(x)));
}
