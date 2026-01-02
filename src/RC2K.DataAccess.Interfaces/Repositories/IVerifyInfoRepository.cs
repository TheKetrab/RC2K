using RC2K.DomainModel;

namespace RC2K.DataAccess.Interfaces.Repositories;

public interface IVerifyInfoRepository
{
    Task Create(VerifyInfo entity);
    Task<List<VerifyInfo>> GetAll();
    Task<VerifyInfo?> GetById(Guid id);
}
