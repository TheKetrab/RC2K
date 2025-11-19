using RC2K.Logic.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace RC2K.Logic;

public class PasswordProvider : IPasswordProvider
{
    private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int PasswordLength = 16;

    private int _iterations;
    private byte[] _salt;

    public PasswordProvider(int iterations, string salt)
    {
        _iterations = iterations;
        _salt = Encoding.UTF8.GetBytes(salt);
    }

    public string CalculatePasswordHash(string password)
    {
        byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(password, _salt, _iterations, HashAlgorithmName.SHA256, 64);

        string hash = Convert.ToBase64String(hashBytes);

        return hash;
    }

    public string CreateDriverKey() => GenerateTemporaryPasswordInternal();

    public string GenerateTemporaryPassword() => GenerateTemporaryPasswordInternal();

    private static string GenerateTemporaryPasswordInternal()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(PasswordLength);
        return new string(randomBytes
            .Select(b => Chars[b % Chars.Length])
            .ToArray());
    }
}