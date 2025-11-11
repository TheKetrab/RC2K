namespace RC2K.Logic.Interfaces;

public interface IUserService
{
    Task<Result> CreateUserWithPassword(string name, string password, string nationality, string? email = null);
    Task<Result> CreateUserWithOAuth(string name, string email, string nationality);
}
