using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;
using System.Globalization;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class DateTimeMessageMapper : IModelMapper<DateTimeMessage, MessageModel>
{
    public MessageModel ToCosmosModel(DateTimeMessage message)
    {
        return new MessageModel()
        {
            Id = message.Id,
            Message = message.Value,
            DateTime = message.DateTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
            Published = message.Published,
            Name = message.Name
        };
    }

    public DateTimeMessage ToDomainModel(MessageModel model)
    {
        return new DateTimeMessage()
        {
            Id = model.Id,
            DateTime = DateTime.ParseExact(model.DateTime!, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
            Published = model.Published,
            Value = model.Message,
            Name = model.Name
        };
    }
}
