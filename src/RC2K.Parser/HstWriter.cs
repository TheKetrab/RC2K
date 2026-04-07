namespace RC2K.Parser;

public static class HstWriter
{
    /// <summary>
    /// Zeros the Centiseconds field at the given byte offset in the raw file bytes.
    /// </summary>
    public static void ZeroEntry(byte[] fileBytes, long centisecondsOffset)
    {
        BitConverter.GetBytes(0).CopyTo(fileBytes, centisecondsOffset);
    }

    /// <summary>
    /// Saves the modified byte array back to a file.
    /// </summary>
    public static void Save(string filePath, byte[] fileBytes)
    {
        File.WriteAllBytes(filePath, fileBytes);
    }
}
