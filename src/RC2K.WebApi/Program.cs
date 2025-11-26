using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;
using RC2K.WebApi;
using RC2K.WebApi.Dto;
using RC2K.Extensions;
using System.Globalization;
using RC2K.DataAccess.Interfaces.Repositories;

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
    var userService = app.Services.GetService<IUserService>()!;
    await userService.CreateUserWithPassword(dto.Name, dto.Password, dto.Nationality, dto.Email);

}).WithOpenApi();



app.MapGet("car/name2id", async () =>
{
    var carService = app.Services.GetService<ICarService>()!;
    var allCars = await carService.GetAll();

    return allCars.Select(x => new
    {
        x.Name,
        x.Id,
    });
});

app.MapGet("driver/driverName2id", async () =>
{
    var userRepository = app.Services.GetService<IUserRepository>()!;
    var driverRepository = app.Services.GetService<IDriverRepository>()!;

    List<(string, string)> friendlyName2driverId = new();

    var allUsers = await userRepository.GetAll();
    friendlyName2driverId.AddRange(allUsers.Select(x => (x.Name, x.DriverId.ToString())));

    var allAnonymousDrivers = (await driverRepository.GetAll()).Where(x => !x.Known).ToList();
    friendlyName2driverId.AddRange(allAnonymousDrivers.Select(x => (x.Name!, x.Id.ToString())));

    return friendlyName2driverId.Select(x => new
    {
        Name = x.Item1,
        Id = x.Item2
    });
});

app.MapGet("stage/code2id", async () =>
{
    var stageService = app.Services.GetService<IStageService>()!;
    var allStages = await stageService.GetAll();


    List<(int code, int[] ids)> map = new();
    foreach (var group in allStages.GroupBy(x => x.Code))
    {
        (int code, int[] ids) s;
        s.code = group.Key;
        s.ids = new int[2];
        s.ids[0] = group.First(x => x.Direction == Direction.Simulation).Id;
        s.ids[1] = group.First(x => x.Direction == Direction.Arcade).Id;

        map.Add(s);
    }
    return map.Select(item => new
    {
        Code = item.code,
        Ids = item.ids
    });
    //return map;
});


app.MapPost("/timeEntry/upload", async (UploadTimeEntryDto dto) =>
{
    try
    {
        TimeEntry te = new()
        {
            CarId = dto.CarId,
            DriverId = dto.DriverId,
            Id = Guid.NewGuid(),
            StageId = dto.StageId,
            Labels = dto.Labels,
            Time = dto.Time.ToTimeOnly(),
            UploadTime = RC2K.Utils.Utils.StringToDateTime(dto.Date),            
        };

        if (dto.Proofs?.Any() ?? false)
        {
            te.Proofs = dto.Proofs.Select(x => RC2K.Utils.Utils.DeserializeProof(x)).ToList();
        }

        if (dto.VerifyInfoId.HasValue)
        {
            te.VerifyInfoId = dto.VerifyInfoId.Value;
        }

        var timeEntryService = app.Services.GetService<ITimeEntryService>()!;
        await timeEntryService.Upload(te);
    }
    catch (Exception ex)
    {

    }

}).WithOpenApi();


app.Run();
