using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloAttendances.Queries.GetGaia1001FormWithApolloAttendance;

public record GetGaia1001FormWithApolloAttendanceQuery(string FormKind, int FormNo)
    : IRequest<ErrorOr<GetGaia1001FormWithApolloAttendanceQueryResult>>;