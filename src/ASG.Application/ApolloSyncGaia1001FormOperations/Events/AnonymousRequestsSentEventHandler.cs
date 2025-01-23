using ASG.Application.Common.Interfaces;
using ASG.Domain.ApolloSyncGaia1001FormOperations.Events;
using MediatR;

namespace ASG.Application.ApolloSyncGaia1001FormOperations.Events;

public class AnonymousRequestsSentEventHandler : INotificationHandler<AnonymousRequestsSentEvent>
{
    private readonly IAnonymousRequestSender _anonymousRequestSender;

    public AnonymousRequestsSentEventHandler(IAnonymousRequestSender anonymousRequestSender)
    {
        _anonymousRequestSender = anonymousRequestSender;
    }

    public async Task Handle(AnonymousRequestsSentEvent notification, CancellationToken cancellationToken)
    {
        foreach (var request in notification.AnonymousRequests)
            await _anonymousRequestSender.SendAndUpdateAnonymousRequest(request);
    }
}