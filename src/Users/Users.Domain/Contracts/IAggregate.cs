namespace Users.Domain.Contracts;

public interface IAggregate : IAggregate<IEntityId>
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    List<IDomainEvent> PopDomainEvents();
}

public interface IAggregate<T>
{
}