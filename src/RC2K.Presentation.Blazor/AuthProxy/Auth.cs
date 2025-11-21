using Microsoft.AspNetCore.Components.Authorization;

namespace RC2K.Presentation.Blazor.AuthProxy;

public static class Auth
{
    public static void Authorize(AuthenticationState state)
    {
        if (state.User.Identity?.IsAuthenticated is not true)
        {
            throw new NotAuthorizedException();
        }
    }

    public static void Authorize(AuthenticationState state, string role)
    {
        if (!state.User.IsInRole(role))
        {
            throw new NotAuthorizedException();
        }
    }

    public static void AuthorizeSelf(AuthenticationState state, string username)
    {
        if (state.User.Identity?.Name != username)
        {
            throw new NotAuthorizedException();
        }
    }
}