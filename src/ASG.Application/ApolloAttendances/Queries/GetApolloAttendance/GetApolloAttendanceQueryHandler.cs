using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.ApolloAttendances;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloAttendances.Queries.GetApolloAttendance;

public class GetApolloAttendanceQueryHandler : IRequestHandler<GetApolloAttendanceQuery, ErrorOr<ApolloAttendance>>
{
    private readonly IApolloAttendanceRepository _apolloAttendanceRepository;
    private readonly IGaia1001FormRepository _gaia1001FormRepository;
    
    public GetApolloAttendanceQueryHandler(IGaia1001FormRepository gaia1001FormRepository,
        IApolloAttendanceRepository apolloAttendanceRepository)
    {
        _gaia1001FormRepository = gaia1001FormRepository;
        _apolloAttendanceRepository = apolloAttendanceRepository;
    }

    public async Task<ErrorOr<ApolloAttendance>> Handle(GetApolloAttendanceQuery query,
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
        
        return apolloAttendance is null
            ? Error.NotFound(description: "ApolloAttendance not found")
            : apolloAttendance;

        return new ApolloAttendance
        {
            CompanyId = new Guid(),
            UserEmployeeId = new Guid(),
            ApolloAttendanceHistories = null,
            Apollo1001Forms = null
        };
    }
}