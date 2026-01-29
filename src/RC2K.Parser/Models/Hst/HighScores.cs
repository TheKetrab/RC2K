namespace RC2K.Parser.Models.Hst;

public class HighScores
{
    public Header Header { get; set; }
    public SimulationScoresCollection SimulationScores { get; set; }
    public TimeEntriesCollection TimesSimulationNormal { get; set; }
    public ArcadeScoresCollection ArcadeScores { get; set; }
    public TimeEntriesCollection TimesArcadeNormal { get; set; }
    public TimeEntriesCollection TimesArcadeTimeTrial { get; set; }
    public TimeEntriesCollection TimesSimulationTimeTrial { get; set; }


    public HighScores(BinaryReader reader)
    {
        Header = new Header(reader);
        SimulationScores = new SimulationScoresCollection(reader);
        TimesSimulationNormal = new TimeEntriesCollection(isNormal: true, isArcade: false, reader);
        ArcadeScores = new ArcadeScoresCollection(reader);
        TimesArcadeNormal = new TimeEntriesCollection(isNormal: true, isArcade: true, reader);
        TimesArcadeTimeTrial = new TimeEntriesCollection(isNormal: false, isArcade: true, reader);
        TimesSimulationTimeTrial = new TimeEntriesCollection(isNormal: false, isArcade: false, reader);
    }

    public IEnumerable<TimeEntry> GetAll()
    {
        foreach (var te in TimesSimulationNormal.GetAll()) yield return te;
        foreach (var te in TimesArcadeNormal.GetAll()) yield return te;
        foreach (var te in TimesArcadeTimeTrial.GetAll()) yield return te;
        foreach (var te in TimesSimulationTimeTrial.GetAll()) yield return te;
    }
}
