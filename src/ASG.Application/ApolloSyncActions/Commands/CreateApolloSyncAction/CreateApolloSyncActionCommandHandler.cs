using ASG.Application.ApolloSyncActions.Interfaces;
using ASG.Domain.ApolloSyncActions;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloSyncActions.Commands.CreateApolloSyncAction;

public class CreateApolloSyncActionCommandHandler :　IRequestHandler<CreateApolloSyncActionCommand, ErrorOr<ApolloSyncAction>>
{
    
    private readonly IApolloSyncActionRepository _apolloSyncActionRepository;

    public CreateApolloSyncActionCommandHandler(IApolloSyncActionRepository apolloSyncActionRepository)
    {
        _apolloSyncActionRepository = apolloSyncActionRepository;
    }

    public async Task<ErrorOr<ApolloSyncAction>> Handle(CreateApolloSyncActionCommand command, CancellationToken cancellationToken)
    {
        var apolloSyncAction = new ApolloSyncAction
        {
            anonymousRequests = new List<AnonymousRequest>()
        };
        
        await _apolloSyncActionRepository.AddApolloSyncActionAsync(apolloSyncAction);
        
        return apolloSyncAction;
    }
}