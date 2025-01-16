using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;

public class AsiaTubeDbTestDatabase : IDisposable
{
    public SqlConnection Connection { get; }

    public static AsiaTubeDbTestDatabase CreateAndInitialize()
    {
        var testDatabase = new AsiaTubeDbTestDatabase("Server=localhost,1433;User=sa;Password=YourStrong@Password;Database=AsiaTubeDbTestDatabase;Encrypt=False;");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        var options = new DbContextOptionsBuilder<AsiaTubeDbContext>()
            .UseSqlServer(Connection)
            .Options;

        using var context = new AsiaTubeDbContext(options);
        context.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        Connection.Close();
        DropAllTables();
        InitializeDatabase();
    }

    private AsiaTubeDbTestDatabase(string connectionString)
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