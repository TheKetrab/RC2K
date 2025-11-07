namespace RC2K.DataAccess.Interfaces.Cache;

public interface IGenericCache<T> where T : class
{
    T? Get(int id);
    void Set(int id, T entity);
    U? Get<U>(string key) where U : class;
    void Set<U>(string key, U entity) where U : class;
}
