using ASG.Application.ApolloSyncActions.Interfaces;
using ASG.Domain.ApolloSyncActions;

namespace ASG.Infrastructure.ApolloSyncActions.Persistence;

public class ApolloSyncActionRepository : IApolloSyncActionRepository
{
    private readonly List<ApolloSyncAction> _apolloSyncActions = new();
    
    public async Task AddApolloSyncActionAsync(ApolloSyncAction apolloSyncAction)
    {
        _apolloSyncActions.Add(apolloSyncAction);
    }
}