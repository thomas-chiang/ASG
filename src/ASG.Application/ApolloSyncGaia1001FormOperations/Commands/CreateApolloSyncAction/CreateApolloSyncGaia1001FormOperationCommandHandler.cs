using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Application.Common.Interfaces;
using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.ApolloSyncGaia1001FormOperation;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncAction;

public class CreateApolloSyncGaia1001FormOperationCommandHandler :　IRequestHandler<CreateApolloSyncGaia1001FormOperationCommand, ErrorOr<ApolloSyncGaia1001FormOperation>>
{
    
    private readonly IApolloAttendanceRepository _apolloAttendanceRepository;
    private readonly IGaia1001FormRepository _gaia1001FormRepository;
    private readonly IAnonymousRequestSender _anonymousRequestSender;


    public CreateApolloSyncGaia1001FormOperationCommandHandler(IApolloAttendanceRepository apolloAttendanceRepository, IGaia1001FormRepository gaia1001FormRepository, IAnonymousRequestSender anonymousRequestSender)
    {
        _apolloAttendanceRepository = apolloAttendanceRepository;
        _gaia1001FormRepository = gaia1001FormRepository;
        _anonymousRequestSender = anonymousRequestSender;
    }

    public async Task<ErrorOr<ApolloSyncGaia1001FormOperation>> Handle(CreateApolloSyncGaia1001FormOperationCommand command, CancellationToken cancellationToken)
    {
        var gaia1001Form = await _gaia1001FormRepository.GetGaia1001Form(command.FormKind, command.FormNo);
        
        if (gaia1001Form == null)
            throw new InvalidOperationException(
                $"Gaia1001 form with FormKind '{command.FormKind}' and FormNo '{command.FormNo}' not found.");
        
        var apolloAttendance = await _apolloAttendanceRepository.GetApolloAttendance(
            gaia1001Form.CompanyId,
            gaia1001Form.UserEmployeeId,
            DateOnly.FromDateTime(gaia1001Form.AttendanceOn),
            gaia1001Form.AttendanceType
        );
        
        if (apolloAttendance == null)
            throw new InvalidOperationException($"ApolloAttendance not found.");

        var apolloSyncGaia1001FormOperation = new ApolloSyncGaia1001FormOperation
        {
            Gaia1001Form = gaia1001Form,
            ApolloAttendance = apolloAttendance
        };
        
        apolloSyncGaia1001FormOperation.SetAnonymousRequestsToBeSent();
        
        foreach (var request in apolloSyncGaia1001FormOperation.AnonymousRequests)
        {
            await _anonymousRequestSender.SendAndUpdateAnonymousRequest(request);
        }
        
        var updatedApolloAttendance = await _apolloAttendanceRepository.GetApolloAttendance(
            gaia1001Form.CompanyId,
            gaia1001Form.UserEmployeeId,
            DateOnly.FromDateTime(gaia1001Form.AttendanceOn),
            gaia1001Form.AttendanceType
        );
        
        apolloSyncGaia1001FormOperation.UpdatedApolloAttendance = updatedApolloAttendance;
        
        apolloSyncGaia1001FormOperation.SetSituation();

        return apolloSyncGaia1001FormOperation;
    }
}