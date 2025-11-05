using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces.Fillers;

public interface ITimeEntryFiller
{
    Task FillRecursive(TimeEntry timeEntry, FillingContext context, IFillersBag fillers);
}
