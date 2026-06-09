using RC2K.DomainModel;
using System.Collections.Frozen;
using System.Collections.ObjectModel;

namespace RC2K.Utils;

public static class LevelHelper
{
    private readonly static FrozenDictionary<int, int> _arcadeId2CodePrefixMap =
        new Dictionary<int, int>()
        {
            { 1, 2 },
            { 2, 5 },
            { 3, 7 },
            { 4, 3 },
            { 5, 6 },
            { 6, 4 },
        }.ToFrozenDictionary();

    private readonly static FrozenDictionary<RallyCode, int> _rallyCode2CodePrefixMap =
        new Dictionary<RallyCode, int>()
        {
            { RallyCode.Sony, 2 },
            { RallyCode.Stena, 5 },
            { RallyCode.Seat, 7 },
            { RallyCode.Scottish, 3 },
            { RallyCode.Pirelli, 6 },
            { RallyCode.Vauxhall, 4 },
        }.ToFrozenDictionary();

    private readonly static FrozenDictionary<RallyCode, string> _rallyCode2RallyNameMap =
        new Dictionary<RallyCode, string>()
        {
            { RallyCode.Sony, "SONY Manx International Rally" },
            { RallyCode.Stena, "Stena Line Ulster Rally" },
            { RallyCode.Seat, "SEAT Jim Clark Memorial Rally" },
            { RallyCode.Scottish, "Scottish Rally RSAC" },
            { RallyCode.Pirelli, "Pirelli International Rally" },
            { RallyCode.Vauxhall, "Vauxhall Rally of Wales" }
        }.ToFrozenDictionary();

    private readonly static FrozenDictionary<RallyCode, string> _rallyCode2shortName =
        new Dictionary<RallyCode, string>()
        {
            { RallyCode.Sony, "sony" },
            { RallyCode.Stena, "stena" },
            { RallyCode.Seat, "seat" },
            { RallyCode.Scottish, "scottish" },
            { RallyCode.Pirelli, "pirelli" },
            { RallyCode.Vauxhall, "vauxhall" }
        }.ToFrozenDictionary();

    private readonly static FrozenDictionary<RallyCode, string> _rallyCode2imageName =
        new Dictionary<RallyCode, string>()
        {
                { RallyCode.Sony, "WELMNX" },
                { RallyCode.Stena, "WELUL" },
                { RallyCode.Seat, "WELJC" },
                { RallyCode.Scottish, "WELSCT" },
                { RallyCode.Pirelli, "WELPIR" },
                { RallyCode.Vauxhall, "WELVX" }
        }.ToFrozenDictionary();

    private readonly static FrozenDictionary<int, RallyCode> _codePrefix2RallyCodeMap =
        new Dictionary<int, RallyCode>(_rallyCode2CodePrefixMap.ToDictionary(x => x.Value, x => x.Key)).ToFrozenDictionary();

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

    public static string RallyCodeToRallyImageName(RallyCode rallyCode) =>
        _rallyCode2imageName[rallyCode];

    public static string StageCodeToRallyShortName(int stageCode) =>
        RallyCodeToRallyShortName(GetRallyCodeByStageCode(stageCode));

    public static string StageCodeToRallyName(int stageCode) =>
        RallyCodeToRallyName(GetRallyCodeByStageCode(stageCode));

    public static string StageCodeToRallyImageName(int stageCode) =>
        RallyCodeToRallyImageName(GetRallyCodeByStageCode(stageCode));
}
