
namespace RC2K.Logic.Interfaces;

public interface IPasswordProvider
{
    string CalculatePasswordHash(string password);
    string CreateDriverKey();
    string GenerateTemporaryPassword();
}
