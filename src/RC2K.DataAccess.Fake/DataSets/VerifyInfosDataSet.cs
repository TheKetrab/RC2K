
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class VerifyInfosDataSet(IDataContext? context = null)
    : AbstractDataSet<VerifyInfo, VerifyInfoFaker, VerifyInfosDataSet>(context)
{
    protected override VerifyInfoFaker CreateFaker() 
        => new VerifyInfoFaker(_context ?? throw new NullReferenceException());
}
