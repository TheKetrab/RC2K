using Microsoft.EntityFrameworkCore;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;
using RC2K.Extensions;

namespace RC2K.DataAccess.Database;

public class RallyDbContext : DbContext, IDataContext
{
    public RallyDbContext(DbContextOptions opt) : base(opt)
    {
        Cars = Set<Car>();
        Drivers = Set<Driver>();
        Stages = Set<Stage>();
        StagesData = Set<StageData>();
        StageWaypoints = Set<StageWaypoints>();
        TimeEntries = Set<TimeEntry>();
        Users = Set<User>();
        VerifyInfos = Set<VerifyInfo>();
    }

    public DbSet<Car> Cars { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<StageData> StagesData { get; set; }
    public DbSet<StageWaypoints> StageWaypoints { get; set; }
    public DbSet<TimeEntry> TimeEntries { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<VerifyInfo> VerifyInfos { get; set; }

    IQueryable<Car> IDataContext.Cars => Cars;
    IQueryable<Driver> IDataContext.Drivers => Drivers;
    IQueryable<Stage> IDataContext.Stages => Stages;
    IQueryable<StageData> IDataContext.StagesData => StagesData;
    IQueryable<StageWaypoints> IDataContext.StageWaypoints => StageWaypoints;
    IQueryable<TimeEntry> IDataContext.TimeEntries => TimeEntries;
    IQueryable<User> IDataContext.Users => Users;
    IQueryable<VerifyInfo> IDataContext.VerifyInfos => VerifyInfos;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CAR
        var resourceCars = Resources.Cars.GetAll();
        IEnumerable<Car> carsData = resourceCars.Select((x,i) => new Car { Id = i + 1, Name = x.Name, Class = x.ClassName[1] - '0' });

        modelBuilder.Entity<Car>()
            .HasData(carsData);

        // DRIVER
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.ToTable("Drivers");
            entity.HasKey(d => d.Id);
            entity.HasOne(d => d.User).WithOne(u => u.Driver).HasForeignKey<Driver>(d => d.UserId);
        });

        // STAGE
        var resourceStages = Resources.Stages.GetStages();
        List<Stage> stages =
            (from s in resourceStages
             from b in new[] { true, false }
             select new { s.Code, Arcade = b })
             .Select((x, i) => new Stage()
             {
                 Id = i + 1,
                 Code = int.Parse(x.Code),
                 IsArcade = x.Arcade
             }).ToList();

        modelBuilder.Entity<Stage>()
            .HasOne(x => x.StageData)
            .WithMany()
            .HasForeignKey(x => x.Code)
            .HasPrincipalKey(x => x.StageCode);

        modelBuilder.Entity<Stage>()
            .HasOne(x => x.StageWaypoints)
            .WithMany()
            .HasForeignKey(x => x.Code)
            .HasPrincipalKey(x => x.StageCode);

        modelBuilder.Entity<Stage>()
            .HasData(stages);

        // STAGE DETAILS
        var resourceStageDetails = Resources.Stages.GetStageDetails();
        List<StageDetails> stagesDetails = resourceStageDetails.Select(x => new StageDetails()
        {
            StageCode = int.Parse(x.Code),
            Length = float.Parse(x.Length),
            Asphalt = int.Parse(x.Asphalt),
            Dirt = int.Parse(x.Dirt),
            Gravel = int.Parse(x.Gravel),
            Snow = int.Parse(x.Snow),
            Temp = int.Parse(x.Temp),
            Wind = float.Parse(x.Wind),
            Mood = x.Mood.ParseMood()
        }).ToList();

        modelBuilder.Entity<StageDetails>().HasKey(x => x.StageCode);
        modelBuilder.Entity<StageDetails>().HasData(stagesDetails);

        // STAGE DATA
        var resourceRallyInfos = Resources.Stages.GetRallyInfos();
        List<StageData> stagesData =
            (from s in resourceStages
             join si in resourceRallyInfos.SelectMany(x => x.Stages) on s.Name equals si.Name
             select new { s.Code, s.Name, si.ImgName, si.Description })
             .Select(x => new StageData()
             {
                 StageCode = int.Parse(x.Code),
                 Name = x.Name,
                 ImgName = x.ImgName,
                 Description = x.Description,
             }).ToList();

        modelBuilder.Entity<StageData>().HasKey(x => x.StageCode);
        modelBuilder.Entity<StageData>().HasOne(x => x.StageDetails).WithOne().HasForeignKey<StageDetails>(x => x.StageCode);
        modelBuilder.Entity<StageData>().HasData(stagesData);

        // STAGE WAYPOINTS
        var resourceWaypoints = Resources.Stages.GetWaypoints();
        IEnumerable<StageWaypoints> stageWaypoints =
            resourceWaypoints.Select(x => new StageWaypoints()
            {
                ApiHint = x.ApiHint,
                StageCode = int.Parse(x.Code),
                Waypoints = x.Waypoints
            });

        modelBuilder.Entity<StageWaypoints>()
            .HasKey(x => x.StageCode);
        modelBuilder.Entity<StageWaypoints>()
            .HasData(stageWaypoints);

        // TIME ENTRY
        modelBuilder.Entity<TimeEntry>()
            .HasOne(x => x.VerifyInfo);

        // USERS
        modelBuilder.Entity<User>()
            .HasKey(x => x.Id);
    }
}
