using Microsoft.AspNetCore.Components.Authorization;
using RC2K.Logic;
using RC2K.Logic.Interfaces;

namespace RC2K.Presentation.Blazor.AuthProxy
{
    public class AuthUserServiceProxy : IUserService
    {
        private AuthenticationStateProvider _asp;
        private UserService _service;

        public AuthUserServiceProxy(
            AuthenticationStateProvider asp,
            UserService service)
        {
            _asp = asp;
            _service = service;
        }

        public Task<Result> Authenticate(string name, string password) =>
            _service.Authenticate(name, password);

        public Task<Result> CreateUserWithOAuth(string name, string email, string? nationality) =>
            _service.CreateUserWithOAuth(name, email, nationality);

        public Task<Result> CreateUserWithPassword(string name, string password, string? nationality, string email) =>
            _service.CreateUserWithPassword(name, password, nationality, email);

        public async Task<string> GetCurrentUserName()
        {
            var auth = await _asp.GetAuthenticationStateAsync();
            if (!Auth.TryAuthorize(auth))
            {
                return await _service.GetCurrentUserName();
            }

            return auth.User.Identity!.Name!;
        }

        public void SetEmailConfirmationCode(string email, string code) =>
            _service.SetEmailConfirmationCode(email, code);

        public Task<Result> UpgradeDriverToUser(string name, string driverPassCode, string password, string email)
            => _service.UpgradeDriverToUser(name, driverPassCode, password, email);
    }
}
