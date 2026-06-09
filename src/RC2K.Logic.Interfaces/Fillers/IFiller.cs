
namespace RC2K.Logic.Interfaces.Fillers;

public interface IFiller<in T>
{
    Task FillRecursive(T entity, FillingContext context, IFillersBag fillers, CancellationToken ct);
}
