using RC2K.Presentation.Blazor;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureRazor()
    .ConfigureAuthentication()
    .ConfigureDatabase()
    .RegisterServices()
    .AddAuthorization();

var app = builder.Build()
    .ConfigureExceptionHandler()
    .ConfigureApplication();

await app.RunAsync();
