using ASG.Contracts.ApolloAttendences;
using ASG.Contracts.Gaia1001Forms;

namespace ASG.Contracts.Gaia1001FormWithApolloAttendances;

public record GetGaia1001FormWithApolloAttendanceResponse(
    GetGaia1001FormResponse Gaia1001Form,
    ApolloAttendanceResponse ApolloAttendance
);