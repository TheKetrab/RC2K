using RC2K.Extensions;

namespace RC2K.Resources.DAOs;

public class StageWaypointsDao
{
    public StageWaypointsDao(string code, string waypoints)
    {
        Code = code;
        Waypoints = waypoints;
    }

    public string Code { get; set; }
    public string Waypoints { get; set; }
}