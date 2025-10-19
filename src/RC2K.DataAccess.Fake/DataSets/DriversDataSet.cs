using Bogus;
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class DriversDataSet(IDataContext? context = null)
    : AbstractDataSet<Driver, DriverFaker, DriversDataSet>(context)
{
    protected override DriverFaker CreateFaker() => 
        new DriverFaker(_context ?? throw new NullReferenceException());
}
