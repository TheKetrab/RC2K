using System.Text;
using static RC2K.Parser.Utils;

namespace RC2K.Parser.Models.Hst;

public class ArcadeScore
{
    const int NameMaxBytes = 32;

    public int F1 { get; set; }
    public int F2 { get; set; }
    public int Nat { get; set; }
    public int F4 { get; set; }
    public int Car { get; set; }
    public int Points { get; set; }
    public int F7 { get; set; }
    public int F8 { get; set; }
    public string Name { get; set; }
    public int F10 { get; set; }
    public int F11 { get; set; }
    public int F12 { get; set; }
    public int F13 { get; set; }

    public ArcadeScore(BinaryReader reader)
    {
        F1 = reader.ReadInt32();
        F2 = reader.ReadInt32();
        Nat = reader.ReadInt32();
        F4 = reader.ReadInt32();
        Car = reader.ReadInt32();
        Points = reader.ReadInt32();
        F7 = reader.ReadInt32();
        F8 = reader.ReadInt32();

        byte[] stringBytes = reader.ReadBytes(NameMaxBytes);
        Name = Encoding.ASCII.GetString(stringBytes).Trim('\0');

        F10 = reader.ReadInt32();
        F11 = reader.ReadInt32();
        F12 = reader.ReadInt32();
        F13 = reader.ReadInt32();

    }

    public override string ToString() => $"{GetNat(Nat)} {GetCar(Car)} {Name} ({Points})";
}
