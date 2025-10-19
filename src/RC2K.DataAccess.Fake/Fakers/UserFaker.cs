
using Bogus;
using RC2K.DomainModel;
using RC2K.Extensions;

namespace RC2K.DataAccess.Fake.Fakers;

public class UserFaker : Faker<User>
{
    private int _id = 1;

    public UserFaker()
    {
        RuleFor(x => x.Id, f => _id++);
        RuleFor(x => x.Name, f => f.Person.FullName);
        RuleFor(x => x.Roles, (f, u) => UserIdToRoles(u.Id));
    }

    private const string AdminRole = "Admin";
    private const int _rolesCnt = 7;
    private string[] _roles = new string[_rolesCnt] { AdminRole, "Moderator", "Verifier", "Driver", "Deserved", "Privelaged", "Inactive" };
    private string[] UserIdToRoles(int id)
    {
        if (id == 1)
        {
            List<string> roles = _roles
                .RandomNonRepetitive(Random.Shared, 2)
                .ToList();

            if (!roles.Contains(AdminRole))
            {
                roles.Add(AdminRole);
            }
            return roles.ToArray();
        }

        int totalRoles = Random.Shared.Next(4) + 1;

        return _roles
            .RandomNonRepetitive(Random.Shared, totalRoles)
            .ToArray();
    }
}
