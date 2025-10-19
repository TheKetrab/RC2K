
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class CarsDataSet(IDataContext? context = null)
    : AbstractDataSet<Car, CarFaker, CarsDataSet>(context)
{
    protected override CarFaker CreateFaker() => new CarFaker();
}
