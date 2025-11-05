namespace RC2K.DomainModel;

public class VerifyInfo
{
    public required Guid Id { get; init; }
    public required Guid VerifierId { get; init; }
    public string? Comment { get; set; }
    public DateTime VerifyDate { get; init; }

    public User? Verifier { get; set; }
}
