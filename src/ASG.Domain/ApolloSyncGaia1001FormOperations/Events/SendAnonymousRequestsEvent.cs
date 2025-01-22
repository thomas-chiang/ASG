using ASG.Domain.Common;

namespace ASG.Domain.ApolloSyncGaia1001FormOperations.Events;

public record SendAnonymousRequestsEvent(List<AnonymousRequest> AnonymousRequests) :　IDomainEvent;