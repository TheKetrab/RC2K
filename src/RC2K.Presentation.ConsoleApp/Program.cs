
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
using System.Net.Http.Json;
using RC2K.DomainModel;
using System.IO;

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


//Option<FileInfo> fileOption = new("--createUser", "-cu")
//{
//    Description = "The file to read and display on the console."
//};

//Option<FileInfo> fileOption2 = new("--createTimeEntry", "-cte")
//{
//    Description = "The file to read and display on the console."
//};


//RootCommand rootCommand = new("RC2K cli");
//rootCommand.Options.Add(fileOption);
//rootCommand.Options.Add(fileOption2);

//ParseResult parseResult = rootCommand.Parse(args);

//// ---------- JOB 1

////if (parseResult.Errors.Count == 0 && parseResult.GetValue(fileOption) is FileInfo parsedFile)
////{
////    string rawJson = File.ReadAllText(parsedFile.FullName);
////    dynamic data = JObject.Parse(rawJson);

////    var users = data["users"];
////    foreach (var user in users)
////    {
////        var payload = new
////        {
////            name = (string)user["name"],
////            email = string.IsNullOrEmpty((string)user["email"]) ? null : (string)user["email"],
////            password = string.IsNullOrEmpty((string)user["password"]) ? null : (string)user["password"],
////            nationality = string.IsNullOrEmpty((string)user["nat"]) ? null : (string)user["nat"]
////        };

////        var json = JsonConvert.SerializeObject(payload);

////        using var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

////        HttpClient client = new HttpClient();
////        var response = await client.PostAsync("https://localhost:7266/user/create", jsonContent);
////    }

////    return 0;
////}


//// ---------- JOB 2

//(string p, string vi)[] inputs = [
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\BRC20_data.json", "2d9b2fc0-6219-4d0f-9ec7-c25b1c1cfa9c"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI18_data.json", "e7755544-1fbf-4f87-9464-f7278d2b68b5"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI19_data.json", "6e2afa05-0c54-46ff-a984-710474aef6f7"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI20_data.json", "40cb64dc-4276-4f27-90aa-594137ec27fb"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI21_data.json", "4fa55626-68da-451d-971e-8e61e3859011"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI22_data.json", "aa39f135-ebc6-4459-83ad-a9db5f20e610"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI23_data.json", "6a00e126-4a15-42f8-87fe-bf8322fe14ad"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI24_data.json", "5994ec09-f0b5-4950-80c1-6e94de3b52a1"),
//    ("C:\\Projekty\\rc2k\\utils\\real-data\\MFMI25_data.json", "a9b02b41-ed70-4c47-aca3-7384e0373d66")
//];

//foreach (var (p,vi) in inputs)
//{
//    await fun_upload(p, vi);
//}

//// Verify info
//// BRC20  -> Ephemeral  2d9b2fc0-6219-4d0f-9ec7-c25b1c1cfa9c 
//// MFMI18 -> Ephemeral  e7755544-1fbf-4f87-9464-f7278d2b68b5
//// MFMI19 -> Ephemeral  6e2afa05-0c54-46ff-a984-710474aef6f7
//// MFMI20 -> Ephemeral  40cb64dc-4276-4f27-90aa-594137ec27fb
//// MFMI21 -> Linotrix   4fa55626-68da-451d-971e-8e61e3859011
//// MFMI22 -> Ephemeral  aa39f135-ebc6-4459-83ad-a9db5f20e610
//// MFMI23 -> Ephemeral  6a00e126-4a15-42f8-87fe-bf8322fe14ad
//// MFMI24 -> Ephemeral  5994ec09-f0b5-4950-80c1-6e94de3b52a1
//// MFMI25 -> Ephemeral  a9b02b41-ed70-4c47-aca3-7384e0373d66

//async Task fun_upload(string path, string verifyInfoGuid)
//{

