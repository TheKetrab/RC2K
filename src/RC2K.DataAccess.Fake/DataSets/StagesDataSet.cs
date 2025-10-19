
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class StagesDataSet(IDataContext? context = null)
    : AbstractDataSet<Stage, StageFaker, StagesDataSet>(context)
{
    protected override StageFaker CreateFaker() => new StageFaker();
}
