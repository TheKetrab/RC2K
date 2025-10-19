using RC2K.Presentation.Blazor.Views;

public static class AppConfiguration
{
    public static WebApplication ConfigureExceptionHandler(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        return app;
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        return app;
    }
}
