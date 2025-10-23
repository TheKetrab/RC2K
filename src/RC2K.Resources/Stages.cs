using RC2K.Resources.DAOs;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace RC2K.Resources
{
    public static class Stages
    {
        private readonly static List<StageDao> _stages = [];
        private readonly static List<StageWaypointsDao> _waypoints = [];
        private readonly static List<StageDetailsDao> _stagesDetails = [];
        private readonly static List<RallyInfoDao> _rallyInfos = [];
        
        static Stages()
        {
            LoadRallyInfos();
            LoadStagesData();
            LoadStagesWaypoints();
            LoadStagesDetails();
        }

        public static IReadOnlyList<StageDao> GetStages() => _stages;
        public static IReadOnlyList<StageWaypointsDao> GetWaypoints() => _waypoints;
        public static IReadOnlyList<StageDetailsDao> GetStageDetails() => _stagesDetails;
        public static IReadOnlyList<RallyInfoDao> GetRallyInfos() => _rallyInfos;

        private static void LoadStagesData()
        {
            string stagesCsv = Properties.Resources.Stages;
            using StringReader reader = new(stagesCsv);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split(',');
                _stages.Add(new(split[0], split[1]));
            }
        }

        private static void LoadStagesWaypoints()
        {
            string stagesWaypointsCsv = Properties.Resources.StagesWaypoints;
            using StringReader reader = new(stagesWaypointsCsv);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                int comma1 = line.IndexOf(',');
                int comma2 = line.IndexOf(',', comma1 + 1);

                string code = line[..comma1];
                if (comma2 == -1)
                {
                    _waypoints.Add(new(code, "", ""));
                    continue;
                }

                string apiHint = line[(comma1+1)..comma2];

                string coords = line.Length > comma2 + 1
                    ? line[(comma2 + 2)..^1] // skip quotes
                    : "";

                _waypoints.Add(new(code, apiHint, coords));
            }
        }

        private static void LoadStagesDetails()
        {
            string stagesDetailsCsv = Properties.Resources.StagesDetails;
            using StringReader reader = new(stagesDetailsCsv);

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] split = line.Split(",");
                _stagesDetails.Add(new(split[0], split[1], split[2], split[3], split[4], split[5], split[6], split[7], split[8]));
            }
        }


        private static void LoadRallyInfos()
        {
            var assembly = typeof(Stages).Assembly;
            var resourceStream = assembly.GetManifestResourceStream("RC2K.Resources.Properties.stages-info.json") 
                ?? throw new InvalidDataException("Cannot find stages-info.json embeded resource");

            string stagesInfoJson = string.Empty;
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                stagesInfoJson =  reader.ReadToEnd();
            }
            var obj = JsonSerializer.Deserialize<List<RallyInfoDao>>(stagesInfoJson)
                ?? throw new InvalidDataException("Cannot read Rally Info");
            _rallyInfos.AddRange(obj);
        }
    }
}
