using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public static class FillFullDataExtensions
{
    public async static Task FillFullData<T>(this List<T> entities, IFiller<T> topLevelFiller, IFillersBag fillers, CancellationToken ct = default)
    {
        FillingContext context = new();
        foreach (var entity in entities)
        {
            ct.ThrowIfCancellationRequested();
            await topLevelFiller.FillRecursive(entity, context, fillers, ct);
        }
    }

    public async static Task FillFullData<T>(this T entity, IFiller<T> topLevelFiller, IFillersBag fillers, CancellationToken ct = default)
    {
        FillingContext context = new();
        await topLevelFiller.FillRecursive(entity, context, fillers, ct);
    }

}
