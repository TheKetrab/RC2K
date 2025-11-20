namespace RC2K.Resources.DAOs;

public class CountryDao
{
    public CountryDao(string code, string name)
    {
        this.code = code;
        this.name = name;
    }

    public string code { get; set; }
    public string name { get; set; }
}