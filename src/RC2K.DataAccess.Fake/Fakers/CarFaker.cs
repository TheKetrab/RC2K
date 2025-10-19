
using Bogus;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Fakers;

public class CarFaker : Faker<Car>
{
    private int _id = 1;

    public CarFaker()
    {
        RuleFor(x => x.Id, x => _id++);
        RuleFor(x => x.Name, x => x.Vehicle.Model());
    }
}
