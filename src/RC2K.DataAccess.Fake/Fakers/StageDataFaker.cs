
using Bogus;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Fakers;

public class StageDataFaker : Faker<StageData>
{
    public StageDataFaker()
    {
        RuleFor(x => x.Name, f => f.Address.City());
        RuleFor(x => x.Description, f => string.Join(" ", f.Lorem.Words(10)));
        RuleFor(x => x.ImgName, f => string.Empty);
    }
}
