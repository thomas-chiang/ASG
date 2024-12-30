namespace ASG.Contracts.ApolloAttendances;

public record ApolloAttendanceHistoryResponse(DateTime AttendanceOn, string AttendanceMethod, Boolean IsEffective);