namespace RC2K.DomainModel;

public class StageDetails
{
    public int StageCode { get; set; }
    public float Length { get; set; }
    public int Asphalt { get; set; }
    public int Dirt { get; set; }
    public int Gravel { get; set; }
    public int Snow { get; set; }
    public int Temp { get; set; }
    public float Wind { get; set; }
    public Mood Mood { get; set; }
}
