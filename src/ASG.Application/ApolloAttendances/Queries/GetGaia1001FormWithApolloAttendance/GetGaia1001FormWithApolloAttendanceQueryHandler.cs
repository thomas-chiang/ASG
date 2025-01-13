using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.ApolloAttendances;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloAttendances.Queries.GetGaia1001FormWithApolloAttendance;

public class GetGaia1001FormWithApolloAttendanceQueryHandler : IRequestHandler<GetGaia1001FormWithApolloAttendanceQuery,
    ErrorOr<GetGaia1001FormWithApolloAttendanceQueryResult>>
{
    private readonly IApolloAttendanceRepository _apolloAttendanceRepository;
    private readonly IGaia1001FormRepository _gaia1001FormRepository;

    public GetGaia1001FormWithApolloAttendanceQueryHandler(IGaia1001FormRepository gaia1001FormRepository,
        IApolloAttendanceRepository apolloAttendanceRepository)
    {
        _gaia1001FormRepository = gaia1001FormRepository;
        _apolloAttendanceRepository = apolloAttendanceRepository;
    }

    public async Task<ErrorOr<GetGaia1001FormWithApolloAttendanceQueryResult>> Handle(
        GetGaia1001FormWithApolloAttendanceQuery query,
        CancellationToken cancellationToken)
    {
        var gaia1001Form = await _gaia1001FormRepository.GetGaia1001Form(query.FormKind, query.FormNo);

        if (gaia1001Form == null)
            throw new InvalidOperationException(
                $"Gaia1001 form with FormKind '{query.FormKind}' and FormNo '{query.FormNo}' not found.");

        var apolloAttendance = await _apolloAttendanceRepository.GetApolloAttendance(
            gaia1001Form.CompanyId,
            gaia1001Form.UserEmployeeId,
            DateOnly.FromDateTime(gaia1001Form.AttendanceOn),
            gaia1001Form.AttendanceType
        );

        if (apolloAttendance == null)
            throw new InvalidOperationException($"ApolloAttendance not found.");

        return new GetGaia1001FormWithApolloAttendanceQueryResult(gaia1001Form, apolloAttendance);
    }
}