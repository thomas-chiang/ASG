using ASG.Domain.ApolloSyncGaia1001FormOperation;

namespace ASG.Application.ApolloSyncActions.Interfaces;

public interface IApolloSyncActionRepository
{
    Task AddApolloSyncActionAsync(ApolloSyncGaia1001FormOperation apolloSyncGaia1001FormOperation);
}