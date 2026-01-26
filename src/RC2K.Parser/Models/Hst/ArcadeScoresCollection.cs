namespace RC2K.Parser.Models.Hst;

public class ArcadeScoresCollection
{
    public ArcadeScore[] Arcade1 { get; set; } 
    public ArcadeScore[] Arcade2 { get; set; } 
    public ArcadeScore[] Arcade3 { get; set; } 
    public ArcadeScore[] Arcade4 { get; set; } 
    public ArcadeScore[] Arcade5 { get; set; } 
    public ArcadeScore[] Arcade6 { get; set; } 
    public ArcadeScore[] Unknown { get; set; } 

    public ArcadeScoresCollection(BinaryReader reader)
    {
        Arcade1 = new ArcadeScore[10];
        for (int i = 0; i < 10; i++)
        {
            Arcade1[i] = new ArcadeScore(reader);
        }

        Arcade2 = new ArcadeScore[10];
        for (int i = 0; i < 10; i++)
        {
            Arcade2[i] = new ArcadeScore(reader);
        }

        Arcade3 = new ArcadeScore[10];
        for (int i = 0; i < 10; i++)
        {
            Arcade3[i] = new ArcadeScore(reader);
        }

        Arcade4 = new ArcadeScore[10];
        for (int i = 0; i < 10; i++)
        {
            Arcade4[i] = new ArcadeScore(reader);
        }

        Arcade5 = new ArcadeScore[10];
        for (int i = 0; i < 10; i++)
        {
            Arcade5[i] = new ArcadeScore(reader);
        }

        Arcade6 = new ArcadeScore[10];
        for (int i = 0; i < 10; i++)
        {
            Arcade6[i] = new ArcadeScore(reader);
        }

        Unknown = new ArcadeScore[10];
        for (int i = 0; i < 10; i++)
        {
            Unknown[i] = new ArcadeScore(reader);
        }
    }
}
