using ASG.Domain.ApolloSyncGaia1001FormOperation;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncAction;

public record CreateApolloSyncGaia1001FormOperationCommand(
    string FormKind, int FormNo
    ) : IRequest<ErrorOr<ApolloSyncGaia1001FormOperation>>;