using RC2K.Logic;
using RC2K.Logic.Interfaces;
using RC2K.WebApi;
using RC2K.WebApi.Dto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.RegisterServices();
// TODO: Add bearer token auth, refresh token rotation + saving token in redis + invalidate compromised refresh token (invalidate token family -> for user)

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/user/create", async (CreateUserDto dto) =>
{
    try
    {
        var userService = app.Services.GetService<IUserService>()!;
        await userService.CreateUserWithPassword(dto.Name, dto.Password, dto.Nationality, dto.Email);
    }
    catch (Exception ex)
    {

    }

}).WithOpenApi();

app.Run();
