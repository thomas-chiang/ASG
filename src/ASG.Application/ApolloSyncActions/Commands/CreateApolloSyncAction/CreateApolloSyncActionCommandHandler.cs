using ASG.Application.ApolloSyncActions.Interfaces;
using ASG.Domain.ApolloSyncGaia1001FormOperation;
using ASG.Domain.Gaia1001Forms;
using ErrorOr;
using MediatR;

namespace ASG.Application.ApolloSyncActions.Commands.CreateApolloSyncAction;

public class CreateApolloSyncActionCommandHandler :　IRequestHandler<CreateApolloSyncActionCommand, ErrorOr<ApolloSyncGaia1001FormOperation>>
{
    
    private readonly IApolloSyncActionRepository _apolloSyncActionRepository;

    public CreateApolloSyncActionCommandHandler(IApolloSyncActionRepository apolloSyncActionRepository)
    {
        _apolloSyncActionRepository = apolloSyncActionRepository;
    }

    public async Task<ErrorOr<ApolloSyncGaia1001FormOperation>> Handle(CreateApolloSyncActionCommand command, CancellationToken cancellationToken)
    {
        // var apolloSyncAction = new ApolloSyncGaia1001FormOperation
        // {
        //     Gaia1001Form =new Gaia1001Form{};
        //     AnonymousRequests = new List<AnonymousRequest>()
        // };
        //
        // await _apolloSyncActionRepository.AddApolloSyncActionAsync(apolloSyncAction);
        //
        // return apolloSyncAction;
        throw new NotImplementedException();
    }
}