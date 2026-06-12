using RC2K.DomainModel;

namespace RC2K.IntegrationTests.Helpers;

public class DriverValueComparer : IEqualityComparer<Driver>
{
    public bool Equals(Driver? x, Driver? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;

        return x.Id == y.Id &&
               x.Known == y.Known &&
               x.UserId == y.UserId &&
               x.Name == y.Name &&
               x.Key == y.Key &&
               x.Nationality == y.Nationality;
    }

    public int GetHashCode(Driver obj)
    {
        return HashCode.Combine(obj.Id, obj.Known, obj.UserId, obj.Name, obj.Key, obj.Nationality);
    }
}
