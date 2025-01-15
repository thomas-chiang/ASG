using System.Reflection.Metadata;
using ASG.Infrastructure.Common.SqlServerDbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestCommon.TestConstants;

namespace ASG.Application.SubcutaneousTests.Common;


/// <summary>
/// In Subcutaneous tests we aren't testing integration with a real database,
/// so even if we weren't using sql server of docker we would use some in-memory database.
/// </summary>
public class SqlServerTestDatabase : IDisposable
{
    public SqlConnection Connection { get; }

    public static SqlServerTestDatabase CreateAndInitialize()
    {
        var testDatabase = new SqlServerTestDatabase("Server=localhost,1433;User=sa;Password=YourStrong@Password;Database=TestDb;Encrypt=False;");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        var options = new DbContextOptionsBuilder<AsiaFlowDbContext>()
            .UseSqlServer(Connection)
            .Options;

        using var context = new AsiaFlowDbContext(options);
        context.Database.EnsureCreated();
        
        // Drop the tables if they already exist and recreate them
        context.Database.ExecuteSqlRaw(@"
            IF OBJECT_ID('gbpm.fm_form_header', 'U') IS NOT NULL
                DROP TABLE gbpm.fm_form_header;
        ");
        
        // Create the tables
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE gbpm.fm_form_header (
                form_kind NVARCHAR(50),
                form_no INT PRIMARY KEY,
                form_status NVARCHAR(10),
                form_date DATETIME
            );
           ");

        // Insert fake data into gbpm.fm_form_header
        context.Database.ExecuteSqlRaw(@"
                INSERT INTO gbpm.fm_form_header (form_kind, form_no, form_status, form_date)
                VALUES 
                    (@FormKind, @FormNo, 'AP', '2025-01-01 08:00:00');
            ", 
            new SqlParameter("@FormKind", Constants.Gaia1001Forms.DefaultFormKind), 
            new SqlParameter("@FormNo", Constants.Gaia1001Forms.DefaultFormNo));

        // Drop the tables if they already exist and recreate them
        context.Database.ExecuteSqlRaw($@"
                IF OBJECT_ID('gbpm.{Constants.Gaia1001Forms.DefaultFormKind.Replace(".", "")}', 'U') IS NOT NULL
                    DROP TABLE gbpm.{Constants.Gaia1001Forms.DefaultFormKind.Replace(".", "")};
            ");
        
        // Create the tables
        context.Database.ExecuteSqlRaw($@"
                CREATE TABLE gbpm.{Constants.Gaia1001Forms.DefaultFormKind.Replace(".", "")} (
                    form_no INT PRIMARY KEY,
                    ATTENDANCETYPE NVARCHAR(50),
                    DATETIME DATETIME,
                    TIMEZONE NVARCHAR(10),
                    NAME NVARCHAR(50),
                    LOCATION NVARCHAR(100),
                    LOCATIONINFO NVARCHAR(100),
                    REASON NVARCHAR(255),
                    ADVANCEWORKTYPE NVARCHAR(50),
                    POSTPONEWORKTYPE NVARCHAR(50),
                    ADVANCEREASONLIST NVARCHAR(255),
                    POSTPONEREASONLIST NVARCHAR(255),
                    CHECKINPERSONALREASON NVARCHAR(255),
                    USERTUBECOMPANYID NVARCHAR(50),
                    USERTUBEEMPID NVARCHAR(50)
                );
            ");
        
        // Insert fake data into gbpm.fm_form_header
        context.Database.ExecuteSqlRaw($@"
                INSERT INTO gbpm.{Constants.Gaia1001Forms.DefaultFormKind.Replace(".", "")} (
                    form_no, ATTENDANCETYPE, DATETIME, TIMEZONE, NAME, LOCATION, LOCATIONINFO, REASON, 
                    ADVANCEWORKTYPE, POSTPONEWORKTYPE, ADVANCEREASONLIST, POSTPONEREASONLIST, CHECKINPERSONALREASON, 
                    USERTUBECOMPANYID, USERTUBEEMPID)
                VALUES 
                    (@FormNo, @ATTENDANCETYPE1, '2025-01-01T08:00:00', '+08:00', 'John Doe', @Location1, 'Main Building', 'N/A', '', '', NULL, NULL, 'N/A', @UserTubeCompanyId1, @UserTubeEmpId1);
            ",
            new SqlParameter("@FormNo", Constants.Gaia1001Forms.DefaultFormNo),
            new SqlParameter("@AttendanceType1", ((int)Constants.Gaia1001Forms.DefaultAttendanceType).ToString()),
            new SqlParameter("@Location1", Constants.Gaia1001Forms.DefaultLocationId),
            new SqlParameter("@UserTubeCompanyId1", Constants.Common.DefaultCompanyId),
            new SqlParameter("@UserTubeEmpId1", Constants.Common.DefaultUserEmployeeId)
            );
    }

    public void ResetDatabase()
    {
        Connection.Close();
        InitializeDatabase();
    }

    private SqlServerTestDatabase(string connectionString)
    {
        Connection = new SqlConnection(connectionString);
    }

    public void Dispose()
    {
        Connection.Close();
    }
}
