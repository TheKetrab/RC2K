using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IUserService
{
    Task<Result> Authenticate(string name, string password);
    Task<Result> CreateUserWithPassword(string name, string password, string? nationality, string email);
    Task<Result> CreateUserWithOAuth(string name, string email, string? nationality);
}
