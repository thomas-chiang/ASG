using System.Net;
using ASG.Application.SubcutaneousTests.Common;
using ASG.Application.SubcutaneousTests.Common.Infrastructure.TestDatabases;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TestCommon.ApolloSyncGaia1001FormOperations;
using TestCommon.TestConstants;

namespace ASG.Application.SubcutaneousTests.ApolloSyncGaia1001FormOperations.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateApolloSyncGaia1001FormOperationTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task
        CreateApolloSyncGaia1001FormOperation_WhenValidCommandAndApolloAlreadyHasClockInRecord_ShouldCreateApolloSyncGaia1001FormOperationOfSituationAlreadyHasClockInRecord()
    {
        // Arrange
        await ArrangeGaia1001FormIsApprovedAndApolloHasClientInRecord(
            mediatorFactory.AsiaFlowDbTestDatabase,
            mediatorFactory.AsiaTubeManageDbTestDatabase,
            mediatorFactory.AsiaTubeDbTestDatabase);


        var createApolloSyncGaia1001FormOperationCommand = ApolloSyncGaia1001FormOperationCommandFactory
            .CreateCreateApolloSyncGaia1001FormOperationCommand();

        // Act
        var createApolloSyncGaia1001FormOperationResult =
            await _mediator.Send(createApolloSyncGaia1001FormOperationCommand);

        // Assert
        createApolloSyncGaia1001FormOperationResult.IsError.Should().BeFalse();
        createApolloSyncGaia1001FormOperationResult.Value.Situation.Should()
            .Be(Sync1001FormSituation.AlreadyHasClockInRecord);
    }

    [Fact]
    public async Task
        CreateApolloSyncGaia1001FormOperation_WhenValidCommandAndNoApolloRecord_ShouldPerformAnonymousPostRequest()
    {
        // Arrange
        await SetUpGaia1001FormIsApproved(mediatorFactory.AsiaFlowDbTestDatabase);
        await InsertCompany(mediatorFactory.AsiaTubeManageDbTestDatabase);

        var createApolloSyncGaia1001FormOperationCommand = ApolloSyncGaia1001FormOperationCommandFactory
            .CreateCreateApolloSyncGaia1001FormOperationCommand();

        // Act
        var createApolloSyncGaia1001FormOperationResult =
            await _mediator.Send(createApolloSyncGaia1001FormOperationCommand);

        // Assert
        createApolloSyncGaia1001FormOperationResult.IsError.Should().BeFalse();
        createApolloSyncGaia1001FormOperationResult.Value.AnonymousRequests.Should()
            .HaveCount(1, "there should be exactly one anonymous request");
        createApolloSyncGaia1001FormOperationResult.Value.AnonymousRequests.First().StatusCode.Should()
            .Be(HttpStatusCode.InternalServerError);
    }

    [Theory]
    [InlineData("1001")]
    [InlineData("9.001")]
    [InlineData("001")]
    public async Task
        CreateApolloSyncGaia1001FormOperation_WhenCommandContainsInvalidFormKind_ShouldReturnValidationError(
            string formKind)
    {
        // Arrange
        var createApolloSyncGaia1001FormOperationCommand = ApolloSyncGaia1001FormOperationCommandFactory
            .CreateCreateApolloSyncGaia1001FormOperationCommand(formKind);

        // Act
        var createApolloSyncGaia1001FormOperationResult =
            await _mediator.Send(createApolloSyncGaia1001FormOperationCommand);

        // Assert
        createApolloSyncGaia1001FormOperationResult.IsError.Should().BeTrue();
        createApolloSyncGaia1001FormOperationResult.FirstError.Code.Should().Be("FormKind");
    }


    public static async Task ArrangeGaia1001FormIsApprovedAndApolloHasClientInRecord(
        AsiaFlowDbTestDatabase asiaFlowDbTestDatabase,
        AsiaTubeManageDbTestDatabase asiaTubeManageDbTestDatabase,
        AsiaTubeDbTestDatabase asiaTubeDbTestDatabase)
    {
        await SetUpGaia1001FormIsApproved(asiaFlowDbTestDatabase);

        await InsertCompany(asiaTubeManageDbTestDatabase);

        await InsertApolloClockInRecord(asiaTubeDbTestDatabase);
    }


    private static async Task InsertCompany(AsiaTubeManageDbTestDatabase asiaTubeManageDbTestDatabase)
    {
        await asiaTubeManageDbTestDatabase.Context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO [Company] (CompanyId, CompanyCode, CompanyName)
            VALUES (@CompanyId, @CompanyCode, @CompanyName)",
            new SqlParameter("@CompanyId", Constants.Common.DefaultCompanyId),
            new SqlParameter("@CompanyCode", "DefaultCompanyCode"),
            new SqlParameter("@CompanyName", "DefaultCompanyName")
        );
    }

    private static async Task InsertApolloClockInRecord(AsiaTubeDbTestDatabase asiaTubeDbTestDatabase)
    {
        await asiaTubeDbTestDatabase.Context.Database.ExecuteSqlRawAsync(@"
            INSERT INTO [pt].[AttendanceHistory] 
            (AttendanceHistoryId, EmployeeId, iOriginType, iAttendanceType, AttendanceDate, 
             AttendanceOn, IsDeleted, IsEffect, PunchesLocationId, LocationName, Latitude, 
             Longitude, LocationDetails, ExtendWorkHourType, CheckInTimeoutType, 
             CheckInPersonalReasonTypeId, CheckInPersonalReasonCode, CheckInPersonalReason, 
             CompanyId, Creater, CreateTime, LatestUpdater, LatestUpdateTime, IdentifyCode, 
             EditTimes, iLastType, AdjustCheckInTimeoutType, AdjustCheckInPersonalReasonTypeId, 
             AdjustCheckInPersonalReasonCode, AdjustCheckInPersonalReason)
            VALUES 
            (@AttendanceHistoryId, @EmployeeId, @iOriginType, @iAttendanceType, @AttendanceDate, 
             @AttendanceOn, @IsDeleted, @IsEffect, @PunchesLocationId, @LocationName, @Latitude, 
             @Longitude, @LocationDetails, @ExtendWorkHourType, @CheckInTimeoutType, 
             @CheckInPersonalReasonTypeId, @CheckInPersonalReasonCode, @CheckInPersonalReason, 
             @CompanyId, @Creater, @CreateTime, @LatestUpdater, @LatestUpdateTime, @IdentifyCode, 
             @EditTimes, @iLastType, @AdjustCheckInTimeoutType, @AdjustCheckInPersonalReasonTypeId, 
             @AdjustCheckInPersonalReasonCode, @AdjustCheckInPersonalReason)",
            new SqlParameter("@AttendanceHistoryId", Guid.NewGuid()),
            new SqlParameter("@EmployeeId", Constants.Common.DefaultUserEmployeeId),
            new SqlParameter("@iOriginType", 8), // Example value: 匯入
            new SqlParameter("@iAttendanceType",
                (int)Constants.Gaia1001Forms.DefaultAttendanceType), // Example value: ClockIn
            new SqlParameter("@AttendanceDate", Constants.Common.DefaultAttendanceOn),
            new SqlParameter("@AttendanceOn", Constants.Common.DefaultAttendanceOn),
            new SqlParameter("@IsDeleted", false),
            new SqlParameter("@IsEffect", true),
            new SqlParameter("@PunchesLocationId", Constants.Gaia1001Forms.DefaultLocationId),
            new SqlParameter("@LocationName", DBNull.Value),
            new SqlParameter("@Latitude", DBNull.Value),
            new SqlParameter("@Longitude", DBNull.Value),
            new SqlParameter("@LocationDetails", DBNull.Value),
            new SqlParameter("@ExtendWorkHourType", DBNull.Value),
            new SqlParameter("@CheckInTimeoutType", DBNull.Value),
            new SqlParameter("@CheckInPersonalReasonTypeId", DBNull.Value),
            new SqlParameter("@CheckInPersonalReasonCode", DBNull.Value),
            new SqlParameter("@CheckInPersonalReason", DBNull.Value),
            new SqlParameter("@CompanyId", Constants.Common.DefaultCompanyId),
            new SqlParameter("@Creater", Constants.Common.DefaultUserEmployeeId),
            new SqlParameter("@CreateTime", DateTime.Now),
            new SqlParameter("@LatestUpdater", Constants.Common.DefaultUserEmployeeId),
            new SqlParameter("@LatestUpdateTime", DateTime.Now),
            new SqlParameter("@IdentifyCode", DBNull.Value),
            new SqlParameter("@EditTimes", DBNull.Value),
            new SqlParameter("@iLastType", DBNull.Value),
            new SqlParameter("@AdjustCheckInTimeoutType", DBNull.Value),
            new SqlParameter("@AdjustCheckInPersonalReasonTypeId", DBNull.Value),
            new SqlParameter("@AdjustCheckInPersonalReasonCode", DBNull.Value),
            new SqlParameter("@AdjustCheckInPersonalReason", DBNull.Value)
        );
    }

    private static async Task SetUpGaia1001FormIsApproved(AsiaFlowDbTestDatabase asiaFlowDbTestDatabase)
    {
        await asiaFlowDbTestDatabase.Context.Database.ExecuteSqlRawAsync(@"
            IF OBJECT_ID('gbpm.fm_form_header', 'U') IS NOT NULL
                DROP TABLE gbpm.fm_form_header;
        ");

        // Create the tables
        await asiaFlowDbTestDatabase.Context.Database.ExecuteSqlRawAsync(@"
            CREATE TABLE gbpm.fm_form_header (
                form_kind NVARCHAR(50),
                form_no INT PRIMARY KEY,
                form_status NVARCHAR(10),
                form_date DATETIME
            );
           ");

        // Insert fake data
        await asiaFlowDbTestDatabase.Context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO gbpm.fm_form_header (form_kind, form_no, form_status, form_date)
                VALUES 
                    (@FormKind, @FormNo, 'AP', '2025-01-01 08:00:00');
            ",
            new SqlParameter("@FormKind", Constants.Gaia1001Forms.DefaultFormKind),
            new SqlParameter("@FormNo", Constants.Gaia1001Forms.DefaultFormNo));

        // Drop the tables if they already exist and recreate them
        await asiaFlowDbTestDatabase.Context.Database.ExecuteSqlRawAsync($@"
                IF OBJECT_ID('gbpm.{Constants.Gaia1001Forms.DefaultFormKind.Replace(".", "")}', 'U') IS NOT NULL
                    DROP TABLE gbpm.{Constants.Gaia1001Forms.DefaultFormKind.Replace(".", "")};
            ");

        // Create the tables
        await asiaFlowDbTestDatabase.Context.Database.ExecuteSqlRawAsync($@"
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

        // Insert fake data 
        await asiaFlowDbTestDatabase.Context.Database.ExecuteSqlRawAsync($@"
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
}