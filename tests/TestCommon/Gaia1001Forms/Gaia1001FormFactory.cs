using ASG.Domain.Common.Enums;
using ASG.Domain.Gaia1001Forms;
using ASG.Domain.Gaia1001Forms.Enums;
using TestCommon.TestConstants;

namespace TestCommon.Gaia1001Forms;


public class Gaia1001FormFactory
{
    public static Gaia1001Form CreateGaia1001Form(
        string formKind = Constants.Gaia1001Forms.DefaultFormKind,
        int formNo = Constants.Gaia1001Forms.DefaultFormNo,
        Guid? companyId = null,
        Guid? userEmployeeId = null,
        List<PtSyncFormOperation>? ptSyncFormOperations = null,
        Gaia1001FormStatus gaia1001FormStatus = Constants.Gaia1001Forms.DefaultGaia1001FormStatus,
        AttendanceType attendanceType = Constants.Common.DefaultAttendanceType,
        DateTime? attendanceOn = null,
        bool isBehalf = false,
        Guid? punchesLocationId = null,
        string? locationDetails = null,
        string? reasonsForMissedClocking = null,
        string? extendWorkHourType = null,
        string? checkInTimeoutType = null,
        Guid? checkInPersonalReasonTypeId = null,
        string? checkInPersonalReason = null,
        bool isApproved = false
    )
    {
        return new Gaia1001Form
        {
            FormKind = formKind,
            FormNo = formNo,
            CompanyId = companyId ?? Constants.Common.DefaultCompanyId,
            UserEmployeeId = userEmployeeId ?? Constants.Common.DefaultUserEmployeeId,
            PtSyncFormOperations = ptSyncFormOperations,
            FormStatus = gaia1001FormStatus,
            AttendanceType = attendanceType,
            AttendanceOn = attendanceOn ?? Constants.Common.DefaultAttendanceOn,
            IsBehalf = isBehalf,
            PunchesLocationId = punchesLocationId ?? Constants.Common.DefaultPunchesLocationId,
            LocationDetails = locationDetails,
            ReasonsForMissedClocking = reasonsForMissedClocking,
            ExtendWorkHourType = extendWorkHourType,
            CheckInTimeoutType = checkInTimeoutType,
            CheckInPersonalReasonTypeId = checkInPersonalReasonTypeId,
            CheckInPersonalReason = checkInPersonalReason,
            IsApproved = isApproved
        };
    }
}