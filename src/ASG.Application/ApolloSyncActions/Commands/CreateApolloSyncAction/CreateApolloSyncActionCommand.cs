using ASG.Domain.ApolloSyncActions;
using MediatR;
using ErrorOr;

namespace ASG.Application.ApolloSyncActions.Commands.CreateApolloSyncAction;

public record CreateApolloSyncActionCommand(
    string FormKindPlusFormNo
    ) : IRequest<ErrorOr<ApolloSyncAction>>;