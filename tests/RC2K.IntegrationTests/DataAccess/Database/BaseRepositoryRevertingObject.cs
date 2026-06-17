using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using RC2K.DataAccess.Database;

namespace RC2K.IntegrationTests.DataAccess.Database;

/// <summary>
/// Before every test it opens transaction scope so that potential changes do not update db data
/// </summary>
public abstract class BaseRepositoryRevertingObject
{
    private IDbContextTransaction _transaction;
    private RallyDbContext _context;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        OnOneTimeSetup();
        _context = IntegrationFixture.ServiceProvider.GetRequiredService<RallyDbContext>();
    }

    protected virtual void OnOneTimeSetup() { }

    [SetUp]
    public async Task Setup()
    {
        _transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted);
    }

    [TearDown]
    public async Task TearDown()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }
}
