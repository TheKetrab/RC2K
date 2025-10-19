
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class StagesDataDataSet(IDataContext? context = null)
    : AbstractDataSet<StageData, StageDataFaker, StagesDataDataSet>(context)
{
    protected override StageDataFaker CreateFaker() => new StageDataFaker();
}
