using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Database.Repositories;

public class DriverRepository(IDataContext context)
    : AbstractRepository<Driver>(context), IDriverRepository
{
    protected override IQueryable<Driver> DataSet => _context.Drivers;

    public async Task<bool> Exist(string name) =>
        (await DataSet.FirstOrDefaultAsync(x => x.ToString() == name)) != null;
}
