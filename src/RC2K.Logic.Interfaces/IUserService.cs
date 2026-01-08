using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IUserService
{
    Task<User?> GetById(Guid id);
    Task<Result> UpgradeDriverToUser(string name, string driverPassCode, string password, string email);
    void SetEmailConfirmationCode(string email, string code);
    Task<Result> Authenticate(string name, string password);
    Task<Result> CreateUserWithPassword(string name, string password, string? nationality, string email);
    Task<Result> CreateUserWithOAuth(string name, string email, string? nationality);
    Task<string> GetCurrentUserName();
}
