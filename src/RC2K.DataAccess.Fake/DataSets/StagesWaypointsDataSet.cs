
using RC2K.DataAccess.Fake.Fakers;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class StagesWaypointsDataSet(IDataContext? context = null)
    : AbstractDataSet<StageWaypoints, StageWaypointsFaker, StagesWaypointsDataSet>(context)
{
    protected override StageWaypointsFaker CreateFaker() => new StageWaypointsFaker();
}
