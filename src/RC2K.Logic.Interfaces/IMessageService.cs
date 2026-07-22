using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IMessageService
{
    Task<List<DateTimeMessage>> GetAll();
    Task<List<DateTimeMessage>> GetForToday();
    Task Update(DateTimeMessage entity);
}
