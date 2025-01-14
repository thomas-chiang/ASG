using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ASG.Application.SubcutaneousTests.Common;


/// <summary>
/// In Subcutaneous tests we aren't testing integration with a real database,
/// so even if we weren't using SQLite we would use some in-memory database.
/// </summary>
public class InMemoryTestDatabase : IDisposable
{
    public string InMemoryDatabaseName = "TestDatabase";
    public static InMemoryTestDatabase CreateAndInitialize()
    {
        var testDatabase = new InMemoryTestDatabase();

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        var options = new DbContextOptionsBuilder<AsiaFlowDbContext>()
            .UseInMemoryDatabase(InMemoryDatabaseName)
            .Options;

        var context = new AsiaFlowDbContext(options);

        context.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        InitializeDatabase();
    }

    public void Dispose()
    {
        // No cleanup needed for in-memory database.
    }
}