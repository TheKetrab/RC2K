using RC2K.Parser.Models.Hst;
using System.Text;

namespace RC2K.Parser;

public class HstWriter
{
    private readonly Func<int> _zeroCar;
    private readonly Func<int> _zeroNat;
    private readonly Func<string> _zeroName;

    public HstWriter(Func<int> zeroCar, Func<int> zeroNat, Func<string> zeroName)
    {
        _zeroCar = zeroCar;
        _zeroNat = zeroNat;
        _zeroName = zeroName;
    }

    public void ShredTimeEntries(Stream stream, IEnumerable<TimeEntry> entries)
    {
        using BinaryWriter writer = new(stream);

        foreach (var entry in entries)
        {
            writer.BaseStream.Seek(entry.ByteOffset, SeekOrigin.Begin);
            WriteZeroTimeEntry(writer);
        }
    }

    private void WriteZeroTimeEntry(BinaryWriter writer)
    {
        const int twentyMin = 20 * 60 * 100;

        writer.Write(0); // F1
        writer.Write(0); // F2

        writer.Write(_zeroNat()); // Nat

        writer.Write(0); // F4

        writer.Write(_zeroCar()); // Car

        writer.Write(twentyMin); // Centiseconds
        writer.Write(0); // F7
        writer.Write(0); // F8

        // Name - 32 bytes
        string name = _zeroName();
        if (name.Length >= 32)
        {
            throw new ArgumentException($"Name must be shorter than 32 chars, but was: {name}");
        }

        byte[] stringBytes = Encoding.ASCII.GetBytes(name);
        writer.Write(stringBytes);
        int padZeros = 32 - (stringBytes.Length);
        if (padZeros > 0)
        {
            writer.Write(new byte[padZeros]);
        }

        writer.Write(0); // F10
        writer.Write(0); // F11
        writer.Write(0); // F12
        writer.Write(0); // F13
        writer.Write(0); // F14

        writer.Write(0L); // Guid, 8 bytes

        // CheckpointsCentiseconds
        for (int i = 0; i < 16; i++)
        {
            writer.Write(0);
        }
    }
}
