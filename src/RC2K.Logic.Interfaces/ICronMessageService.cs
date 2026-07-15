using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface ICronMessageService
{
    Task<List<CronMessage>> GetAll();
    Task Update(CronMessage entity);
}
