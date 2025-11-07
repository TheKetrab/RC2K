using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Fillers;

public interface IUserFiller
{
    Task FillRecursive(User user, FillingContext context, IFillersBag fillers);
}
