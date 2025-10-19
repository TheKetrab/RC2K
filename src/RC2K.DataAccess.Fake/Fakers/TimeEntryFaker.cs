
using Bogus;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake.Fakers;

public class TimeEntryFaker : Faker<TimeEntry>
{
    private readonly DateTime _minDate = new(2020, 01, 01);
    private readonly DateTime _maxDate = new(2024, 12, 31);

    private readonly TimeOnly _minTime = new(0, 2, 0, 0);
    private readonly TimeOnly _maxTime = new(0, 20, 0, 0);

    private int _id = 1;

    private readonly IDataContext _context;

    public TimeEntryFaker(IDataContext context)
    {
        _context = context;

        RuleFor(x => x.Id, f => _id++);
        RuleFor(x => x.Time, f => f.Date.BetweenTimeOnly(_minTime, _maxTime));
        RuleFor(x => x.UploadTime, f => f.Date.Between(_minDate, _maxDate));
        RuleFor(x => x.CarId, GetCarId);
        RuleFor(x => x.StageId, GetStageId);
        RuleFor(x => x.DriverId, GetDriverId);
    }

    private int GetCarId(Faker f) => f.Random.Id(_context.Cars.Count());
    private int GetStageId(Faker f) => f.Random.Id(_context.Stages.Count());
    private int GetDriverId(Faker f) => f.Random.Id(_context.Drivers.Count());

}
