using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;
using static RC2K.Utils.Utils;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class NotificationMapper : IModelMapper<Notification, NotificationModel>
{
    public NotificationModel ToCosmosModel(Notification notification)
    {
        NotificationModel model = new NotificationModel()
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            Created = DateTimeToString(notification.Created)
        };

        return model;
    }

    public Notification ToDomainModel(NotificationModel model)
    {
        Notification notification = new Notification()
        {
            Id = model.Id,
            UserId = model.UserId,
            Message = model.Message,
            Created = StringToDateTime(model.Created)
        };

        return notification;
    }
}
