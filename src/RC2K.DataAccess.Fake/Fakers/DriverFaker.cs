
using Bogus;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Fakers;

public class DriverFaker : Faker<Driver>
{
    private int _id = 1;

    private readonly IDataContext _context;

    public DriverFaker(IDataContext context)
    {
        _context = context;

        RuleFor(x => x.Id, f => _id++);
        RuleFor(x => x.Known, f => f.Random.Bool());
        RuleFor(x => x.UserId, (f,d) => d.Known ? f.Random.Int(0, _context.Users.Count()) : null);
        RuleFor(x => x.Name, (f,d) => d.Known ? null : f.Person.FullName);
        RuleFor(x => x.Key, (f,d) => d.Known ? null : f.Random.String());
    }
}
