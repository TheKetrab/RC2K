using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.DomainModel.Exceptions;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic;

public class DriverService : IDriverService
{
    private readonly IUserRepository _userRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly IFillersBag _fillers;

    public DriverService(IUserRepository userRepository, IDriverRepository driverRepository, IFillersBag fillers)
    {
        _userRepository = userRepository;
        _driverRepository = driverRepository;
        _fillers = fillers;
    }

    public async Task<Driver?> GetByName(string name)
    {
        User? user = await _userRepository.GetByName(name);
        if (user is not null)
        {
            FillingContext context = new();
            await _fillers.UserFiller.FillRecursive(user, context, _fillers);
            return user.Driver;
        }

        Driver? anonymousDriver = await _driverRepository.GetByName(name);
        if (anonymousDriver is not null) 
        {
            FillingContext context = new();
            await _fillers.DriverFiller.FillRecursive(anonymousDriver, context, _fillers);
            return anonymousDriver;
        }

        return null;
    }
}