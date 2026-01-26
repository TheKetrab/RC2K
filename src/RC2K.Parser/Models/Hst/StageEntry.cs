
namespace RC2K.Parser.Models.Hst;

public class StageEntry
{
    public int StageCode { get; }
    public bool IsNormal { get; } // t->Normal / f->TimeTrial
    public bool IsArcade { get; } // t->Arcade / f->Simulation

    public TimeEntry[] TimeEntries { get; }

    public StageEntry(int stageCode, bool isNormal, bool isArcade, BinaryReader reader)
    {
        StageCode = stageCode;
        IsNormal = isNormal;
        IsArcade = isArcade;

        TimeEntries = new TimeEntry[10];
        for (int i=0; i<10; i++)
        {
            TimeEntries[i] = new TimeEntry(this, reader);
        }

    }

    public IEnumerable<TimeEntry> GetAll()
    {
        // returns from the best to avoid uploading many with the same car
        for (int i=0; i<10; i++)
        {
            yield return TimeEntries[i];
        }
    }
}
