using System.Text;
using static RC2K.Parser.Utils;

namespace RC2K.Parser.Models.Hst;

public class SimulationScore
{
    const int NameMaxBytes = 32;

    public int F1 { get; set; }
    public int F2 { get; set; }
    public int Nat { get; set; }
    public int Car { get; set; }
    public int F5 { get; set; }
    public int F6 { get; set; }
    public string Name { get; set; }
    public int Points { get; set; }

    public SimulationScore(BinaryReader reader)
    {
        F1 = reader.ReadInt32();
        F2 = reader.ReadInt32();
        Nat = reader.ReadInt32();
        Car = reader.ReadInt32();
        F5 = reader.ReadInt32();
        F6 = reader.ReadInt32();

        byte[] stringBytes = reader.ReadBytes(NameMaxBytes);
        Name = Encoding.ASCII.GetString(stringBytes).Trim('\0');
        
        Points = reader.ReadInt32();
    }

    public override string ToString() => $"{GetNat(Nat)} {GetCar(Car)} {Name} ({Points})";
}
