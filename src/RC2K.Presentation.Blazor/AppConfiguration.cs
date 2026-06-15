using RC2K.Presentation.Blazor.TrafficLimits;
using RC2K.Presentation.Blazor.Views;

namespace RC2K.Presentation.Blazor;

public static class AppConfiguration
{
    public static WebApplication ConfigureExceptionHandler(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        return app;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseBotBlocking();
        app.UseAuthentication();
        app.UseCors(BuilderConfiguration.MyAllowSpecificOrigins);
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        return app;
    }
}
