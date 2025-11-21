using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IDriverService
{
    Task<Driver?> GetByName(string name);
}
