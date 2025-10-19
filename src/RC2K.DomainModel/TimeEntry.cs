namespace RC2K.DomainModel;

public class TimeEntry : IEntity
{

    public required int Id { get; init; }
    public required int StageId { get; init; }
    public required int CarId { get; init; }
    public required int DriverId { get; init; }
    public required TimeOnly Time { get; init; }
    public required DateTime UploadTime { get; init; }
    public int? VerifyInfoId { get; set; }

    public Stage? Stage { get; set; }
    public Car? Car { get; set; }
    public Driver? Driver { get; set; }
    public VerifyInfo? VerifyInfo { get; set; }
}
