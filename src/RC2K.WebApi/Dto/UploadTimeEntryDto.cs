namespace RC2K.WebApi.Dto;

public class UploadTimeEntryDto
{
    public required Guid DriverId { get; set; }
    public required int StageId { get; init; }
    public required int CarId { get; init; }
    public required int Time { get; init; }
    public required string Labels { get; init; }
    public List<string>? Proofs { get; set; }
    public required string Date { get; init; }
    public Guid? VerifyInfoId { get; set; }
}
