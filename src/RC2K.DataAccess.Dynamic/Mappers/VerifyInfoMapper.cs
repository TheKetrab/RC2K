using RC2K.DataAccess.Dynamic.Models;
using static RC2K.Utils.Utils;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class VerifyInfoMapper : IModelMapper<VerifyInfo, VerifyInfoModel>
{
    public VerifyInfoModel ToCosmosModel(VerifyInfo verifyInfo)
    {
        VerifyInfoModel model = new VerifyInfoModel()
        {
            Id = verifyInfo.Id,
            VerifierId = verifyInfo.VerifierId,
            Comment = verifyInfo.Comment,
            VerifyDate = DateTimeToString(verifyInfo.VerifyDate),
        };

        return model;
    }

    public VerifyInfo ToDomainModel(VerifyInfoModel model)
    {
        VerifyInfo verifyInfo = new VerifyInfo()
        {
            Id = model.Id,
            VerifierId = model.VerifierId,
            Comment = model.Comment,
            VerifyDate = StringToDateTime(model.VerifyDate),
        };

        return verifyInfo;
    }
}
