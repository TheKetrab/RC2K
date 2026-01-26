namespace RC2K.Parser.Models.Hst;

public class Header
{
    public int R5HS { get; set; }
    public int F2 { get; set; }
    public int F3 { get; set; }
    public int F4 { get; set; }

    public Header(BinaryReader reader)
    {
        R5HS = reader.ReadInt32();
        F2 = reader.ReadInt32();
        F3 = reader.ReadInt32();
        F4 = reader.ReadInt32();
    }

    public override string ToString() => $"R5HS {F2} {F3} {F4}";
}
