using Users.Domain.Contracts;
using Users.Domain.Ids;

namespace Users.Domain.DomainEvents;

public sealed record UserVerifiedDomainEvent(UserId UserId) : IDomainEvent;