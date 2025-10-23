using RC2K.Extensions;

namespace RC2K.Resources.DAOs;

public class StageWaypointsDao
{
    public StageWaypointsDao(string code, string apiHint, string waypoints)
    {
        Code = code;
        ApiHint = apiHint;
        Waypoints = waypoints;
    }

    public string Code { get; set; }
    public string ApiHint { get; set; }
    public string Waypoints { get; set; }
}