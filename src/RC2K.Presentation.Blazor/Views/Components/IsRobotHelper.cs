using Microsoft.JSInterop;

namespace RC2K.Presentation.Blazor.Views.Components;

public static class IsRobotHelper
{
    public static async Task<bool> IsRobot(IJSRuntime js, ICaptchaVerifier captcha, Action<string> showError)
    {
        string token = await js.InvokeAsync<string>("runCaptcha");
        int isRobot = await captcha.IsRobot(token);
        if (isRobot == 1)
        {
            showError("You are a robot! Aren't you? - try again or contact TheKetrab via discord.");
            return true;
        }
        if (isRobot == -1)
        {
            showError("You failed reCAPTCHA verification and we are not sure if you are a robot. Try again or contact TheKetrab via discord.");
            return true;
        }

        return false;
    }
}
