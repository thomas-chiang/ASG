using ASG.Application.ApolloAttendances.Interfaces;
using ASG.Application.Common.Interfaces;
using ASG.Application.Gaia1001Forms.Interfaces;
using ASG.Domain.ApolloSyncGaia1001FormOperations;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Enums;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation;

public class CreateApolloSyncGaia1001FormOperationCommandHandler :　IRequestHandler<
    CreateApolloSyncGaia1001FormOperationCommand, ErrorOr<ApolloSyncGaia1001FormOperation>>
{
    private readonly IApolloAttendanceRepository _apolloAttendanceRepository;
    private readonly IGaia1001FormRepository _gaia1001FormRepository;
    private readonly IAnonymousRequestSender _anonymousRequestSender;
    private readonly IAsiaTubeDbSetter _asiaTubeDbSetter;
    private readonly IDomainEventAdapter _domainEventAdapter;


    public CreateApolloSyncGaia1001FormOperationCommandHandler(
        IApolloAttendanceRepository apolloAttendanceRepository,
        IGaia1001FormRepository gaia1001FormRepository,
        IAnonymousRequestSender anonymousRequestSender,
        IAsiaTubeDbSetter asiaTubeDbSetter, IDomainEventAdapter domainEventAdapter)
    {
        _apolloAttendanceRepository = apolloAttendanceRepository;
        _gaia1001FormRepository = gaia1001FormRepository;
        _anonymousRequestSender = anonymousRequestSender;
        _asiaTubeDbSetter = asiaTubeDbSetter;
        _domainEventAdapter = domainEventAdapter;
    }

    public async Task<ErrorOr<ApolloSyncGaia1001FormOperation>> Handle(
        CreateApolloSyncGaia1001FormOperationCommand command, CancellationToken cancellationToken)
    {
        var gaia1001Form = await _gaia1001FormRepository.GetGaia1001Form(command.FormKind, command.FormNo);

        if (gaia1001Form == null)
            return CreateApolloSyncGaia1001FormOperationCommandErrors.Gaia1001FormNotFound(command.FormKind,
                command.FormNo);

        await _asiaTubeDbSetter.SetAsiaTubeDb(gaia1001Form.CompanyId);

        var apolloAttendance = await _apolloAttendanceRepository.GetApolloAttendance(
            gaia1001Form.CompanyId,
            gaia1001Form.UserEmployeeId,
            DateOnly.FromDateTime(gaia1001Form.AttendanceOn),
            gaia1001Form.AttendanceType
        );

        var apolloSyncGaia1001FormOperation = new ApolloSyncGaia1001FormOperation
        {
            Gaia1001Form = gaia1001Form,
            ApolloAttendance = apolloAttendance
        };

        if (ApolloSyncGaia1001FormOperation.HasOtherEffectiveAttendanceMethod(
                apolloAttendance.ApolloAttendanceHistories, apolloAttendance.Apollo1001Forms, command.FormNo,
                gaia1001Form.FormStatus))
        {
            apolloSyncGaia1001FormOperation.SetSituationWithExistedEffectiveAttendance();
            return apolloSyncGaia1001FormOperation;
        }

        var apollo1001FormMatchingGaiaFormNo =
            ApolloSyncGaia1001FormOperation.GetApollo1001FormMatchingGaia1001Form(apolloAttendance.Apollo1001Forms,
                command.FormKind, command.FormNo);
        apolloSyncGaia1001FormOperation.SetAnonymousRequestsToBeSent(apollo1001FormMatchingGaiaFormNo);

        apolloSyncGaia1001FormOperation.SendAnonymousRequests();
        await _domainEventAdapter.CollectDomainEvents(apolloSyncGaia1001FormOperation);
        await _domainEventAdapter.HandleDomainEvents();

        var updatedApolloAttendance = await _apolloAttendanceRepository.GetApolloAttendance(
            gaia1001Form.CompanyId,
            gaia1001Form.UserEmployeeId,
            DateOnly.FromDateTime(gaia1001Form.AttendanceOn),
            gaia1001Form.AttendanceType
        );
        apolloSyncGaia1001FormOperation.UpdatedApolloAttendance = updatedApolloAttendance;
        var updatedApollo1001Form =
            ApolloSyncGaia1001FormOperation.GetApollo1001FormMatchingGaia1001Form(
                updatedApolloAttendance.Apollo1001Forms,
                command.FormKind, command.FormNo);

        var operationResult =
            apolloSyncGaia1001FormOperation.SetSituationAfterSendingAnonymousRequests(updatedApollo1001Form);
        if (!operationResult.IsError) return apolloSyncGaia1001FormOperation;
        if (!operationResult.Errors.Contains(
                ApolloSyncGaia1001FormOperationErrors.FailedApolloSyncGaia1001FormOperation))
            return operationResult.Errors;
        apolloSyncGaia1001FormOperation.Situation = Sync1001FormSituation.NeedFurtherInvestigation;
        return apolloSyncGaia1001FormOperation;
    }
}