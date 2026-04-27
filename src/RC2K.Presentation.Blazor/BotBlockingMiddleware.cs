namespace RC2K.Presentation.Blazor;

public static class BotBlockingMiddlewareExtensions
{
    public static IApplicationBuilder UseBotBlocking(this IApplicationBuilder app)
        => app.UseMiddleware<BotBlockingMiddleware>();
}

/// <summary>
/// Detects well-known bot/crawler User-Agent strings and returns a minimal 200 response
/// so they never reach the Blazor circuit or SignalR layer.
/// This prevents bots from holding open WebSocket connections that block ACA scale-to-zero.
/// </summary>
public class BotBlockingMiddleware(RequestDelegate next)
{
    private static readonly string[] BotKeywords =
    [
        "bot", "crawler", "spider", "scraper", "slurp", "facebookexternalhit",
        "twitterbot", "linkedinbot", "whatsapp", "telegrambot", "applebot",
        "baiduspider", "yandexbot", "duckduckbot", "sogou", "exabot",
        "ia_archiver", "archive.org", "semrushbot", "ahrefsbot", "mj12bot",
        "dotbot", "petalbot", "bytespider", "gptbot", "ccbot", "claude-web",
        "anthropic", "dataprovider", "dataforseo", "rogerbot", "screaming frog",
    ];

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsBot(context))
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("OK");
            return;
        }

        await next(context);
    }

    private static bool IsBot(HttpContext context)
    {
        var userAgent = context.Request.Headers.UserAgent.ToString();
        if (string.IsNullOrEmpty(userAgent))
            return false;

        var ua = userAgent.ToLowerInvariant();
        foreach (var keyword in BotKeywords)
        {
            if (ua.Contains(keyword))
                return true;
        }

        return false;
    }
}
