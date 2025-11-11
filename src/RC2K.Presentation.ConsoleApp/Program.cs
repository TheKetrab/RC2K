
using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DataAccess.Interfaces;
using System.Linq;
using System.CommandLine;
using System.CommandLine.Parsing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

const int CarsCount = 20;
const int UsersCount = 10;
const int StagesCount = 50;
const int DriversCount = 20;
const int TimeEntriesCount = 100;
const int VerifyInfosCount = 60;

//var serviceProvider = new ServiceCollection()
//    .AddSingleton<IDataContext, InMemoryDataAccess>(sp => new InMemoryDataAccess(CarsCount, DriversCount, StagesCount, TimeEntriesCount, UsersCount, 10, VerifyInfosCount))
//    .AddSingleton<ICarRepository, RC2K.DataAccess.Fake.Repositories.CarRepository>()
//    .AddSingleton<IDriverRepository, RC2K.DataAccess.Fake.Repositories.DriverRepository>()
//    .AddSingleton<IStageRepository, RC2K.DataAccess.Fake.Repositories.StageRepository>()
//    .AddSingleton<ITimeEntryRepository, RC2K.DataAccess.Fake.Repositories.TimeEntryRepository>()
//    .AddSingleton<IUserRepository, RC2K.DataAccess.Fake.Repositories.UserRepository>()
//    .AddSingleton<IVerifyInfoRepository, RC2K.DataAccess.Fake.Repositories.VerifyInfoRepository>()
//    .AddSingleton<IRallyUoW,RallyUoW>()
//    .BuildServiceProvider();

//IRallyUoW uow = serviceProvider.GetService<IRallyUoW>()!;


Option<FileInfo> fileOption = new("--createUser", "-cu")
{
    Description = "The file to read and display on the console."
};

RootCommand rootCommand = new("RC2K cli");
rootCommand.Options.Add(fileOption);

ParseResult parseResult = rootCommand.Parse(args);
if (parseResult.Errors.Count == 0 && parseResult.GetValue(fileOption) is FileInfo parsedFile)
{
    string rawJson = File.ReadAllText(parsedFile.FullName);
    dynamic data = JObject.Parse(rawJson);

    var users = data["users"];
    foreach (var user in users)
    {
        var payload = new
        {
            name = (string)user["name"],
            email = string.IsNullOrEmpty((string)user["email"]) ? null : (string)user["email"],
            password = string.IsNullOrEmpty((string)user["password"]) ? null : (string)user["password"],
            nationality = string.IsNullOrEmpty((string)user["nat"]) ? null : (string)user["nat"]
        };

        var json = JsonConvert.SerializeObject(payload);

        using var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

        HttpClient client = new HttpClient();
        var response = await client.PostAsync("https://localhost:7266/user/create", jsonContent);
    }

    return 0;
}
foreach (ParseError parseError in parseResult.Errors)
{
    Console.Error.WriteLine(parseError.Message);
}
return 1;





//var cars = await uow.Cars.GetAll();

//Console.WriteLine(new string('-', 100));
//Console.WriteLine("Cars:");
//foreach (var x in cars)
//    Console.WriteLine($"{x.Id:d2}. {x.Name}");

//var users = await uow.Users.GetAll();

//Console.WriteLine(new string('-', 100));
//Console.WriteLine("Users:");
//foreach (var x in users)
//    Console.WriteLine($"{x.Id:d2}. {x.ToString()})");

//var stages = await uow.Stages.GetAll();

//Console.WriteLine(new string('-', 100));
//Console.WriteLine("Stages:");
//foreach (var x in stages)
//    Console.WriteLine($"{x.Id:d2}. {x.Code,-20}");

//var drivers = await uow.Drivers.GetAll();

//Console.WriteLine(new string('-', 100));
//Console.WriteLine("Drivers:");
//foreach (var x in drivers)
//    Console.WriteLine($"{x.Id:d2}. userId = {x.ToString()}");

//var timeEntries = await uow.TimeEntries.GetAll();

//Console.WriteLine(new string('-', 100));
//Console.WriteLine("Time entries:");
//foreach (var x in timeEntries)
//    Console.WriteLine($"{x.Id:d3}. driver: {x.DriverId,-4} stage: {x.StageId,-4} car: {x.CarId,-4} time = {x.Time.ToString("mm:ss.fff")}");

//var verifyInfos = await uow.VerifyInfos.GetAll();

//Console.WriteLine(new string('-', 100));
//Console.WriteLine("Verify infos:");
//foreach (var x in verifyInfos)
//    Console.WriteLine($"{x.Id:d3}. verifier: {x.VerifierId,-4} comment: {x.Comment.Substring(0, Math.Min(x.Comment.Length, 60))}");
