namespace RC2K.Resources.DAOs;

public class CarDao
{
    public CarDao(string name, string className)
    {
        Name = name;
        ClassName = className;
    }

    public string Name { get; set; }
    public string ClassName { get; set; }
}