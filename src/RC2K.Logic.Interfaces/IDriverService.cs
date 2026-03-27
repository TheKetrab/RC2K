using RC2K.DomainModel;

namespace RC2K.Logic.Interfaces;

public interface IDriverService
{
    Task<Driver?> GetById(Guid id, CancellationToken ct);
    Task<Driver?> GetByName(string name);
    Task<Dictionary<Guid, string>> GetAllNames();
    Task<Result<Driver>> CreateAnonymous(string name, string? nationality);
}
