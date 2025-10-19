namespace RC2K.DomainModel;

public class VerifyInfo : IEntity
{
    public required int Id { get; init; }
    public required int VerifierId { get; init; }
    public string? Comment { get; set; }
    public DateTime VerifyDate { get; init; }

    public User? Verifier { get; set; }
}
