using ASG.Infrastructure.Common.AsiaTubeManageDbSchemas;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestCommon.TestConstants;

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

    private AsiaTubeManageDbTestDatabase(string connectionString) : base(connectionString) { }

    public override void InitializeDatabase()
    {
        base.InitializeDatabase();
        Context.Database.ExecuteSqlRaw(@"
            INSERT INTO [Company] (CompanyId, CompanyCode, CompanyName)
            VALUES (@CompanyId, @CompanyCode, @CompanyName)",
            new SqlParameter("@CompanyId", Constants.Common.DefaultCompanyId),
            new SqlParameter("@CompanyCode", "DefaultCompanyCode"),
            new SqlParameter("@CompanyName", "DefaultCompanyName")
        );
    }
}