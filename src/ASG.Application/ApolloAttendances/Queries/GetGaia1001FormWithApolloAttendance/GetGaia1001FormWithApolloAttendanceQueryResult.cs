using ASG.Domain.ApolloAttendances;
using ASG.Domain.Gaia1001Forms;

namespace ASG.Application.ApolloAttendances.Queries.GetGaia1001FormWithApolloAttendance;

public record GetGaia1001FormWithApolloAttendanceQueryResult(
    Gaia1001Form Gaia1001Form,
    ApolloAttendance ApolloAttendance);