using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class DriverRepository(IDataContext context)
    : AbstractRepository<Driver>(context), IDriverRepository
{
    protected override IQueryable<Driver> DataSet => _context.Drivers;

    public Task<bool> Exist(string name) =>
        Task.FromResult(DataSet.FirstOrDefault(x => x.ToString() == name) != null);
}
