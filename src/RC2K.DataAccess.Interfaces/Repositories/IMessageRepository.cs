using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IMessageRepository
{
    Task<List<DateTimeMessage>> GetAll();
    Task Update(DateTimeMessage entity);

}
