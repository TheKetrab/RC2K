var builder = WebApplication.CreateBuilder(args)
    .ConfigureRazor()
    .ConfigureAuthentication()
    .ConfigureDatabase()
    .RegisterServices()
    .AddAuthorization();

var app = builder.Build()
    .ConfigureExceptionHandler()
    .ConfigureApplication();

app.Run();
