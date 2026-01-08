using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class UserService : IUserService
{
    internal static Dictionary<string, string> _emailConfirmationKeys = [];

    private readonly IUserRepository _userRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IPasswordProvider _passwordProvider;

    public UserService(IUserRepository userRepository, IDriverRepository driverRepository, IPasswordProvider passwordProvider)
    {
        _userRepository = userRepository;
        _driverRepository = driverRepository;
        _passwordProvider = passwordProvider;
    }

    public Task<string> GetCurrentUserName()
    {
        return Task.FromResult("");
    }

    public async Task<Result> Authenticate(string name, string password)
    {
        User? user = await _userRepository.GetByName(name);
        if (user is null)
        {
            return new Result() { Success = false };
        }

        string passwordHash = _passwordProvider.CalculatePasswordHash(password);

        if (user.PasswordHash == passwordHash)
        {
            return new Result() { Success = true, Message = string.Join(",", user.Roles) };
        }

        return new Result() { Success = false };
    } 

    public async Task<Result> UpgradeDriverToUser(string name, string driverPassCode, string password, string email)
    {
        Driver? driver = await _driverRepository.GetByName(name);
        if (driver is null)
        {
            return new Result() { Success = false, Message = $"Driver with name {name} not found" };
        }

        if (driverPassCode != driver.Key) 
        {
            return new Result() { Success = false, Message = $"Driver pass code is not valid" };
        }

        string passwordHash = _passwordProvider.CalculatePasswordHash(password);
        return await UpgradeDriverToUserInternal(driver, passwordHash, email);
    }

    public async Task<Result> CreateUserWithPassword(string name, string? password, string? nationality, string email)
    {
        password ??= _passwordProvider.GenerateTemporaryPassword();
        string passwordHash = _passwordProvider.CalculatePasswordHash(password);
        return await CreateUserInternal(name, nationality, passwordHash, email);
    }

    public Task<Result> CreateUserWithOAuth(string name, string email, string? nationality) =>
        CreateUserInternal(name, nationality, null, email);

    private async Task<Result> CreateUserInternal(string name, string? nationality, string? passwordHash, string email)
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
                Message = "User with given name or email already exists"
            };
        }
    }

    private async Task<Result> UpgradeDriverToUserInternal(Driver driver, string? passwordHash, string email)
    {
        Guid userId = Guid.NewGuid();

        Driver updatedDriver = new Driver()
        {
            Id = driver.Id,
            Known = true,
            Nationality = driver.Nationality,
            UserId = userId
        };

        User user = new User()
        {
            DriverId = driver.Id,
            Id = userId,
            Name = driver.Name,
            PasswordHash = passwordHash,
            Roles = ["user"],
            Email = email,
        };

        try
        {
            await _userRepository.Create(user);
            await _driverRepository.Update(updatedDriver);

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
                Message = "User with given name or email already exists"
            };
        }
    }

    public void SetEmailConfirmationCode(string email, string code)
    {
        _emailConfirmationKeys[email] = code;
    }
}