namespace ASG.Contracts.Gaia1001FormWithApolloAttendances;

public record GetGaia1001FormWithApolloAttendanceRequest(
    string FormKind,
    int FormNo
);