using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface ICronMessageRepository
{
    Task<List<CronMessage>> GetAll();
    Task Update(CronMessage entity);

}
