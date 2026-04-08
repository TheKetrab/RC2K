using Bogus;
using static Bogus.DataSets.Name;

namespace RC2K.Presentation.HstEraser;

public class DataGenerator
{
    private const int DefaultSeed = 1835711;
    private readonly Random _rnd;
    private readonly Faker _faker;

    public DataGenerator(int seed, Random rnd)
    {
        _rnd = rnd;
        _faker = new Faker("en");
        _faker.Random = new Randomizer(seed);
    }
    public DataGenerator() : this(DefaultSeed, Random.Shared)
    {
    }

    public string GenerateDriverName() =>
        new string($"{_faker.Name.FirstName(Gender.Male)} {_faker.Name.LastName(Gender.Male)}"
        .ToLower()
        .Take(30)
        .ToArray());

    public int GenerateNationality() => _rnd.Next(40);

    public int GenerateCar() => _rnd.Next(23);
}
