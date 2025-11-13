namespace RC2K.DomainModel;

public class Proof
{
    public ProofType Type { get; set; } = ProofType.Unknown;
    public required string Url { get; init; }
}