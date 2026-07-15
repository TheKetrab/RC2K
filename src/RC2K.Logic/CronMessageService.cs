using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class CronMessageService(ICronMessageRepository cronMessageRepository) : ICronMessageService
{
    public Task<List<CronMessage>> GetAll() => cronMessageRepository.GetAll();
    public Task Update(CronMessage entity) => cronMessageRepository.Update(entity);
}