//    HttpClient client = new HttpClient();
//    var response = await client.GetAsync("https://localhost:7266/stage/code2id");
//    var data1 = await response.Content.ReadAsStringAsync();
//    var xs = JArray.Parse(data1);

//    Dictionary<(int, Direction), int> _codeDirection2stageId = new();
//    foreach (var x in xs)
//    {
//        int code = x.Value<int>("code");
//        int[] ids = x["ids"].ToObject<int[]>();
//        _codeDirection2stageId.Add((code, Direction.Simulation), ids[0]);
//        _codeDirection2stageId.Add((code, Direction.Arcade), ids[1]);
//    }

//    response = await client.GetAsync("https://localhost:7266/car/name2id");
//    var data2 = await response.Content.ReadAsStringAsync();
//    var xs2 = JArray.Parse(data2);

//    Dictionary<string, int> _carName2carId = new();
//    foreach (var x in xs2)
//    {
//        string name = x.Value<string>("name");
//        int id = x.Value<int>("id");
//        _carName2carId.Add(name, id);
//    }

//    response = await client.GetAsync("https://localhost:7266/driver/driverName2id");
//    var data3 = await response.Content.ReadAsStringAsync();
//    var xs3 = JArray.Parse(data3);

//    Dictionary<string, Guid> _name2driverId = new();
//    foreach (var x in xs3)
//    {
//        string name = x.Value<string>("name");
//        string id = x.Value<string>("id");
//        Guid guid = Guid.Parse(id);
//        _name2driverId.Add(name, guid);
//    }

////if (parseResult.Errors.Count == 0 && parseResult.GetValue(fileOption2) is FileInfo parsedFile)
////{
////string path = "C:\\Users\\bartlomiej\\Desktop\\rc2k_users\\time-entries-mfmi24.json";






//    string rawJson = File.ReadAllText(path /*parsedFile.FullName*/);
//    var arr = JArray.Parse(rawJson);
//    var _allStages = RC2K.Resources.Stages.GetStages();
//    var _allCars = RC2K.Resources.Cars.GetAll();
//    foreach (dynamic item in arr)
//    {
//        try
//        {
//            string stage = item.stage;
//            if (stage == "Vauxhall Rally of Wales Summary" ||
//                stage == "Pirelli International Rally Summary" ||
//                stage == "RSAC Scottish Rally Summary" ||
//                stage == "Seat Jim Clark Memorial Rally Summary" ||
//                stage == "Stena Line Ulster Rally Summary" ||
//                stage == "Sony Manx International Rally Summary")
//            {
//                continue;
//            }


//            string type = item.type;
//            string labels = item.labels;
//            string driverName = item.driverName;
//            string[] proofs = ((JObject)item)["proofs"].ToObject<string[]>();
//            int time = item.time;
//            if (time <= 0)
//            {
//                continue;
//            }
//            string carName = item.car;
//            if (carName == "SEAT Córdoba WRC" || carName == "SEAT Cordoba WRC")
//            {
//                carName = "Seat Cordoba WRC";
//            }
//            if (carName == "Nissan Micra")
//            {
//                carName = "Nissan Micra Maxi";
//            }
//            string date = item.date;

//            if (stage == "Black loch")
//            {
//                stage = "Black Loch";
//            }
//            if (stage == "Moon and star")
//            {
//                stage = "Moon and Star";
//            }
//            if (stage == "Tholt-y-will")
//            {
//                stage = "Tholt-y-Will";
//            }

//            if (_allStages.FirstOrDefault(x => x.Name == stage) == null)
//            {

//            }
//            int stageCode = int.Parse(_allStages.First(x => x.Name == stage).Code);
//            Direction direction = type switch
//            {
//                "simulation" => Direction.Simulation,
//                "arcade" => Direction.Arcade,
//                _ => throw new Exception()
//            };

//            int stageId = _codeDirection2stageId[(stageCode, direction)];
//            int carId = _carName2carId[carName];
//            DateTime dt = DateTime.ParseExact(date, "yyyy.MM.dd", null);

