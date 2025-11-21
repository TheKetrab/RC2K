using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class VerifyInfoFiller(IUserRepository userRepository)
    : IVerifyInfoFiller
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task FillRecursive(VerifyInfo verifyInfo, FillingContext context, IFillersBag fillers)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (context.VerifyInfos.ContainsKey(verifyInfo.Id))
            {
                return;
            }
            context.VerifyInfos.Add(verifyInfo.Id, verifyInfo);

            await FillVerifier(verifyInfo, context, fillers);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task FillVerifier(VerifyInfo verifyInfo, FillingContext context, IFillersBag fillers)
    {
        if (context.Users.TryGetValue(verifyInfo.VerifierId, out User? user))
        {
            verifyInfo.Verifier = user;
        }
        else
        {
            verifyInfo.Verifier = (await userRepository.GetById(verifyInfo.VerifierId)) ?? throw new KeyNotFoundException();
            await fillers.UserFiller.FillRecursive(verifyInfo.Verifier, context, fillers);
        }
    }
}
