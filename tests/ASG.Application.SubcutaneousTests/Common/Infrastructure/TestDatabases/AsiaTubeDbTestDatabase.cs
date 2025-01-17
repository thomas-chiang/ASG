using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;

public class AsiaTubeDbTestDatabase : SqlServerDbTestDatabase<AsiaTubeDbContext>
{
    public static AsiaTubeDbTestDatabase CreateAndInitialize()
    {
        var testDatabase = new AsiaTubeDbTestDatabase(
            "Server=localhost,1433;User=sa;Password=YourStrong@Password;Database=AsiaTubeDbTestDatabase;Encrypt=False;");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    private AsiaTubeDbTestDatabase(string connectionString) : base(connectionString) { }
}