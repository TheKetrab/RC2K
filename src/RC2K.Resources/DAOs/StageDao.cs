namespace RC2K.Resources.DAOs;

public class StageDao
{
    public StageDao(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; set; }
    public string Name { get; set; }
}