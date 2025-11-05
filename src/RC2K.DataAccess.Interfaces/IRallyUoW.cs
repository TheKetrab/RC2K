using RC2K.DataAccess.Interfaces.Repositories;

namespace RC2K.DataAccess.Interfaces;

public interface IRallyUoW
{
    ICarRepository Cars { get; }
    IStageRepository Stages { get; }

    Task Save();
}
