using RC2K.Parser.Models.Hst;

namespace RC2K.Parser;

public interface IHstReader
{
    IEnumerable<TimeEntry> GetEntries(Stream stream, string name);
}

public class HstReader : IHstReader
{
    public IEnumerable<TimeEntry> GetEntries(Stream stream, string name)
    {
        using BinaryReader reader = new(stream);
        HighScores hs = new HighScores(reader);
        return hs.GetAll()
            .Where(x => !string.IsNullOrEmpty(x.HexGuid) && x.HexGuid != "0000000000000000" && x.Name == name);
    }
}
