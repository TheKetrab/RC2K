using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.Mappers;

public class UserMapper : IModelMapper<User, UserModel>
{
    public User ToDomainModel(UserModel model)
    {
        User user = new User()
        {
            Id = model.Id,
            DriverId = model.DriverId,
            Name = model.Name,
            PasswordHash = model.PasswordHash,
            Roles = model.Roles.ToArray(),
        };

        return user;
    }

    public UserModel ToCosmosModel(User user)
    {
        UserModel model = new UserModel()
        {
            Id = user.Id,
            DriverId = user.DriverId,
            Name = user.Name,
            PasswordHash = user.PasswordHash,
        };

        model.Roles.AddRange(user.Roles);

        return model;
    }
}
