namespace ASG.Contracts.ApolloAttendences;

public record ApolloAttendanceHistoryResponse(
    string? AttendanceHistoryId,
    DateTime AttendanceOn,
    string AttendanceMethod,
    bool IsEffective);