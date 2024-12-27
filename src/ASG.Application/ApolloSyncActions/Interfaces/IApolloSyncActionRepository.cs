using ASG.Domain.ApolloSyncActions;

namespace ASG.Application.ApolloSyncActions.Interfaces;

public interface IApolloSyncActionRepository
{
    Task AddApolloSyncActionAsync(ApolloSyncAction apolloSyncAction);
}