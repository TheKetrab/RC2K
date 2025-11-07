
using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess;
using RC2K.DataAccess.Fake;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DataAccess.Interfaces;

const int CarsCount = 20;
const int UsersCount = 10;
const int StagesCount = 50;
const int DriversCount = 20;
const int TimeEntriesCount = 100;
const int VerifyInfosCount = 60;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IDataContext, InMemoryDataAccess>(sp => new InMemoryDataAccess(CarsCount, DriversCount, StagesCount, TimeEntriesCount, UsersCount, 10, VerifyInfosCount))
    .AddSingleton<ICarRepository, RC2K.DataAccess.Fake.Repositories.CarRepository>()
    .AddSingleton<IDriverRepository, RC2K.DataAccess.Fake.Repositories.DriverRepository>()
    .AddSingleton<IStageRepository, RC2K.DataAccess.Fake.Repositories.StageRepository>()
    .AddSingleton<ITimeEntryRepository, RC2K.DataAccess.Fake.Repositories.TimeEntryRepository>()
    .AddSingleton<IUserRepository, RC2K.DataAccess.Fake.Repositories.UserRepository>()
    .AddSingleton<IVerifyInfoRepository, RC2K.DataAccess.Fake.Repositories.VerifyInfoRepository>()
    .AddSingleton<IRallyUoW,RallyUoW>()
    .BuildServiceProvider();

IRallyUoW uow = serviceProvider.GetService<IRallyUoW>()!;


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
