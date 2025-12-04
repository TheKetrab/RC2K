using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public class BonusPointsService : IBonusPointsService
{
    private readonly IBonusPointsRepository _bonusPointsRepository;
    private readonly IFillersBag _fillers;

    public BonusPointsService(IBonusPointsRepository bonusPointsRepository, IFillersBag fillers)
    {
        _bonusPointsRepository = bonusPointsRepository;
        _fillers = fillers;
    }

    public async Task<List<BonusPoints>> GetAll()
    {
        var bonusPoints = await _bonusPointsRepository.GetAll();

        FillingContext context = new();
        Task[] tasks = bonusPoints.Select(x => _fillers.BonusPointsFiller.FillRecursive(x, context, _fillers)).ToArray();
        await Task.WhenAll(tasks);

        return bonusPoints;
    }
}