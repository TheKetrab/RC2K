
using Bogus;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Fakers;

public class VerifyInfoFaker : Faker<VerifyInfo>
{
    private int _id = 1;

    private readonly DateTime _minDate = new DateTime(2020, 01, 01);
    private readonly DateTime _maxDate = new DateTime(2024, 12, 31);

    private readonly IDataContext _context;

    public VerifyInfoFaker(IDataContext context)
    {
        _context = context;
        RuleFor(x => x.Id, f => _id++);
        RuleFor(x => x.VerifyDate, f => f.Date.Between(_minDate, _maxDate));
        RuleFor(x => x.VerifierId, GetVerifierId);
        RuleFor(x => x.Comment, f => f.Lorem.Text());
    }
    private int GetVerifierId(Faker f)
    {
        int[] verifiersIds = _context.Users
            .Where(u => u.Roles.Any(x => x == "Verifier"))
            .Select(u => u.Id)
            .ToArray();

        return f.Random.FromArray(verifiersIds);
    } 
}
