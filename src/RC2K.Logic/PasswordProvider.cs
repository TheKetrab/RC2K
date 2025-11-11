using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;

namespace RC2K.Logic;

public class PasswordProvider : IPasswordProvider
{
    public string CalculatePasswordHash(string password)
    {
        return "d77c41f7aa5a02c7297fb7a75565a76fc95b90a89988073fbe3a04fa5903eced"; // TODO
    }

    public string CreateDriverKey()
    {
        return "n5L=Z?81EFE?"; // TODO
    }

    public string GenerateTemporaryPassword()
    {
        return "u5w7Q0SmdJU4"; // TODO
    }

}