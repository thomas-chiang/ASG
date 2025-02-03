namespace ASG.Domain.Common.Events;

public record AnonymousRequestsSentEvent(List<AnonymousRequest> AnonymousRequests) :　IDomainEvent;