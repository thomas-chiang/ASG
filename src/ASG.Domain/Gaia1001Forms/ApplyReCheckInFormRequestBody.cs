using ASG.Domain.Common;

namespace ASG.Domain.Gaia1001Forms;

public record ApplyReCheckInFormRequestBody(
    bool IsBehalf,
    int SourceFormNo,
    string SourceFormKind,
    string CompanyId,
    string EmployeeId,
    string UserEmployeeId,
    int AttendanceType,
    string AttendanceOn,
    string PunchesLocationId,
    string? LocationDetails,
    string? ReasonsForMissedClocking,
    string? ExtendWorkHourType,
    string? CheckInTimeoutType,
    string? CheckInPersonalReasonTypeId,
    string? CheckInPersonalReason,
    bool? IsApproved
): RequestBody;