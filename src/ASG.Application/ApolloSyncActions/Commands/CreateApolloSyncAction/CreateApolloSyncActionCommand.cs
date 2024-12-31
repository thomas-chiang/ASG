using ASG.Domain.ApolloSyncGaia1001FormOperation;
using MediatR;
using ErrorOr;

namespace ASG.Application.ApolloSyncActions.Commands.CreateApolloSyncAction;

public record CreateApolloSyncActionCommand(
    string FormKindPlusFormNo
    ) : IRequest<ErrorOr<ApolloSyncGaia1001FormOperation>>;