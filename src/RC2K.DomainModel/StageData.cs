namespace RC2K.DomainModel;

public class StageData
{
    public required int StageCode { get; set; }
    public required string Name { get; init; }
    public string? Description { get; set; }
    public string? ImgName { get; set; }
    public StageDetails? StageDetails { get; set; }

    public override string ToString() => Name;
}
