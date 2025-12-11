
namespace RC2K.Logic.Interfaces.Fillers;

public interface IFiller<T>
{
    Task FillRecursive(T entity, FillingContext context, IFillersBag fillers);
}
