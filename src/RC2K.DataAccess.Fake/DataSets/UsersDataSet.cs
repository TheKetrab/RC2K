
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class UsersDataSet(IDataContext? context = null)
    : AbstractDataSet<User, UserFaker, UsersDataSet>(context)
{
    protected override UserFaker CreateFaker() => new UserFaker();
}
