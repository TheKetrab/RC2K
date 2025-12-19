using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public static class FillFullDataExtensions
{
    public async static Task FillFullData<T>(this List<T> entities, IFiller<T> topLevelFiller, IFillersBag fillers)
    {
        FillingContext context = new();
        Task[] tasks = entities.Select(x => topLevelFiller.FillRecursive(x, context, fillers)).ToArray();
        await Task.WhenAll(tasks);
    }

    public async static Task FillFullData<T>(this T entity, IFiller<T> topLevelFiller, IFillersBag fillers)
    {
        FillingContext context = new();
        await topLevelFiller.FillRecursive(entity, context, fillers);
    }

}
