using RC2K.Presentation.Blazor;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureRazor()
    .ConfigureAuthentication()
    .ConfigureDatabase()
    .RegisterServices()
    .ConfigureLogging()
    .AddAuthorization();

var app = builder.Build()
    .ConfigureExceptionHandler()
    .ConfigureApplication();

if (builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"] is null)
{
    app.Logger.LogWarning("APPLICATIONINSIGHTS_CONNECTION_STRING env variable not defined so AppInsights will not be working");
}


await app.RunAsync();
