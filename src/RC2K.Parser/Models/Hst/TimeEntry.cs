
using System.Text;

namespace RC2K.Parser.Models.Hst;

public class TimeEntry
{
    public StageEntry Parent { get; }
    public int F1 { get; set; }
    public int F2 { get; set; }
    public int Nat { get; set; }
    public int F4 { get; set; }
    public int Car { get; set; }
    public int Centiseconds { get; set; }
    public int F7 { get; set; }
    public int F8 { get; set; }
    public string Name { get; set; }
    public int F10 { get; set; }
    public int F11 { get; set; }
    public int F12 { get; set; }
    public int F13 { get; set; }
    public int F14 { get; set; }
    public string HexGuid { get; set; }
    public int[] CheckpointsCentiseconds { get; set; }

   
    public string Time => TimeSpan.FromMilliseconds(Centiseconds * 10).ToString(@"m\:ss\.ff");
    public string CheckpointTime(int i) => TimeSpan.FromMilliseconds(CheckpointsCentiseconds[i] * 10).ToString(@"m\:ss\.ff");

    public override string ToString() => $"s: {Parent.StageCode,-15} c:{Car,2} {HexGuid} {Name,20} => {Time}";

    public TimeEntry(StageEntry parent, BinaryReader reader)
    {
        Parent = parent;

        F1 = reader.ReadInt32();
        F2 = reader.ReadInt32();
        Nat = reader.ReadInt32();
        F4 = reader.ReadInt32();
        Car = reader.ReadInt32();
        Centiseconds = reader.ReadInt32();
        F7 = reader.ReadInt32();
        F8 = reader.ReadInt32();

        byte[] stringBytes = reader.ReadBytes(32);
        Name = Encoding.ASCII.GetString(stringBytes).Trim('\0');

        F10 = reader.ReadInt32();
        F11 = reader.ReadInt32();
        F12 = reader.ReadInt32();
        F13 = reader.ReadInt32();
        F14 = reader.ReadInt32();

        byte[] guidBytes = reader.ReadBytes(8);
        HexGuid = string.Join('-', BitConverter.ToString(guidBytes).Replace("-", "").Split("",4));

        CheckpointsCentiseconds = new int[16];
        for (int i = 0; i < 16; i++)
        {
            CheckpointsCentiseconds[i] = reader.ReadInt32();
        }
    }
}
