
namespace RC2K.DataAccess.Interfaces;

public interface IEnvironmentProvider
{
    string ResolveContainerName(string entityName);
}
