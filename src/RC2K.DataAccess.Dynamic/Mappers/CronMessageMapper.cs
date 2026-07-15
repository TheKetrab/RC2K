using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class CronMessageMapper : IModelMapper<CronMessage, CronMessageModel>
{
    public CronMessageModel ToCosmosModel(CronMessage cronMessage)
    {
        CronMessageModel model = new()
        {
            Id = cronMessage.Id,
            Published = cronMessage.Published,
            Message = cronMessage.Message,
            Cron = cronMessage.Cron
        };

        return model;
    }

    public CronMessage ToDomainModel(CronMessageModel model)
    {
        CronMessage cronMessage = new()
        {
            Id = model.Id,
            Published = model.Published,
            Message = model.Message,
            Cron = model.Cron
        };

        return cronMessage;
    }
}
