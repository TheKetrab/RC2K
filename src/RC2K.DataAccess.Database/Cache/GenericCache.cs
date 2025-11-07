using Microsoft.Extensions.Caching.Memory;
using RC2K.DataAccess.Interfaces.Cache;

namespace RC2K.DataAccess.Database.Cache;

public class GenericCache<T>(IMemoryCache cache) : IGenericCache<T> where T : class
{
    public T? Get(int id) => cache.TryGetValue(id, out T? entity) ? entity : null;

    public U? Get<U>(string key) where U : class => cache.TryGetValue(key, out U? val) ? val : null;

    public void Set(int id, T entity) => cache.Set(id, entity);

    public void Set<U>(string key, U entity) where U : class => cache.Set(key, entity);
}
