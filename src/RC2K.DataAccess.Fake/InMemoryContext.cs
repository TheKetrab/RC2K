using RC2K.DataAccess.Fake.Repositories;
using RC2K.DataAccess.Interfaces;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Fake
{
    public class InMemoryDataAccess : IDataContext
    {
        public InMemoryDataAccess(
            int carsCnt, int driversCnt, int stagesCnt, int timeEntriesCnt,
            int usersCnt, int stageWaypointsCnt, int verifyInfosCnt)
        {
            Cars = new CarsDataSet(this).Generate(carsCnt);
            Users = new UsersDataSet(this).Generate(usersCnt);
            Drivers = new DriversDataSet(this).Generate(driversCnt);
            Stages = new StagesDataSet(this).Generate(0);
            StagesData = new StagesDataDataSet(this).Generate(stagesCnt);
            TimeEntries = new TimeEntriesDataSet(this).Generate(timeEntriesCnt);
            StageWaypoints = new StagesWaypointsDataSet().Generate(0);
            VerifyInfos = new VerifyInfosDataSet(this).Generate(verifyInfosCnt);
        }


        public IQueryable<Car> Cars { get; set; }

        public IQueryable<Driver> Drivers { get; set; }

        public IQueryable<Stage> Stages { get; set; }

        public IQueryable<StageData> StagesData { get; set; }

        public IQueryable<StageWaypoints> StageWaypoints { get; set; }

        public IQueryable<TimeEntry> TimeEntries { get; set; }

        public IQueryable<User> Users { get; set; }

        public IQueryable<VerifyInfo> VerifyInfos { get; set; }
    }
}



