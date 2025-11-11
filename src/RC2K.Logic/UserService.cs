using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IPasswordProvider _passwordProvider;

    public UserService(IUserRepository userRepository, IDriverRepository driverRepository, IPasswordProvider passwordProvider)
    {
        _userRepository = userRepository;
        _driverRepository = driverRepository;
        _passwordProvider = passwordProvider;
    }

    public async Task<Result> CreateUserWithPassword(string name, string? password, string? nationality, string? email = null)
    {
        password ??= _passwordProvider.GenerateTemporaryPassword();
        string passwordHash = _passwordProvider.CalculatePasswordHash(password);
        return await CreateUserInternal(name, nationality, passwordHash, email);
    }

    public Task<Result> CreateUserWithOAuth(string name, string email, string? nationality) =>
        CreateUserInternal(name, nationality, null, email);

    private async Task<Result> CreateUserInternal(string name, string? nationality, string? passwordHash, string? email)
    {
        Guid driverId = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        Driver driver = new Driver()
        {
            Id = driverId,
            Known = true,
            Nationality = nationality,
            UserId = userId
        };

        User user = new User()
        {
            DriverId = driverId,
            Id = userId,
            Name = name,
            PasswordHash = passwordHash,
            Roles = ["user"],
            Email = email,
        };

        try
        {
            await _userRepository.Create(user);
            await _driverRepository.Create(driver);

            return new Result()
            {
                Success = true
            };
        }
        catch (UserExistsException)
        {
            return new Result()
            {
                Success = false,
                ErrorCode = (int)ErrorCodes.UserAlreadyExists,
                Message = "User exists"
            };
        }
    }
}