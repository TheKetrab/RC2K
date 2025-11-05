namespace RC2K.DomainModel;

public class Car
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Class { get; init; }
    public override string ToString() => $"{Name} (A{Class})";
}