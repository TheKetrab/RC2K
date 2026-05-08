using RC2K.Presentation.Blazor;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureRazor()
    .ConfigureAuthentication()
    .ConfigureDatabase()
    .RegisterServices()
    .ConfigureLogging()
    .AddAuthorization();
builder.Services.AddHealthChecks();


var app = builder.Build()
    .ConfigureExceptionHandler()
    .ConfigureApplication();

app.MapHealthChecks("/ping");

if (builder.Configuration["ApplicationInsights:ConnectionString"] is null)
{
    app.Logger.LogWarning("ApplicationInsights:ConnectionString env variable not defined so AppInsights will not be working");
}


await app.RunAsync();
