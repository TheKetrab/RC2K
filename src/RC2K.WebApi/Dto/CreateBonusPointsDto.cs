namespace RC2K.WebApi.Dto;

public class CreateBonusPointsDto
{
    public required Guid driverId { get; set; }
    public required int points { get; init; }
    public required string comment { get; init; }
}
