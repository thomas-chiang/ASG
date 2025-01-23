using ASG.Application.Common.Interfaces;
using ASG.Domain.Common;
using MediatR;

namespace ASG.Infrastructure.Common;

public class DomainEventAdapter : IDomainEventAdapter
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private IPublisher _publisher;

    public DomainEventAdapter(IPublisher publisher)
    {
        _publisher = publisher;
    }

    private List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();

        _domainEvents.Clear();

        return copy;
    }

    public Task CollectDomainEvents(Entity entity)
    {
        _domainEvents.AddRange(entity.PopDomainEvents());
        return Task.CompletedTask;
    }

    public async Task HandleDomainEvents()
    {
        var domainEvents = PopDomainEvents();
        foreach (var domainEvent in domainEvents) await _publisher.Publish(domainEvent);
    }
}