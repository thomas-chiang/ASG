namespace ASG.Contracts.Gaia1001Forms;

public record ApplyReCheckInFormJsonResponse(
    bool IsBehalf,
    int SourceFormNo,
    string SourceFormKind,
    string Status,
    string CompanyId,
    string EmployeeId,
    string UserEmployeeId,
    int AttendanceType,
    string AttendanceOn,
    string PunchesLocationId,
    string? LocationDetails,
    string ReasonsForMissedClocking,
    string? ExtendWorkHourType,
    string? CheckInTimeoutType,
    string? CheckInPersonalReasonTypeId,
    string? CheckInPersonalReason
);