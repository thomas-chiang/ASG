using ASG.Domain.Common;

namespace ASG.Application.Common.Interfaces;

public interface IDomainEventContext
{
    Task Collect(Entity entity);
    Task Publish();
}