//            if (driverName == "SpartaRemixerPL" || driverName == "Remixer")
//            {
//                driverName = "SpartaRemixer";
//            }
//            if (driverName == "SpartaX18")
//            {
//                driverName = "Ephemeral";
//            }
//            if (driverName == "Xsara" || driverName == "XaraTorrada")
//            {
//                driverName = "XsaraTorrada";
//            }
//            if (driverName == "Woeringen")
//            {
//                driverName = "Woeringen1288";
//            }
//            if (driverName == "Bros")
//            {
//                driverName = "BrosTheThird";
//            }
//            if (driverName == "sBinala" || driverName == "sBinalla")
//            {
//                driverName = "sBinnala";
//            }


//            Guid driverId = _name2driverId[driverName];

//            Guid verifyInfo = Guid.Parse(verifyInfoGuid);
            
//            var payload = new
//            {
//                DriverId = driverId,
//                StageId = stageId,
//                CarId = carId,
//                Time = time,
//                Labels = labels,
//                Proofs = proofs,
//                Date = dt.ToString("dd/MM/yyyy"),
//                VerifyInfoId = verifyInfo,
//            };

//            var json = JsonConvert.SerializeObject(payload);
//            using var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

//            var responsePost = await client.PostAsync("https://localhost:7266/timeEntry/upload", jsonContent);
//        }
//        catch (Exception ex)
//        {
//        }
//    }
//}

//foreach (var x in xs)
//{
//    var xxx = x["code"];

//}

//var allCars = RC2K.Resources.Cars.GetAll();
//var allStages = RC2K.Resources.Stages.GetStages();

//var tes = data["timeEntries"];
//foreach (var te in tes)
//{
//    string stageName = te["stage"];
//    string type = te["type"];

//    //RC2K.DataAccess.Database.Repositories.CarRepository
//    //allStages().
//    //allCars.Where(x => x.)


//    //var payload = new
//    //{
//    //    name = (string)user["name"],
//    //    email = string.IsNullOrEmpty((string)user["email"]) ? null : (string)user["email"],
//    //    password = string.IsNullOrEmpty((string)user["password"]) ? null : (string)user["password"],
//    //    nationality = string.IsNullOrEmpty((string)user["nat"]) ? null : (string)user["nat"]
//    //};

//    //var json = JsonConvert.SerializeObject(payload);

//    //using var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

//    //HttpClient client = new HttpClient();
//    //var response = await client.PostAsync("https://localhost:7266/user/create", jsonContent);
//}

//return 0;
//}



//foreach (ParseError parseError in parseResult.Errors)
//{
//    Console.Error.WriteLine(parseError.Message);
//}
//return 1;





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




// ----------- JOB 3
string path = @"C:\Projekty\rc2k\utils\real-data\MFMI-winners.txt";
string[] lines = File.ReadAllText(path).Replace("\t", " ")
    .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

int[] _mfmiPoints = [
        500,400,300,250,200,
        150,100,75,50,25
    ];

string year = "";
for (int i=0; i<lines.Length; i++)
{
    if (lines[i].StartsWith("// "))
    {
        year = lines[i].Substring(3);
        continue;
    }

    string[] split = lines[i].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (split[1] == "---")
    {
        continue;
    }
    int place = int.Parse(split[0]);
    string driverId = split[2];
    string end = place switch
    {
        1 => "st",
        2 => "nd",
        3 => "rd",
        _ => "th"
    };
    string comment = $"MFMI {year} {place}{end} place";
    int points = _mfmiPoints[place - 1];

    var payload = new
    {
        points,
        comment,
        driverId
    };

    var json = JsonConvert.SerializeObject(payload);

    using var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

    HttpClient client = new HttpClient();
    var response = await client.PostAsync("https://localhost:7266/bonusPoints/create", jsonContent);
}

Console.Read();