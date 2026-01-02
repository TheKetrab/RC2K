using Microsoft.AspNetCore.Components.Authorization;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Dtos;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Presentation.Blazor.AuthProxy
{
    public class AuthTimeEntryServiceProxy : ITimeEntryService
    {
        private AuthenticationStateProvider _asp;
        private TimeEntryService _service;
        private IDriverRepository _driverRepository;
        private IUserRepository _userRepository;
        private IFillersBag _fillers;

        public AuthTimeEntryServiceProxy(
            AuthenticationStateProvider asp,
            TimeEntryService service,
            IDriverRepository driverRepository,
            IUserRepository userRepository,
            IFillersBag fillers)
        {
            _asp = asp;
            _service = service;
            _driverRepository = driverRepository;
            _userRepository = userRepository;
            _fillers = fillers;
        }

        public async Task<TimeEntriesCollectionInfo> CalculateTimeEntriesWithPoints(int stageId, int maximum = -1)
        {
            var auth = await _asp.GetAuthenticationStateAsync();
            if (!Auth.TryAuthorize(auth))
            {
                if (maximum < 0 || maximum >= 20)
                {
                    maximum = 20;
                }
            }

            var result = await _service.CalculateTimeEntriesWithPoints(stageId, maximum);
            return result;
        }

        public async Task Delete(List<TimeEntry> timeEntries)
        {
            var auth = await _asp.GetAuthenticationStateAsync();
            Auth.Authorize(auth, "admin");

            await _service.Delete(timeEntries);
        }

        public Task<List<TimeEntry>> Get(int stageId, int? carId = null) =>
            _service.Get(stageId, carId);

        public Task<List<TimeEntry>> GetAllNotVerified() =>
            _service.GetAllNotVerified();

        public async Task Upload(
            int stageId, int carId, Guid driverId,
            int min, int sec, int cc,
            List<Proof> proofs, string? labels)
        {
            Driver driver = await _driverRepository.GetById(driverId)
                ?? throw new ArgumentException();

            FillingContext context = new();
            await _fillers.DriverFiller.FillRecursive(driver, context, _fillers);

            await AuthorizeSelf(driver);

            await _service.Upload(
                stageId, carId, driverId,
                min, sec, cc,
                proofs, labels);
        }

        public async Task Upload(TimeEntry timeEntry)
        {
            await AuthorizeSelf(timeEntry.Driver!);
            await _service.Upload(timeEntry);
        }

        public async Task Verify(List<TimeEntry> timeEntries, Guid verifierId, string comment)
        {
            var auth = await _asp.GetAuthenticationStateAsync();
            Auth.Authorize(auth, "admin");

            await _service.Verify(timeEntries, verifierId, comment);
        }

        public async Task Verify(List<TimeEntry> timeEntries, string comment)
        {
            var auth = await _asp.GetAuthenticationStateAsync();
            Auth.Authorize(auth, "admin");

            string name = auth.User.Identity!.Name!;
            var user = await _userRepository.GetByName(name);

            await _service.Verify(timeEntries, user!.Id, comment);
        }

        private async Task AuthorizeSelf(Driver driver)
        {
            string name = driver.Known
                ? driver.User!.Name
                : driver.Name!;

            var auth = await _asp.GetAuthenticationStateAsync();
            Auth.AuthorizeSelf(auth, name);
        }
    }
}
