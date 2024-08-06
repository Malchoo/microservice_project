using Users.Domain.Contracts;
using Users.Domain.Ids;

namespace Users.Domain.DomainEvents;

public sealed record UserRestoredDomainEvent(UserId UserId) : IDomainEvent;