using Users.Domain.Contracts;
using Users.Domain.Entities;

namespace Users.Domain.DomainEvents;

public sealed record MobileNumberChangedDomainEvent(User User) : IDomainEvent;