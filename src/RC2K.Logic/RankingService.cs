using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public class RankingService : IRankingService
{
    private readonly IRankingsRepository _rankingRepository;
    private readonly IFillersBag _fillers;

    public RankingService(IRankingsRepository rankingRepository,
                            IFillersBag fillers)
    {
        _rankingRepository = rankingRepository;
        _fillers = fillers;
    }

    public async Task<RankingSnapshot> GetLatest()
    {
        var ranking = await _rankingRepository.GetCurrent();

        await ranking.FillFullData(_fillers.RankingFiller, _fillers);

        return ranking;
    }

}
