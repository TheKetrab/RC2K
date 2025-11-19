using Microsoft.AspNetCore.Authentication.Cookies;
using RC2K.Logic.Interfaces;
using System.Security.Claims;

namespace RC2K.Presentation.Blazor;

public class AuthService
{
    private readonly IUserService _userService;
    public AuthService(IUserService userSerivce)
    {
        _userService = userSerivce;
    }

    public async Task<ClaimsPrincipal> Login(string username, string password)
    {
        var res = await _userService.Authenticate(username, password);
        if (!res.Success)
        {
            throw new Exception("Incorrect user or password");
        }

        List<Claim> claims = [
            new Claim(ClaimTypes.Name, username),
        ];

        string[] roles = res.Message!.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        return principal;
    }
}