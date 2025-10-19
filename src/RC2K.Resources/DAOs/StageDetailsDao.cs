namespace RC2K.Resources.DAOs;

public class StageDetailsDao
{
    public StageDetailsDao(string code,
        string length, string asphalt, string dirt, string gravel, string snow,
        string temp, string wind, string mood)
    {
        Code = code;
        Length = length;
        Asphalt = asphalt;
        Dirt = dirt;
        Gravel = gravel;
        Snow = snow;
        Temp = temp;
        Wind = wind;
        Mood = mood;
    }

    public string Code { get; set; }
    public string Length { get; set; }
    public string Asphalt { get; set; }
    public string Dirt { get; set; }
    public string Gravel { get; set; }
    public string Snow { get; set; }
    public string Temp { get; set; }
    public string Wind { get; set; }
    public string Mood { get; set; }
}