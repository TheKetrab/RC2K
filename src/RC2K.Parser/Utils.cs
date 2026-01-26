namespace RC2K.Parser;

public class Utils
{
    public static Dictionary<int, string> Nat = new()
    {
        {0x1E, "PL" }
    };
    public static Dictionary<int, string> Car = new()
    {
        {0, "Renault" },
        {1, "Vauxhall" },
        {2, "Seat A8" },
        {3, "Hyundai" },
        {4, "Subaru" },
        {5, "Peugeot 106" },
        {6, "MF" },
        {7, "Evo V" },
        {8, "Golf" },
        {9, "Almera" },
        {10, "Wira" },
        {11, "Saxo" },
        {12, "Honda" },
        {13, "Micra" },
        {14, "Peugeot 206" },
        {15, "Ford Escord" },
        {16, "Scoda Octavia A7" },
        {17, "Seat Ibiza" },
        {18, "Skoda A6" },
        {19, "Proton A6" },
        {20, "Ford Gold" },
        {21, "For Puma" },
        {22, "Evo IV" },
    };


    public static string GetNat(int id) =>
        Nat.TryGetValue(id, out var nat) ? nat : id.ToString();

    public static string GetCar(int id) =>
        Car.TryGetValue(id, out var car) ? car : id.ToString();

}
