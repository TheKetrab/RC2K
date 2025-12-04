using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Fillers;

public interface IBonusPointsFiller
{
    Task FillRecursive(BonusPoints bonusPoints, FillingContext context, IFillersBag fillers);
}
