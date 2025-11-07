using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Fillers;

public interface IDriverFiller
{
    Task FillRecursive(Driver driver, FillingContext context, IFillersBag fillers);
}
