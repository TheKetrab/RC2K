using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers;

public class VerifyInfoFiller(IUserRepository userRepository)
    : IVerifyInfoFiller
{
    public async Task FillRecursive(VerifyInfo verifyInfo, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.VerifyInfos.ContainsKey(verifyInfo.Id))
        {
            return;
        }
        context.VerifyInfos.Add(verifyInfo.Id, verifyInfo);

        await FillVerifier(verifyInfo, context, fillers, ct);
    }

    private async Task FillVerifier(VerifyInfo verifyInfo, FillingContext context, IFillersBag fillers, CancellationToken ct)
    {
        if (context.Users.TryGetValue(verifyInfo.VerifierId, out User? user))
        {
            verifyInfo.Verifier = user;
        }
        else
        {
            verifyInfo.Verifier = (await userRepository.GetById(verifyInfo.VerifierId, ct)) ?? throw new KeyNotFoundException();
            await fillers.UserFiller.FillRecursive(verifyInfo.Verifier, context, fillers, ct);
        }
    }
}
