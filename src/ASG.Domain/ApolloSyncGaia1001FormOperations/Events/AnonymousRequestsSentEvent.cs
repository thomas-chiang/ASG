using ASG.Domain.Common;

namespace ASG.Domain.ApolloSyncGaia1001FormOperations.Events;

public record AnonymousRequestsSentEvent(List<AnonymousRequest> AnonymousRequests) :　IDomainEvent;