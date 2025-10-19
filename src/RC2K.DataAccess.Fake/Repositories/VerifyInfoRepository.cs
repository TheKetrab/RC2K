using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Repositories;

public class VerifyInfoRepository(IDataContext context)
    : AbstractRepository<VerifyInfo>(context), IVerifyInfoRepository
{
    protected override IQueryable<VerifyInfo> DataSet => _context.VerifyInfos;
}
