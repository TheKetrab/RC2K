using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Fillers;

public interface IVerifyInfoFiller
{
    Task FillRecursive(VerifyInfo verifyInfo, FillingContext context, IFillersBag fillers);
}
