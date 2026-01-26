namespace RC2K.Parser.Models.Hst;

public class SimulationScoresCollection
{
    public SimulationScore[] A8Championship { get; set; }
    public SimulationScore[] A7BRC { get; set; }
    public SimulationScore[] A6BRC { get; set; }
    public SimulationScore[] A5BRC { get; set; }
    public SimulationScore[] GlobalBRC { get; set; }

    public SimulationScoresCollection(BinaryReader reader)
    {
        A8Championship = new SimulationScore[10];
        for (int i = 0; i < 10; i++)
        {
            A8Championship[i] = new SimulationScore(reader);
        }

        A7BRC = new SimulationScore[10];
        for (int i = 0; i < 10; i++)
        {
            A7BRC[i] = new SimulationScore(reader);
        }

        A6BRC = new SimulationScore[10];
        for (int i = 0; i < 10; i++)
        {
            A6BRC[i] = new SimulationScore(reader);
        }

        A5BRC = new SimulationScore[10];
        for (int i = 0; i < 10; i++)
        {
            A5BRC[i] = new SimulationScore(reader);
        }

        GlobalBRC = new SimulationScore[10];
        for (int i = 0; i < 10; i++)
        {
            GlobalBRC[i] = new SimulationScore(reader);
        }
    }
}
