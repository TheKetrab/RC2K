using Microsoft.AspNetCore.Components.Authorization;

namespace RC2K.Presentation.Blazor.AuthProxy;

public static class Auth
{
    public static void Authorize(AuthenticationState state)
    {
        if (!TryAuthorize(state))
        {
            throw new NotAuthorizedException("Required logged in user.");
        }
    }

    public static bool TryAuthorize(AuthenticationState state)
    {
        return state.User.Identity?.IsAuthenticated ?? false;
    }


    public static void Authorize(AuthenticationState state, string role)
    {
        if (!TryAuthorize(state, role))
        {
            throw new NotAuthorizedException($"Required role: {role}");
        }
    }

    public static bool TryAuthorize(AuthenticationState state, string role)
    {
        return state.User.IsInRole(role);
    }

    public static void AuthorizeSelf(AuthenticationState state, string username)
    {
        if (!TryAuthorizeSelf(state, username))
        {
            throw new NotAuthorizedException($"Allowed only for user {username}");
        }
    }

    public static bool TryAuthorizeSelf(AuthenticationState state, string username)
    {
        return state.User.Identity?.Name == username;
    }

}