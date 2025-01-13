using ASG.Domain.ApolloSyncGaia1001FormOperations;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloSyncGaia1001FormOperations.Commands.CreateApolloSyncGaia1001FormOperation;

public record CreateApolloSyncGaia1001FormOperationCommand(
    string FormKind,
    int FormNo
) : IRequest<ErrorOr<ApolloSyncGaia1001FormOperation>>;