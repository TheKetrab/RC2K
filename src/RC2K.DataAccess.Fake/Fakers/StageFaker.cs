
using Bogus;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Fakers;

public class StageFaker : Faker<Stage>
{
    private int _id = 1;

    public StageFaker()
    {
        RuleFor(x => x.Id, f => _id++);
        RuleFor(x => x.IsArcade, f => f.Random.Bool());
    }
}
