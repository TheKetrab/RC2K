using RC2K.DataAccess.Interfaces;

namespace RC2K.DataAccess.Dynamic.EnvironmentProviders;

public class DevEnvironmentProvider : IEnvironmentProvider
{
    public string ResolveContainerName(string entityName) => entityName;
}
