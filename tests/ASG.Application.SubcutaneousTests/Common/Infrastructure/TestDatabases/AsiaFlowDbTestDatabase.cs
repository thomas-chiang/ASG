using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestCommon.TestConstants;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;

public class AsiaFlowDbTestDatabase : SqlServerDbTestDatabase<AsiaFlowDbContext>
{
    public static AsiaFlowDbTestDatabase CreateAndInitialize()
    {
        var testDatabase = new AsiaFlowDbTestDatabase(
            "Server=localhost,1433;User=sa;Password=YourStrong@Password;Database=AsiaFlowDbTestDatabase;Encrypt=False;");
        testDatabase.InitializeDatabase();
        return testDatabase;
    }

    private AsiaFlowDbTestDatabase(string connectionString) : base(connectionString)
    {
    }
}