namespace ASG.Contracts.ApolloAttendences;

public record Apollo1001FromResponse(
    string? AttendanceHistoryId,
    string? FormKind,
    int? FormNo,
    string? ApprovalStatus);