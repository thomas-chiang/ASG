using ASG.Domain.Common;

namespace ASG.Application.Common.Interfaces;

public interface IDomainEventAdapter
{
    Task CollectDomainEvents(Entity entity);
    Task HandleDomainEvents();
}