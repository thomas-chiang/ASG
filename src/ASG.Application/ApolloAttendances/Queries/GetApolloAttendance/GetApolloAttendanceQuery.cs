using ASG.Domain.ApolloAttendances;
using ASG.Domain.Gaia1001Forms;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloAttendances.Queries.GetApolloAttendance;

public record GetApolloAttendanceQuery(string FormKind, int FormNo): IRequest<ErrorOr<ApolloAttendance>>;