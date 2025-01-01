using ASG.Application.ApolloSyncGaia1001FormOperations.Interfaces;
using ASG.Domain.ApolloSyncGaia1001FormOperation;

namespace ASG.Infrastructure.ApolloSyncActions.Persistence;

public class ApolloSyncActionRepository : IApolloSyncActionRepository
{
    private readonly List<ApolloSyncGaia1001FormOperation> _apolloSyncActions = new();
    
    public async Task AddApolloSyncActionAsync(ApolloSyncGaia1001FormOperation apolloSyncGaia1001FormOperation)
    {
        _apolloSyncActions.Add(apolloSyncGaia1001FormOperation);
    }
}