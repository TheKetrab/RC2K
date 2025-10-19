using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class UserRepository(IDataContext context)
    : AbstractRepository<User>(context), IUserRepository
{
    protected override IQueryable<User> DataSet => _context.Users;
}
