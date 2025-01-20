using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;

public abstract class SqlServerDbTestDatabase<TContext> : IDisposable where TContext : DbContext
{
    public SqlConnection Connection { get; }
    public TContext Context { get; }

    protected SqlServerDbTestDatabase(string connectionString)
    {
        Connection = new SqlConnection(connectionString);

        // Initialize the specific DbContext for each derived class
        var options = new DbContextOptionsBuilder<TContext>()
            .UseSqlServer(Connection)
            .Options;

        Context = (TContext)Activator.CreateInstance(typeof(TContext), options)!;
    }

    public virtual void InitializeDatabase()
    {
        Context.Database.EnsureCreated();
    }

    public void ResetDatabase()
    {
        Connection.Close();
        DropAllTables();
        InitializeDatabase();
    }

    public void Dispose()
    {
        DropAllTables();
    }

    protected void DropAllTables()
    {
        var dropTablesSql = @"
            DECLARE @sql NVARCHAR(MAX) = N'';
            
            SELECT @sql += 'DROP TABLE [' + SCHEMA_NAME(schema_id) + '].[' + name + ']; '
            FROM sys.tables;
            
            EXEC sp_executesql @sql;
        ";

        Context.Database.ExecuteSqlRaw(dropTablesSql); // Directly use the `Context` property here
    }
}