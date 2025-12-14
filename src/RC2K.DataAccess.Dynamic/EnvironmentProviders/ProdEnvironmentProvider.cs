using RC2K.DataAccess.Interfaces;

namespace RC2K.DataAccess.Dynamic.EnvironmentProviders;

public class ProdEnvironmentProvider : IEnvironmentProvider
{
    public string ResolveContainerName(string entityName) =>
        $"{entityName}-prod";    
}
