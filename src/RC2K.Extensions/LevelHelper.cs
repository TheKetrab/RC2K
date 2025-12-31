using RC2K.DomainModel;

namespace RC2K.Extensions;

public static class LevelHelper
{
    private readonly static IReadOnlyDictionary<int, int> _arcadeId2CodePrefixMap =
        new Dictionary<int, int>()
        {
            { 1, 2 },
            { 2, 5 },
            { 3, 7 },
            { 4, 3 },
            { 5, 6 },
            { 6, 4 },
        };

    private readonly static IReadOnlyDictionary<RallyCode, int> _rallyCode2CodePrefixMap =
        new Dictionary<RallyCode, int>()
        {
            { RallyCode.Sony, 2 },
            { RallyCode.Stena, 5 },
            { RallyCode.Seat, 7 },
            { RallyCode.Scottish, 3 },
            { RallyCode.Pirelli, 6 },
            { RallyCode.Vauxhall, 4 },
        };

    private readonly static IReadOnlyDictionary<RallyCode, string> _rallyCode2RallyNameMap =
        new Dictionary<RallyCode, string>()
        {
            { RallyCode.Sony, "SONY Manx International Rally" },
            { RallyCode.Stena, "Stena Line Ulster Rally" },
            { RallyCode.Seat, "SEAT Jim Clark Memorial Rally" },
            { RallyCode.Scottish, "Scottish Rally RSAC" },
            { RallyCode.Pirelli, "Pirelli International Rally" },
            { RallyCode.Vauxhall, "Vauxhall Rally of Wales" }
        };

    private readonly static IReadOnlyDictionary<RallyCode, string> _rallyCode2shortName =
        new Dictionary<RallyCode, string>()
        {
            { RallyCode.Sony, "sony" },
            { RallyCode.Stena, "stena" },
            { RallyCode.Seat, "seat" },
            { RallyCode.Scottish, "scottish" },
            { RallyCode.Pirelli, "pirelli" },
            { RallyCode.Vauxhall, "vauxhall" }
        };
    
    private readonly static IReadOnlyDictionary<int, RallyCode> _codePrefix2RallyCodeMap =
        new Dictionary<int, RallyCode>(_rallyCode2CodePrefixMap.ToDictionary(x => x.Value, x => x.Key));

    public static int GetStageCode(int rally, int stage, bool arcade) =>
        stage + 10 * (arcade ? _arcadeId2CodePrefixMap[rally] : rally);

    public static int GetStageCode(RallyCode rallyCode, int stage) =>
        stage + 10 * _rallyCode2CodePrefixMap[rallyCode];

    public static RallyCode GetRallyCodeByStageCode(int stage) =>
        _codePrefix2RallyCodeMap[stage / 10];

    public static string RallyCodeToRallyName(RallyCode rallyCode) =>
        _rallyCode2RallyNameMap[rallyCode];

    public static string RallyCodeToRallyShortName(RallyCode rallyCode) =>
        _rallyCode2shortName[rallyCode];
}
