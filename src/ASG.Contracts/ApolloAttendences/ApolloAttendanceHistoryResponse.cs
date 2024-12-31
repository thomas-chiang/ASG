namespace ASG.Contracts.ApolloAttendences;

public record ApolloAttendanceHistoryResponse(DateTime AttendanceOn, string AttendanceMethod, Boolean IsEffective);