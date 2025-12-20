using Microsoft.AspNetCore.Components.Authorization;
using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;

namespace RC2K.Presentation.Blazor.AuthProxy
{
    public class AuthRankingServiceProxy : IRankingService
    {
        private AuthenticationStateProvider _asp;
        private RankingService _service;

        public AuthRankingServiceProxy(
            AuthenticationStateProvider asp,
            RankingService service)
        {
            _asp = asp;
            _service = service;
        }

        public async Task DoRankingSnapshot()
        {
            
            var auth = await _asp.GetAuthenticationStateAsync();
            Auth.Authorize(auth, "admin");

            await _service.DoRankingSnapshot();
        }

        public Task<RankingSnapshot> GetLatest() => _service.GetLatest();
    }
}
