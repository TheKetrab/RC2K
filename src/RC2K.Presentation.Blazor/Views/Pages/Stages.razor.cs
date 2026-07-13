using Microsoft.AspNetCore.Components;
using MudBlazor;
using RC2K.DomainModel;
using RC2K.Utils;

namespace RC2K.Presentation.Blazor.Views.Pages;

public partial class Stages
{
    [Parameter] 
    public string? Name { get; set; }

    private List<Stage> _stages = new();
    private RallyCode? _rallyCode;
    private IList<double[]> _allWaypoints = new List<double[]>();
    private double[][][]? _startsEnds;
    private string[]? _combinedPaths;

    // The fix is here: Cast the result to (object)
    private readonly TableGroupDefinition<Stage> _groupDefinition = new()
    {
        Selector = (x) => (object)$"{LevelHelper.StageCodeToRallyName(x.Code)} ({x.Direction})",
        IsInitiallyExpanded = true
    };

    private string GetRallyFullName(string name)
    {
        if (string.IsNullOrEmpty(Name) || !Enum.TryParse<RallyCode>(Name, true, out var code))
        {
            return "";
        }

        return RC2K.Utils.LevelHelper.RallyCodeToRallyName(code);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (!string.IsNullOrEmpty(Name) && Enum.TryParse<RallyCode>(Name, true, out var code))
        {
            _rallyCode = code;
        }

        var stages = (Name is null)
            ? await StageService.GetAll()
            : await StageService.GetAllByRallyCode(_rallyCode!.Value);

        _stages = stages.ToList();

        // Load all waypoints and paths for the stages
        await LoadMapData();
    }

    private async Task LoadMapData()
    {
        var allWaypointsList = new List<double[]>();
        var allPaths = new List<string>();

        int cnt = Name is null ? 36 : 6;
        _startsEnds = new double[cnt][][];

        int i = 0;
        foreach (var stage in _stages.DistinctBy(x => x.Code))
        {
            _startsEnds[i] = new double[2][];

            var waypoints = await StageService.GetWaypoints(stage.Code, stage.Direction == DomainModel.Direction.Arcade);
            _startsEnds[i][0] = waypoints[0];
            _startsEnds[i++][1] = waypoints[^1];
            foreach (var waypoint in waypoints)
            {
                allWaypointsList.Add(waypoint);
            }

            var path = await StageService.GetPath(stage.Code);
            if (!string.IsNullOrEmpty(path))
            {
                allPaths.Add(path);
            }
        }

        _allWaypoints = allWaypointsList;

        // Combine all paths - assuming they are GeoJSON format, merge them into a FeatureCollection
        if (allPaths.Count > 0)
        {
            _combinedPaths = allPaths.ToArray();
        }
    }

    private static IEnumerable<string> GetMoods(Mood? moodFlags)
    {
        if (moodFlags == null) yield break;
        if (moodFlags.Value.HasFlag(Mood.Sunrise)) yield return "Sunrise";
        if (moodFlags.Value.HasFlag(Mood.Day)) yield return "Day";
        if (moodFlags.Value.HasFlag(Mood.Sunset)) yield return "Sunset";
        if (moodFlags.Value.HasFlag(Mood.Night)) yield return "Night";
    }

    private static Color GetMoodColor(string mood) => mood switch
    {
        "Sunrise" => Color.Warning,
        "Day" => Color.Info,
        "Sunset" => Color.Secondary,
        "Night" => Color.Dark,
        _ => Color.Default
    };
}