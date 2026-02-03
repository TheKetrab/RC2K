using RC2K.DomainModel;

namespace RC2K.Logic.UnitTests;

public class PointsProviderTests
{
    private PointsProvider _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new();
    }

    [Test]
    public void CalculateGeneralStagePoints_OneDriverHasFirstAndSecondPlace_OtherDriverHasSecondPlace()
    {
        //Arrange
        Guid[] te = Enumerable.Range(0, 7).Select(_ => Guid.NewGuid()).ToArray();

        Guid driverA = Guid.NewGuid();
        Guid driverB = Guid.NewGuid();
        Guid driverC = Guid.NewGuid();
        Guid driverD = Guid.NewGuid();

        List<TimeEntry> timeEntries = [
            AnyTimeEntry(te[0], driverA, new TimeOnly(1, 1 ,1)),
            AnyTimeEntry(te[1], driverA, new TimeOnly(2, 2 ,2)),
            AnyTimeEntry(te[2], driverB, new TimeOnly(3, 3 ,3)),
            AnyTimeEntry(te[3], driverC, new TimeOnly(4, 4 ,4)),
            AnyTimeEntry(te[4], driverB, new TimeOnly(5, 5 ,5)),
            AnyTimeEntry(te[5], driverC, new TimeOnly(6, 6 ,6)),
            AnyTimeEntry(te[6], driverD, new TimeOnly(7, 7 ,7))
        ];


        //Act
        Dictionary<Guid, int> result = _sut.CalculateGeneralStagePoints(timeEntries);

        //Assert
        Assert.That(result, Has.Count.EqualTo(4));
        Assert.That(result, Is.EquivalentTo(new Dictionary<Guid, int> {
            { te[0], PointsProvider._generalPoints[0] },
            { te[2], PointsProvider._generalPoints[1] },
            { te[3], PointsProvider._generalPoints[2] },
            { te[6], PointsProvider._generalPoints[3] },
        }));
    }

    [Test]
    public void CalculateGeneralStagePoints_MoreThan30Drivers_LastsDontGetPoints()
    {
        //Arrange
        const int cnt = 40;
        Guid[] te = Enumerable.Range(0, cnt).Select(_ => Guid.NewGuid()).ToArray();
        Guid[] drivers = Enumerable.Range(0, cnt).Select(_ => Guid.NewGuid()).ToArray();

        List<TimeEntry> timeEntries = Enumerable.Range(0, cnt)
            .Select(i => AnyTimeEntry(te[i], drivers[i], new TimeOnly(1, i, 0))).ToList();

        //Act
        Dictionary<Guid, int> result = _sut.CalculateGeneralStagePoints(timeEntries);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            foreach (var driver in Enumerable.Range(30, cnt - 30).Select(i => drivers[i]))
            {
                Assert.That(result, Does.Not.ContainKey(driver));
            }
        }
    }

    [Test]
    public void CalculateGeneralStagePoints_TwoDriversHaveDraw_ThirdDriverLosesTwoPlaces()
    {
        //Arrange
        Guid[] te = Enumerable.Range(0, 8).Select(_ => Guid.NewGuid()).ToArray();

        Guid driverA = Guid.NewGuid();
        Guid driverB = Guid.NewGuid();
        Guid driverC = Guid.NewGuid();
        Guid driverD = Guid.NewGuid();
        Guid driverE = Guid.NewGuid();

        List<TimeEntry> timeEntries = [
            AnyTimeEntry(te[0], driverA, new TimeOnly(1, 1 ,1)), // <-- best A
            AnyTimeEntry(te[1], driverB, new TimeOnly(2, 2 ,2)), // <-- best B
            AnyTimeEntry(te[2], driverA, new TimeOnly(3, 3 ,3)),
            AnyTimeEntry(te[3], driverC, new TimeOnly(4, 4 ,4)), // <-- best C
            AnyTimeEntry(te[4], driverB, new TimeOnly(4, 4 ,4)),
            AnyTimeEntry(te[5], driverD, new TimeOnly(4, 4 ,4)), // <-- best D (=C)
            AnyTimeEntry(te[6], driverB, new TimeOnly(5, 5 ,5)),
            AnyTimeEntry(te[7], driverE, new TimeOnly(6, 6 ,6)), // <-- best E
        ];

        //Act
        Dictionary<Guid, int> result = _sut.CalculateGeneralStagePoints(timeEntries);

        //Assert
        Assert.That(result, Has.Count.EqualTo(5));
        Assert.That(result, Is.EquivalentTo(new Dictionary<Guid, int> {
            { te[0], PointsProvider._generalPoints[0] },
            { te[1], PointsProvider._generalPoints[1] },
            { te[3], PointsProvider._generalPoints[2] },
            { te[5], PointsProvider._generalPoints[2] },
            { te[7], PointsProvider._generalPoints[4] },
        }));
    }

    [Test]
    public void CalculateCarStagePoints_OneDriverHasFirstAndSecondPlacePerCar_OtherDriverHasSecondPlacePerCar()
    {
        //Arrange
        Guid[] te = Enumerable.Range(0, 8).Select(_ => Guid.NewGuid()).ToArray();

        Guid driverA = Guid.NewGuid();
        Guid driverB = Guid.NewGuid();
        Guid driverC = Guid.NewGuid();
        Guid driverD = Guid.NewGuid();

        List<TimeEntry> timeEntries = [
            AnyTimeEntry(te[0], driverA, 1, new TimeOnly(1, 1 ,1)), // <-- best A 1
            AnyTimeEntry(te[1], driverA, 2, new TimeOnly(2, 2 ,2)), // <-- best A 2
            AnyTimeEntry(te[2], driverB, 3, new TimeOnly(3, 3 ,3)), // <-- best B 3
            AnyTimeEntry(te[3], driverC, 2, new TimeOnly(4, 4 ,4)), // <-- best C 2 (2nd)
            AnyTimeEntry(te[4], driverA, 1, new TimeOnly(5, 5 ,5)),
            AnyTimeEntry(te[5], driverB, 3, new TimeOnly(6, 6 ,6)),
            AnyTimeEntry(te[6], driverC, 3, new TimeOnly(7, 7 ,7)), // <-- best C 3 (2nd)
            AnyTimeEntry(te[7], driverD, 1, new TimeOnly(8, 8 ,8)), // <-- best D 1 (2nd)
        ];

        Dictionary<int, int> carId2class = new()
        {
            { 1, 5 },
            { 2, 6 },
            { 3, 7 },
        };

        timeEntries.ForEach(te => FillTimeEntryWithCar(te, carId2class));

        //Act
        Dictionary<Guid, int> result = _sut.CalculateCarStagePoints(timeEntries);

        //Assert
        Assert.That(result, Has.Count.EqualTo(6));
        Assert.That(result, Is.EquivalentTo(new Dictionary<Guid, int> {
            { te[0], PointsProvider._a5carPoints[0] },
            { te[1], PointsProvider._a6carPoints[0] },
            { te[2], PointsProvider._a7carPoints[0] },
            { te[3], PointsProvider._a6carPoints[1] },
            { te[6], PointsProvider._a7carPoints[1] },
            { te[7], PointsProvider._a5carPoints[1] },
        }));
    }

    [Test]
    public void CalculateCarStagePoints_MoreThan5Drivers_LastsDontGetPoints()
    {
        //Arrange
        const int cnt = 10;
        Guid[] te = Enumerable.Range(0, cnt).Select(_ => Guid.NewGuid()).ToArray();
        Guid[] drivers = Enumerable.Range(0, cnt).Select(_ => Guid.NewGuid()).ToArray();

        const int carId = 1;
        List<TimeEntry> timeEntries = Enumerable.Range(0, cnt)
            .Select(i => AnyTimeEntry(te[i], drivers[i], carId, new TimeOnly(1, i, 0))).ToList();

        Dictionary<int, int> carId2class = new() { { carId, 5 } };
        timeEntries.ForEach(te => FillTimeEntryWithCar(te, carId2class));

        //Act
        Dictionary<Guid, int> result = _sut.CalculateCarStagePoints(timeEntries);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            foreach (var driver in Enumerable.Range(5, cnt - 5).Select(i => drivers[i]))
            {
                Assert.That(result, Does.Not.ContainKey(driver));
            }
        }
    }

    [Test]
    public void CalculateCarStagePoints_TwoDriversHaveDraw_ThirdDriverLosesTwoPlaces()
    {
        //Arrange
        Guid[] te = Enumerable.Range(0, 8).Select(_ => Guid.NewGuid()).ToArray();

        Guid driverA = Guid.NewGuid();
        Guid driverB = Guid.NewGuid();
        Guid driverC = Guid.NewGuid();
        Guid driverD = Guid.NewGuid();
        Guid driverE = Guid.NewGuid();

        List<TimeEntry> timeEntries = [
            AnyTimeEntry(te[0], driverA, 1, new TimeOnly(1, 1, 1)), // <-- best A 1
            AnyTimeEntry(te[1], driverB, 2, new TimeOnly(2, 2, 2)), // <-- best B 2
            AnyTimeEntry(te[2], driverA, 2, new TimeOnly(2, 2, 2)), // <-- best A 2 (=B 2)
            AnyTimeEntry(te[3], driverC, 3, new TimeOnly(4, 4, 4)), // <-- best C 3
            AnyTimeEntry(te[4], driverB, 2, new TimeOnly(4, 4, 4)),
            AnyTimeEntry(te[5], driverD, 3, new TimeOnly(4, 4, 4)), // <-- best D 3 (=C 3)
            AnyTimeEntry(te[6], driverB, 1, new TimeOnly(5, 5, 5)), // <-- best B 1 (2nd)
            AnyTimeEntry(te[7], driverE, 2, new TimeOnly(6, 6, 6)), // <-- best E 2 (3rd)
        ];

        Dictionary<int, int> carId2class = new()
        {
            { 1, 5 },
            { 2, 6 },
            { 3, 7 },
        };

        timeEntries.ForEach(te => FillTimeEntryWithCar(te, carId2class));

        //Act
        Dictionary<Guid, int> result = _sut.CalculateCarStagePoints(timeEntries);

        //Assert
        Assert.That(result, Has.Count.EqualTo(7));
        Assert.That(result, Is.EquivalentTo(new Dictionary<Guid, int> {
            { te[0], PointsProvider._a5carPoints[0] },
            { te[1], PointsProvider._a6carPoints[0] },
            { te[2], PointsProvider._a6carPoints[0] },
            { te[3], PointsProvider._a7carPoints[0] },
            { te[5], PointsProvider._a7carPoints[0] },
            { te[6], PointsProvider._a5carPoints[1] },
            { te[7], PointsProvider._a6carPoints[2] },
        }));
    }

    private static TimeEntry AnyTimeEntry(Guid id, Guid driverId, int carId, TimeOnly time)
    {
        return new TimeEntry()
        {
            CarId = carId,
            DriverId = driverId,
            Id = id,
            StageId = 1,
            Time = time,
            UploadTime = DateTime.MinValue
        };
    }

    private static void FillTimeEntryWithCar(TimeEntry te, Dictionary<int, int> carId2class)
    {
        te.Car = new Car() {
            Id = te.CarId,
            Name = te.CarId.ToString(),
            Class = carId2class[te.CarId] 
        };
    }

    private static TimeEntry AnyTimeEntry(Guid id, Guid driverId, TimeOnly time) =>
        AnyTimeEntry(id, driverId, 1, time);

}