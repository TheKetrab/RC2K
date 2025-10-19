
using Bogus;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Fakers;

public class StageWaypointsFaker : Faker<StageWaypoints>
{
    private int _id = 1;

    public StageWaypointsFaker()
    {
        RuleFor(x => x.Id, f => _id++);
        RuleFor(x => x.Waypoints, RandomCoord);
    }

    private string RandomCoord(Faker f) =>
        string.Join(";", Enumerable.Range(0, f.Random.Number(5, 15))
            .Select(i => $"{f.Random.Double(-50,50)},{f.Random.Double(-50, 50)}"));
    
}
