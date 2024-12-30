namespace ASG.Contracts.ApolloAttendances;

public record GetApolloAttendanceRequest(
    string FormKind,
    int FormNo
);