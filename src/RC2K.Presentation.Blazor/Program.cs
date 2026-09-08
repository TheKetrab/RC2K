using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RC2K.Logic;
using RC2K.Logic.Interfaces;
using RC2K.Presentation.Blazor;
using System.Text;

var builder = WebApplication.CreateBuilder(args)
    .ConfigureRazor()
    .RegisterServices();

var app = builder.Build()
    .ConfigureExceptionHandler()
    .ConfigureApplication();

app.MapHealthChecks("/ping");

app.MapPost("/ranking/auto-snapshot", async (
    [FromHeader(Name = "Authorization")] string? authorization,
    IUserService userService,
    RankingService rankingService // non auth service
) =>
{
    if (authorization == null || !authorization.StartsWith("Basic "))
    {
        return Results.BadRequest("Basic authorization required");
    }

    string base64Val = authorization.Substring("Basic ".Length);
    byte[] data = Convert.FromBase64String(base64Val);
    string decodedString = System.Text.Encoding.UTF8.GetString(data);

    if (!decodedString.Contains(":"))
    {
        return Results.BadRequest();
    }

    string[] split = decodedString.Split(':');
    string username = split[0];
    string password = split[1];
    var res = await userService.Authenticate(username, password);
    if (!res.Success || !res.Message!.Contains("admin"))
    {
        return Results.Forbid();
    }

    await rankingService.DoRankingSnapshot();
    return Results.Ok(res);
});


if (builder.Configuration["ApplicationInsights:ConnectionString"] is null)
{
    app.Logger.LogWarning("ApplicationInsights:ConnectionString env variable not defined so AppInsights will not be working");
}

await app.RunAsync();
