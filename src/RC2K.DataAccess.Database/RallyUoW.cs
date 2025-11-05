using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;

namespace RC2K.DataAccess.Database;

public class RallyUoW : IRallyUoW
{
    private RallyDbContext _dbContext;

    public RallyUoW(RallyDbContext dbContext,
                    ICarRepository carRepository,
                    IStageRepository stageRepository)
    {
        _dbContext = dbContext;
        Cars = carRepository;
        Stages = stageRepository;
    }

    public ICarRepository Cars { get; set; }
    public IStageRepository Stages { get; set; }

    public Task Save() => _dbContext.SaveChangesAsync();
}
