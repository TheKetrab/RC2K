
namespace RC2K.Presentation.Blazor;

public interface ICaptchaVerifier
{
    /// <summary>
    /// Verifies token with external provider. Returns 1 if is robot, 0 if is user, -1 if unknown.
    /// </summary>
    Task<int> IsRobot(string token);
}
