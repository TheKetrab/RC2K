
namespace RC2K.DataAccess.Dynamic.Mappers;

public interface IModelMapper<TDomainModel, TCosmosModel>
{
    TDomainModel ToDomainModel(TCosmosModel model);
    TCosmosModel ToCosmosModel(TDomainModel model);
}
