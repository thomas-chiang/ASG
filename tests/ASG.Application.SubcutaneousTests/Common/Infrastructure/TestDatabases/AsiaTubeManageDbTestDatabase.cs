using ASG.Infrastructure.Common.AsiaTubeManageDbSchemas;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestCommon.TestConstants;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;

public class AsiaTubeManageDbTestDatabase : IDisposable
{
    public SqlConnection Connection { get; }

    public static AsiaTubeManageDbTestDatabase CreateAndInitialize()
    {
        var testDatabase = new AsiaTubeManageDbTestDatabase("Server=localhost,1433;User=sa;Password=YourStrong@Password;Database=AsiaTubeManageDbTestDatabase;Encrypt=False;");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        var options = new DbContextOptionsBuilder<AsiaTubeManageDbContext>()
            .UseSqlServer(Connection)
            .Options;

        using var context = new AsiaTubeManageDbContext(options);
        context.Database.EnsureCreated();
        if (context.Companies.Any(c => c.CompanyId == Constants.Common.DefaultCompanyId)) return;
        var newCompany = new Company
        {
            CompanyId = Constants.Common.DefaultCompanyId,
            CompanyCode = "DefaultCompanyCode",
            CompanyName = "DefaultCompanyName"
        };

        context.Companies.Add(newCompany);
        context.SaveChanges();
    }

    public void ResetDatabase()
    {
        Connection.Close();
        DropAllTables();
        InitializeDatabase();
    }

    private AsiaTubeManageDbTestDatabase(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public void Dispose()
    {
        DropAllTables();
        Connection.Close();
    }
    
    private void DropAllTables()
    {
        var options = new DbContextOptionsBuilder<AsiaTubeManageDbContext>()
            .UseSqlServer(Connection)
            .Options;

        using var context = new AsiaTubeManageDbContext(options);
    
        // Query all tables in the database and drop them
        var dropTablesSql = @"
        DECLARE @sql NVARCHAR(MAX) = N'';
        
        SELECT @sql += 'DROP TABLE [' + SCHEMA_NAME(schema_id) + '].[' + name + ']; '
        FROM sys.tables;

        EXEC sp_executesql @sql;
    ";

        context.Database.ExecuteSqlRaw(dropTablesSql);
    }
}