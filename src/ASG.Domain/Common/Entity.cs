namespace ASG.Domain.Common;

public abstract class Entity
{
    protected readonly List<IDomainEvent> DomainEvents = [];

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = DomainEvents.ToList();

        DomainEvents.Clear();

        return copy;
    }
}