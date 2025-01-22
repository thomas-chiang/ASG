using ASG.Application.Common.Interfaces;
using ASG.Domain.Common;
using MediatR;

namespace ASG.Infrastructure.Common;

public class DomainEventContext : IDomainEventContext
{
    protected readonly List<IDomainEvent> _domainEvents = [];
    private IPublisher _publisher;

    public DomainEventContext(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();

        _domainEvents.Clear();

        return copy;
    }

    public Task Collect(Entity entity)
    {
        _domainEvents.AddRange(entity.PopDomainEvents());
        return Task.CompletedTask;
    }

    public async Task Publish()
    {
        var domainEvents = PopDomainEvents();
        foreach (var domainEvent in domainEvents) await _publisher.Publish(domainEvent);
    }
}