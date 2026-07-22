using Microsoft.Extensions.Logging;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class MessageService(IMessageRepository cronMessageRepository) : IMessageService
{
    public Task<List<DateTimeMessage>> GetAll() => cronMessageRepository.GetAll();
    
    public async Task<List<DateTimeMessage>> GetForToday()
    {
        var now = DateTime.UtcNow;
        var todayMessages = (await GetAll())
            .OfType<DateTimeMessage>()
            .Where(x => x.DateTime.Day == now.Day &&
                        x.DateTime.Month == now.Month &&
                        x.DateTime.Year == now.Year)
            .ToList();

        return todayMessages;
    }

    public Task Update(DateTimeMessage entity) => cronMessageRepository.Update(entity);
}