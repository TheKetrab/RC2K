namespace RC2K.DomainModel;

public class StageWaypoints
{
    public required int StageCode { get; init; }
    /// <summary>
    /// List of coordinates (pairs) separated by semicolon: x0,y0;x1,y1;x2,y2
    /// </summary>
    public required string Waypoints { get; init; }
    /// <summary>
    /// List of more detailed coordinates (pairs) separated by semicolon: x0,y0;x1,y1;x2,y2
    /// </summary>
    public string? Path { get; set; }
    public bool IsPathValid { get; set; }
}
