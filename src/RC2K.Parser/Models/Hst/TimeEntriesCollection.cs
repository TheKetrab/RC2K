
namespace RC2K.Parser.Models.Hst;

public class TimeEntriesCollection
{
    private static Dictionary<long, int> _offsets = new Dictionary<long, int> {

        // note: -8, because offsets were counted missly, two integers were missed

        // S6, normal -> 
        { (long)(-8 + 0xc2a0 + 0 * 1560), 21 },
        { (long)(-8 + 0xc2a0 + 1 * 1560), 22 },
        { (long)(-8 + 0xc2a0 + 2 * 1560), 23 },
        { (long)(-8 + 0xc2a0 + 3 * 1560), 24 },
        { (long)(-8 + 0xc2a0 + 4 * 1560), 25 },
        { (long)(-8 + 0xc2a0 + 5 * 1560), 26 },

        // S5, normal
        { (long)(-8 + 0x9e10 + 0 * 1560), 51 },
        { (long)(-8 + 0x9e10 + 1 * 1560), 52 },
        { (long)(-8 + 0x9e10 + 2 * 1560), 53 },
        { (long)(-8 + 0x9e10 + 3 * 1560), 54 },
        { (long)(-8 + 0x9e10 + 4 * 1560), 55 },
        { (long)(-8 + 0x9e10 + 5 * 1560), 56 },

        // S4, normal
        { (long)(-8 + 0x7980 + 0 * 1560), 71 },
        { (long)(-8 + 0x7980 + 1 * 1560), 72 },
        { (long)(-8 + 0x7980 + 2 * 1560), 73 },
        { (long)(-8 + 0x7980 + 3 * 1560), 74 },
        { (long)(-8 + 0x7980 + 4 * 1560), 75 },
        { (long)(-8 + 0x7980 + 5 * 1560), 76 },

        // S3, normal
        { (long)(-8 + 0x54f0 + 0 * 1560), 31 },
        { (long)(-8 + 0x54f0 + 1 * 1560), 32 },
        { (long)(-8 + 0x54f0 + 2 * 1560), 33 },
        { (long)(-8 + 0x54f0 + 3 * 1560), 34 },
        { (long)(-8 + 0x54f0 + 4 * 1560), 35 },
        { (long)(-8 + 0x54f0 + 5 * 1560), 36 },

        // S2, normal
        { (long)(-8 + 0x3060 + 0 * 1560), 61 },
        { (long)(-8 + 0x3060 + 1 * 1560), 62 },
        { (long)(-8 + 0x3060 + 2 * 1560), 63 },
        { (long)(-8 + 0x3060 + 3 * 1560), 64 },
        { (long)(-8 + 0x3060 + 4 * 1560), 65 },
        { (long)(-8 + 0x3060 + 5 * 1560), 66 },

        // S1, normal
        { (long)(-8 + 0xbd0 + 0 * 1560), 41 },
        { (long)(-8 + 0xbd0 + 1 * 1560), 42 },
        { (long)(-8 + 0xbd0 + 2 * 1560), 43 },
        { (long)(-8 + 0xbd0 + 3 * 1560), 44 },
        { (long)(-8 + 0xbd0 + 4 * 1560), 45 },
        { (long)(-8 + 0xbd0 + 5 * 1560), 46 },

        // S1, tt
        { (long)(-8 + 0x2b3d0 + 0 * 1560), 41 },
        { (long)(-8 + 0x2b3d0 + 1 * 1560), 42 },
        { (long)(-8 + 0x2b3d0 + 2 * 1560), 43 },
        { (long)(-8 + 0x2b3d0 + 3 * 1560), 44 },
        { (long)(-8 + 0x2b3d0 + 4 * 1560), 45 },
        { (long)(-8 + 0x2b3d0 + 5 * 1560), 46 },

        // S2, tt
        { (long)(-8 + 0x2d860 + 0 * 1560), 61 },
        { (long)(-8 + 0x2d860 + 1 * 1560), 62 },
        { (long)(-8 + 0x2d860 + 2 * 1560), 63 },
        { (long)(-8 + 0x2d860 + 3 * 1560), 64 },
        { (long)(-8 + 0x2d860 + 4 * 1560), 65 },
        { (long)(-8 + 0x2d860 + 5 * 1560), 66 },

        // S3, tt
        { (long)(-8 + 0x2fcf0 + 0 * 1560), 31 },
        { (long)(-8 + 0x2fcf0 + 1 * 1560), 32 },
        { (long)(-8 + 0x2fcf0 + 2 * 1560), 33 },
        { (long)(-8 + 0x2fcf0 + 3 * 1560), 34 },
        { (long)(-8 + 0x2fcf0 + 4 * 1560), 35 },
        { (long)(-8 + 0x2fcf0 + 5 * 1560), 36 },

        // S4, tt
        { (long)(-8 + 0x32180 + 0 * 1560), 71 },
        { (long)(-8 + 0x32180 + 1 * 1560), 72 },
        { (long)(-8 + 0x32180 + 2 * 1560), 73 },
        { (long)(-8 + 0x32180 + 3 * 1560), 74 },
        { (long)(-8 + 0x32180 + 4 * 1560), 75 },
        { (long)(-8 + 0x32180 + 5 * 1560), 76 },

        // S5, tt
        { (long)(-8 + 0x34610 + 0 * 1560), 51 },
        { (long)(-8 + 0x34610 + 1 * 1560), 52 },
        { (long)(-8 + 0x34610 + 2 * 1560), 53 },
        { (long)(-8 + 0x34610 + 3 * 1560), 54 },
        { (long)(-8 + 0x34610 + 4 * 1560), 55 },
        { (long)(-8 + 0x34610 + 5 * 1560), 56 },

        // S6, tt
        { (long)(-8 + 0x36aa0 + 0 * 1560), 21 },
        { (long)(-8 + 0x36aa0 + 1 * 1560), 22 },
        { (long)(-8 + 0x36aa0 + 2 * 1560), 23 },
        { (long)(-8 + 0x36aa0 + 3 * 1560), 24 },
        { (long)(-8 + 0x36aa0 + 4 * 1560), 25 },
        { (long)(-8 + 0x36aa0 + 5 * 1560), 26 },

        // A1, normal
        { (long)(-8 + 0x1b3e0 + 0 * 1560), 21 },
        { (long)(-8 + 0x1b3e0 + 1 * 1560), 22 },
        { (long)(-8 + 0x1b3e0 + 2 * 1560), 23 },
        { (long)(-8 + 0x1b3e0 + 3 * 1560), 24 },
        { (long)(-8 + 0x1b3e0 + 4 * 1560), 25 },
        { (long)(-8 + 0x1b3e0 + 5 * 1560), 26 },

        // A2, normal
        { (long)(-8 + 0x18f50 + 0 * 1560), 51 },
        { (long)(-8 + 0x18f50 + 1 * 1560), 52 },
        { (long)(-8 + 0x18f50 + 2 * 1560), 53 },
        { (long)(-8 + 0x18f50 + 3 * 1560), 54 },
        { (long)(-8 + 0x18f50 + 4 * 1560), 55 },
        { (long)(-8 + 0x18f50 + 5 * 1560), 56 },

        // A3, normal
        { (long)(-8 + 0x16ac0 + 0 * 1560), 71 },
        { (long)(-8 + 0x16ac0 + 1 * 1560), 72 },
        { (long)(-8 + 0x16ac0 + 2 * 1560), 73 },
        { (long)(-8 + 0x16ac0 + 3 * 1560), 74 },
        { (long)(-8 + 0x16ac0 + 4 * 1560), 75 },
        { (long)(-8 + 0x16ac0 + 5 * 1560), 76 },

        // A4, normal
        { (long)(-8 + 0x14630 + 0 * 1560), 31 },
        { (long)(-8 + 0x14630 + 1 * 1560), 32 },
        { (long)(-8 + 0x14630 + 2 * 1560), 33 },
        { (long)(-8 + 0x14630 + 3 * 1560), 34 },
        { (long)(-8 + 0x14630 + 4 * 1560), 35 },
        { (long)(-8 + 0x14630 + 5 * 1560), 36 },

        // A5, normal
        { (long)(-8 + 0x121a0 + 0 * 1560), 61 },
        { (long)(-8 + 0x121a0 + 1 * 1560), 62 },
        { (long)(-8 + 0x121a0 + 2 * 1560), 63 },
        { (long)(-8 + 0x121a0 + 3 * 1560), 64 },
        { (long)(-8 + 0x121a0 + 4 * 1560), 65 },
        { (long)(-8 + 0x121a0 + 5 * 1560), 66 },

        // A6, normal
        { (long)(-8 + 0xfd10 + 0 * 1560), 41 },
        { (long)(-8 + 0xfd10 + 1 * 1560), 42 },
        { (long)(-8 + 0xfd10 + 2 * 1560), 43 },
        { (long)(-8 + 0xfd10 + 3 * 1560), 44 },
        { (long)(-8 + 0xfd10 + 4 * 1560), 45 },
        { (long)(-8 + 0xfd10 + 5 * 1560), 46 },

        // A1, tt
        { (long)(-8 + 0x28f40 + 0 * 1560), 21 },
        { (long)(-8 + 0x28f40 + 1 * 1560), 22 },
        { (long)(-8 + 0x28f40 + 2 * 1560), 23 },
        { (long)(-8 + 0x28f40 + 3 * 1560), 24 },
        { (long)(-8 + 0x28f40 + 4 * 1560), 25 },
        { (long)(-8 + 0x28f40 + 5 * 1560), 26 },

        // A2, tt
        { (long)(-8 + 0x26ab0 + 0 * 1560), 51 },
        { (long)(-8 + 0x26ab0 + 1 * 1560), 52 },
        { (long)(-8 + 0x26ab0 + 2 * 1560), 53 },
        { (long)(-8 + 0x26ab0 + 3 * 1560), 54 },
        { (long)(-8 + 0x26ab0 + 4 * 1560), 55 },
        { (long)(-8 + 0x26ab0 + 5 * 1560), 56 },

        // A3, tt
        { (long)(-8 + 0x24620 + 0 * 1560), 71 },
        { (long)(-8 + 0x24620 + 1 * 1560), 72 },
        { (long)(-8 + 0x24620 + 2 * 1560), 73 },
        { (long)(-8 + 0x24620 + 3 * 1560), 74 },
        { (long)(-8 + 0x24620 + 4 * 1560), 75 },
        { (long)(-8 + 0x24620 + 5 * 1560), 76 },

        // A4, tt
        { (long)(-8 + 0x22190 + 0 * 1560), 31 },
        { (long)(-8 + 0x22190 + 1 * 1560), 32 },
        { (long)(-8 + 0x22190 + 2 * 1560), 33 },
        { (long)(-8 + 0x22190 + 3 * 1560), 34 },
        { (long)(-8 + 0x22190 + 4 * 1560), 35 },
        { (long)(-8 + 0x22190 + 5 * 1560), 36 },

        // A5, tt
        { (long)(-8 + 0x1fd00 + 0 * 1560), 61 },
        { (long)(-8 + 0x1fd00 + 1 * 1560), 62 },
        { (long)(-8 + 0x1fd00 + 2 * 1560), 63 },
        { (long)(-8 + 0x1fd00 + 3 * 1560), 64 },
        { (long)(-8 + 0x1fd00 + 4 * 1560), 65 },
        { (long)(-8 + 0x1fd00 + 5 * 1560), 66 },

        // A6, tt
        { (long)(-8 + 0x1d870 + 0 * 1560), 41 },
        { (long)(-8 + 0x1d870 + 1 * 1560), 42 },
        { (long)(-8 + 0x1d870 + 2 * 1560), 43 },
        { (long)(-8 + 0x1d870 + 3 * 1560), 44 },
        { (long)(-8 + 0x1d870 + 4 * 1560), 45 },
        { (long)(-8 + 0x1d870 + 5 * 1560), 46 },

    };

    public bool IsNormal { get; }
    public bool IsArcade { get; }
    public StageEntry[] StageEntries { get; }

    public TimeEntriesCollection(bool isNormal, bool isArcade, BinaryReader reader)
    {
        IsNormal = isNormal;
        IsArcade = isArcade;
        StageEntries = new StageEntry[36];

        for (int i = 0; i < 36; i++)
        {
            if (!_offsets.ContainsKey(reader.BaseStream.Position))
            {
                throw new InvalidOperationException();
            }

            int stageCode = _offsets[reader.BaseStream.Position];
            StageEntries[i] = new StageEntry(stageCode, isNormal, isArcade, reader);
        }
    }

    public IEnumerable<TimeEntry> GetAll()
    {
        for (int i=0; i<36; i++)
        {
            foreach (var  entry in StageEntries[i].GetAll())
            {
                yield return entry;
            }
        }
    }
}
