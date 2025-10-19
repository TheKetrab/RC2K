
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class StagesWaypointsDataSet()
{
    public IQueryable<StageWaypoints> Generate(int n) => 
        new List<StageWaypoints>(n).AsQueryable();
}
