
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class StagesDataDataSet()
{
    public IQueryable<StageData> Generate(int n) =>
        new List<StageData>(n).AsQueryable();
}
