using Microsoft.AspNetCore.Components.Authorization;
using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;

namespace RC2K.Presentation.Blazor.AuthProxy;

public class AuthRankingServiceProxy(
    AuthenticationStateProvider asp,
    RankingService service) 
    : IRankingService
{
    public async Task DoRankingSnapshot()
    {
        var auth = await asp.GetAuthenticationStateAsync();
        Auth.Authorize(auth, Auth.AdminRole);

        await service.DoRankingSnapshot();
    }

    public Task<RankingSnapshot> GetLatest() => service.GetLatest();
}
