using ASG.Infrastructure.Common.SqlServerDbContexts;


namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;

public class AsiaTubeManageDbTestDatabase : SqlServerDbTestDatabase<AsiaTubeManageDbContext>
{
    public static AsiaTubeManageDbTestDatabase CreateAndInitialize()
    {
        var testDatabase = new AsiaTubeManageDbTestDatabase(
            "Server=localhost,1433;User=sa;Password=YourStrong@Password;Database=AsiaTubeManageDbTestDatabase;Encrypt=False;");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    private AsiaTubeManageDbTestDatabase(string connectionString) : base(connectionString)
    {
    }
}