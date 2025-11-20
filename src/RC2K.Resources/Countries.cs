using RC2K.DomainModel;
using RC2K.Resources.DAOs;
using System.Text;
using System.Text.Json;

namespace RC2K.Resources;

public class Countries
{

    private readonly static List<(string code, string name)> _countries = [];

    public static IEnumerable<(string code, string name)> GetCountries() => _countries;

    static Countries()
    {
        LoadCountries();
    }

    private static void LoadCountries()
    {
        var assembly = typeof(Countries).Assembly;
        var resourceStream = assembly.GetManifestResourceStream("RC2K.Resources.Properties.countries.json")
            ?? throw new InvalidDataException("Cannot find countries.json embeded resource");

        string countriesJson = string.Empty;
        using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
        {
            countriesJson = reader.ReadToEnd();
        }
        var obj = JsonSerializer.Deserialize<List<CountryDao>>(countriesJson)
            ?? throw new InvalidDataException("Cannot read countries");
        _countries.AddRange(obj.Select(x => (code: x.code, name: x.name)));
    }
}
